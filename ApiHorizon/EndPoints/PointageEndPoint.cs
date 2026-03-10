using ApiHorizon.Data;
using ApiHorizon.Models;
using Dapper;
using System;

namespace ApiHorizon.EndPoints
{
    public static class PointageEndPoint
    {
        public static RouteGroupBuilder MapPointageEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/pointage");
            //MapPost

            Group.MapPost("/create", async (Pointage pointage) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "INSERT INTO pointage(jour,arrive,personnel) VALUES(@Jour,@Arrive,@Person);";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Jour=pointage.pointage_jour, Arrive=pointage.pointage_arrive,Depart=pointage.pointage_depart,Person=pointage.pointage_personnel.Matricul});
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

            Group.MapGet("/list/{date}", async (string date) =>
            {

                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    DateTime Date = DateOnly.Parse(date).ToDateTime(TimeOnly.MinValue);
                    var sql = @"SELECT 
                                    pointage_id, pointage_jour, pointage_arrive, pointage_depart,
                                    matricul, nom, prenom, email, telephone, profilcolor, pseudo, pass, rol, statut,
                                    poste_id, poste_intitule,
                                    departement_id, departement_intitule,
                                    config_id, config_arrive, config_depart, config_jours, config_theme
                                FROM pointage
                                INNER JOIN personnel ON pointage_personnel = personnel.matricul
                                INNER JOIN poste ON personnel.poste = poste_id
                                INNER JOIN departement ON poste.poste_departement = departement_id
                                INNER JOIN config ON personnel.config = config_id
                                WHERE pointage_jour=@date;";

                    var parameters = new {date= Date};
                    cnn.Open();
                    var piontages = await cnn.QueryAsync<Pointage, Personnel, Poste, Departement, Config, Pointage > (
                        sql,
                        (pointage, personnel, poste, departement, config) =>
                        {
                            pointage.pointage_personnel = personnel;
                            personnel.poste = poste;
                            poste.poste_departement = departement;
                            personnel.config = config;
                            return pointage;
                        },
                        param: parameters,
                        splitOn: "matricul,poste_id,departement_id,config_id");

                    if (piontages == null || !piontages.Any())
                    {
                        return Results.NotFound("Piontages not found");
                    }
                    else
                    {
                        return Results.Ok(piontages.ToArray());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });

            return Group;
        }
    }
}