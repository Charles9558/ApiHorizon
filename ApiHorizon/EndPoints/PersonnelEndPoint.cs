using ApiHorizon.Data;
using ApiHorizon.Models;
using Dapper;

namespace ApiHorizon.EndPoints
{
    public static class PersonnelEndPoint
    {
        public static RouteGroupBuilder MapPersonnelEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/personnel");

            //MapPost

            Group.MapPost("/create", async (Personnel personnel) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    string statut = "actif";
                    var sql = @"INSERT INTO 
                                personnel(matricul,nom,prenom,email,telephone,profilcolor,pseudo,pass,rol,statut,poste,config) 
                                VALUES(@Matricul,@Nom,@Prenom,@Email,@Telephone,@Profilcolor,@Pseudo,@Pass,@Rol,@Statut,@Post,@config);";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Matricul = personnel.Matricul, Nom = personnel.Nom, Prenom = personnel.Prenom, Email = personnel.Email, Telephone = personnel.Telephone, Profilcolor = personnel.ProfilColor, Pseudo = personnel.Pseudo, Pass = personnel.Pass, Rol = personnel.Rol, Statut =statut, Post = personnel.poste.poste_id, config=personnel.config.config_id });
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

            //Group.MapGet("/list", async () =>
            //{
            //    await using var cnn = DbConnection.ConnectDB();
            //    try
            //    {
            //        cnn.Open();
            //        var personnels = await cnn.QueryAsync("SELECT * FROM personnel;");

            //        if (personnels == null || !personnels.Any())
            //        {
            //            return Results.NotFound("Personnels not found");
            //        }
            //        else
            //        {
            //            return Results.Ok(personnels.ToArray());
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return Results.Problem("An error occurred while processing the request.");
            //        throw;
            //    }
            //});

            Group.MapGet("/list/actif", async () =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    string statut = "actif";
                    var sql = @"SELECT 
                                    matricul, nom, prenom, email, telephone, profilcolor, pseudo, pass, rol, statut,
                                    poste_id, poste_intitule,
                                    departement_id, departement_intitule,
                                    config_id, config_arrive, config_depart, config_jours, config_theme
                                FROM personnel
                                INNER JOIN poste ON personnel.poste = poste_id
                                INNER JOIN departement ON poste.poste_departement = departement_id
                                INNER JOIN config ON personnel.config = config_id
                                WHERE statut=@Statut;";
                                
                    var parameters = new { Statut = statut };
                    cnn.Open();
                    var personnels = await cnn.QueryAsync<Personnel, Poste, Departement, Config, Personnel>(
                        sql,
                        (personnel, poste, departement, config) =>
                        {
                            personnel.poste = poste;
                            poste.poste_departement = departement;
                            personnel.config = config;
                            return personnel;
                        },
                        param: parameters,
                        splitOn: "poste_id,departement_id,config_id");

                    if (personnels == null || !personnels.Any())
                    {
                        return Results.NotFound("Personnels not found");
                    }
                    else
                    {
                        return Results.Ok(personnels.ToArray());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });

            //Group.MapGet("/person/{matricul}", async (string matricul) =>
            //{
            //    await using var cnn = DbConnection.ConnectDB();
            //    try
            //    {
            //        var sql = "SELECT * FROM personnel WHERE matricul=@Matricul;";
            //        cnn.Open();
            //        var personnel = await cnn.QueryAsync(sql, new { Matricul = matricul });

            //        if (personnel == null || !personnel.Any())
            //        {
            //            return Results.NotFound($"Personnel with matricul {matricul} not found");
            //        }
            //        else { 
            //            return Results.Ok(personnel);
            //        }


            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return Results.Problem("An error occurred while processing the request.");
            //        throw;
            //    }
            //});

            Group.MapGet("/connexion/{pseudo}/{pass}", async (string pseudo, string pass) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    string statut = "actif";
                    var sql = @"SELECT 
                                    matricul, nom, prenom, email, telephone, profilcolor, pseudo, pass, rol, statut,
                                    poste_id, poste_intitule,
                                    departement_id, departement_intitule,
                                    config_id, config_arrive, config_depart, config_jours, config_theme
                                FROM personnel
                                INNER JOIN poste ON personnel.poste = poste_id
                                INNER JOIN departement ON poste.poste_departement = departement_id
                                INNER JOIN config ON personnel.config = config_id
                                WHERE pseudo=@Pseudo and pass=@Pass and statut=@Statut;";
                                
                    var parameters = new { Pseudo = pseudo, Pass = pass, Statut = statut };
                    cnn.Open();

                    var personnel = await cnn.QueryAsync<Personnel, Poste, Departement, Config, Personnel>(
                        sql,
                        (personnel, poste, departement, config) =>
                        {
                            personnel.poste = poste;
                            poste.poste_departement = departement;
                            personnel.config = config;
                            return personnel;
                        },
                        param: parameters,
                        splitOn: "poste_id,departement_id,config_id");

                    if (personnel == null || !personnel.Any())
                    {
                        return Results.NotFound($"Person not found");
                    }
                    else
                    {
                        return Results.Ok(personnel);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });

            Group.MapGet("/username/{pseudo}", async (string pseudo) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    string statut = "actif";
                    var sql = @"SELECT matricul,email,pseudo FROM personnel WHERE pseudo=@Pseudo and statut=@Statut;";
                    cnn.Open();
                    var personnel = await cnn.QueryAsync(sql, new { Pseudo = pseudo, Statut = statut });

                    if (personnel == null || !personnel.Any())
                    {
                        return Results.NotFound($"Person not found");
                    }
                    else
                    {
                        return Results.Ok(personnel);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });

            //MapPut

            Group.MapPut("/newpass", async (Personnel personnel) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "UPDATE personnel SET pass=@Pass WHERE matricul=@Matricul;";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { pass = personnel.Pass, Matricul = personnel.Matricul });
                    return Results.Ok();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });
            Group.MapPut("/userupdate", async (Personnel personnel) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "UPDATE personnel SET email=@Email,pseudo=@Pseudo,telephone=@Telephone,pass=@Pass WHERE matricul=@Matricul;";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Email=personnel.Email, Pseudo=personnel.Pseudo,Telephone=personnel.Telephone,pass = personnel.Pass, Matricul = personnel.Matricul });
                    return Results.Ok();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return Results.Problem("An error occurred while processing the request.");
                    throw;
                }
            });
            //Group.MapPut("desactive/{matricul}", async (string matricul, Personnel personnel) =>
            //{
            //    await using var cnn = DbConnection.ConnectDB();
            //    try
            //    {
            //        var sql = "UPDATE personnel SET statut=@Statut WHERE matricul=@Matricul;";
            //        cnn.Open();
            //        await cnn.QueryAsync(sql, new { Statut = personnel.Statut, Matricul = matricul });
            //        return Results.Ok();

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return Results.Problem("An error occurred while processing the request.");
            //        throw;
            //    }
            //});

            //Group.MapPut("active/{matricul}", async (string matricul, Personnel personnel) =>
            //{
            //    await using var cnn = DbConnection.ConnectDB();
            //    try
            //    {
            //        var sql = "UPDATE personnel SET statut=@Statut WHERE matricul=@Matricul;";
            //        cnn.Open();
            //        await cnn.QueryAsync(sql, new { Statut = personnel.Statut, Matricul = matricul });
            //        return Results.Ok();

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return Results.Problem("An error occurred while processing the request.");
            //        throw;
            //    }
            //});

            //Group.MapPut("profil/{matricul}", async (string matricul, Personnel personnel) =>
            //{
            //    await using var cnn = DbConnection.ConnectDB();
            //    try
            //    {
            //        var sql = "UPDATE personnel SET profil=@Profil WHERE matricul=@Matricul;";
            //        cnn.Open();
            //        await cnn.QueryAsync(sql, new { Profil = personnel.Profil, Matricul = matricul });
            //        return Results.Ok();

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return Results.Problem("An error occurred while processing the request.");
            //        throw;
            //    }
            //});

            return Group;
        }
    }
}
