using ApiHorizon.Data;
using ApiHorizon.Models;
using Dapper;
using System.Security.Cryptography;

namespace ApiHorizon.EndPoints
{
    public static class PosteEndPoint
    {
        public static RouteGroupBuilder MapPosteEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/poste");

            //MapPost

            Group.MapPost("/create/{id:int}", async (int id,Poste poste) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "INSERT INTO poste(poste_intitule,poste_departement) VALUES(@Intitule,@departement);";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Intitule = poste.poste_intitule, departement=id});
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
                                    poste_id, poste_intitule, 
                                    departement_id, departement_intitule 
                                FROM  poste INNER JOIN departement ON poste_departement = departement_id";
                    cnn.Open();
                    var postes = await cnn.QueryAsync<Poste,Departement,Poste>(sql,(poste,departement) => {
                        poste.poste_departement= departement;
                        return poste;
                    },
                    splitOn: "departement_id");
                    if (postes == null || !postes.Any())
                    {
                        return Results.NotFound("Postes not found");
                    }
                    else
                    {
                        return Results.Ok(postes.ToArray());
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
