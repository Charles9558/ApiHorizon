using ApiHorizon.Data;
using ApiHorizon.Models;
using Dapper;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiHorizon.EndPoints
{
    public static class ComgeEndPoint
    {
        public static RouteGroupBuilder MapComgeEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/conge");
            //MapPost

            Group.MapPost("/create", async (Conge conge) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = @"INSERT INTO conge(conge_id,conge_date_depart,conge_date_retour,conge_motif,conge_motif_description,conge_statut,conge_personnel) 
                                VALUES(@id,@date_depart,@date_retour,@motif,@motif_description,@statut,@personnel);";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { id =conge.conge_id, date_depart =conge.conge_date_depart, date_retour=conge.conge_date_retour, motif=conge.conge_motif, motif_description =conge.conge_motif_description, statut =conge.conge_statut, personnel =conge.conge_personnel.Matricul});
                    return Results.Created();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });

            //MapGet

            Group.MapGet("/list", async () =>
            {

                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = @"SELECT 
                                    conge_id,conge_date_depart,conge_date_retour,conge_motif,conge_motif_description,conge_statut,
                                    matricul, nom, prenom, email, telephone, profilcolor, pseudo, pass, rol, statut,
                                    poste_id, poste_intitule,
                                    departement_id, departement_intitule,
                                    config_id, config_arrive, config_depart, config_jours, config_theme
                                FROM conge
                                INNER JOIN personnel ON conge_personnel = personnel.matricul
                                INNER JOIN poste ON personnel.poste = poste_id
                                INNER JOIN departement ON poste.poste_departement = departement_id
                                INNER JOIN config ON personnel.config = config_id;";

                    cnn.Open();
                    var conges = await cnn.QueryAsync<Conge, Personnel, Poste, Departement, Config, Conge>(
                        sql,
                        (conge, personnel, poste, departement, config) =>
                        {
                            conge.conge_personnel = personnel;
                            personnel.poste = poste;
                            poste.poste_departement = departement;
                            personnel.config = config;
                            return conge;
                        },
                        splitOn: "matricul,poste_id,departement_id,config_id");

                    if (conges == null || !conges.Any())
                    {
                        return Results.NotFound("Conges not found");
                    }
                    else
                    {
                        return Results.Ok(conges.ToArray());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });
            Group.MapGet("/list/{matricul}", async (string matricul) =>
            {

                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = @"SELECT 
                                    conge_id,conge_date_depart,conge_date_retour,conge_motif,conge_motif_description,conge_statut,
                                    matricul, nom, prenom, email, telephone, profilcolor, pseudo, pass, rol, statut,
                                    poste_id, poste_intitule,
                                    departement_id, departement_intitule,
                                    config_id, config_arrive, config_depart, config_jours, config_theme
                                FROM conge
                                INNER JOIN personnel ON conge_personnel = personnel.matricul
                                INNER JOIN poste ON personnel.poste = poste_id
                                INNER JOIN departement ON poste.poste_departement = departement_id
                                INNER JOIN config ON personnel.config = config_id
                                WHERE personnel.matricul=@Matricul;";

                    var parameters = new { Matricul = matricul };

                    cnn.Open();
                    var conges = await cnn.QueryAsync<Conge, Personnel, Poste, Departement, Config, Conge>(
                        sql,
                        (conge, personnel, poste, departement, config) =>
                        {
                            conge.conge_personnel = personnel;
                            personnel.poste = poste;
                            poste.poste_departement = departement;
                            personnel.config = config;
                            return conge;
                        },
                        param: parameters,
                        splitOn: "matricul,poste_id,departement_id,config_id");

                    if (conges == null || !conges.Any())
                    {
                        return Results.NotFound("Conges not found");
                    }
                    else
                    {
                        return Results.Ok(conges.ToArray());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });
            Group.MapPut("/update/{statut}", async (string statut, Conge conge) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "UPDATE conge SET conge_statut=@Statut WHERE conge_id=@Id;";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Statut=statut, Id = conge.conge_id });
                    return Results.Ok();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An erro occurred while processing the request.");
                    throw;
                }
            });

            return Group;
        }
    }
}
