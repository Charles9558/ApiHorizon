using ApiHorizon.Data;
using ApiHorizon.Models;
using Dapper;

namespace ApiHorizon.EndPoints
{
    public static class DepartementEndPoint
    {
        public static RouteGroupBuilder MapDepartementEndPoint(this WebApplication app)
        {
            var Group = app.MapGroup("/departement");

            //MapPost

            Group.MapPost("/create", async (Departement departement) =>
            {
                await using var cnn = DbConnection.ConnectDB();
                try
                {
                    var sql = "INSERT INTO departement(departement_intitule) VALUES(@Intitule);";
                    cnn.Open();
                    await cnn.QueryAsync(sql, new { Intitule = departement.departement_intitule});
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
                    cnn.Open();
                    var departements = await cnn.QueryAsync("SELECT * FROM departement;");

                    if (departements == null || !departements.Any())
                    {
                        return Results.NotFound("Departements not found");
                    }
                    else
                    {
                        return Results.Ok(departements.ToArray());
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
