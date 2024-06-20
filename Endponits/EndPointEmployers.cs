
using employers.Model;
using Microsoft.EntityFrameworkCore;

namespace employers.Endpoints
{
    public static class EmployerEndpoints
    {
        public static void MapEmployerEndpoints(this IEndpointRouteBuilder app, List<EmploerModel> employers)
        {
            // create new employer with db
            app.MapPost("/employers", async (CreateEmployer createEmployerDto, EmployerDb dbContext) =>
                {

                    var employer = new EmploerModel
                    (
                        createEmployerDto.id,
                        createEmployerDto.employer_name,
                        createEmployerDto.employer_feild
                    );

                    dbContext.employer.Add(employer);
                    await dbContext.SaveChangesAsync();

                    return Results.Created($"/employers/{employer.id}", employer);
                });


            // get employers from db
            app.MapGet("/employers", async (EmployerDb dbContext) =>
                {
                    var employers = await dbContext.employer.ToListAsync();
                    return Results.Ok(employers);
                });
            app.MapGet("/employers/{id:int}", async (int id, EmployerDb dbContext) =>
                    {
                        var employer = await dbContext.employer.FindAsync(id);
                        if (employer == null)
                        {
                            return Results.NotFound();
                        }
                        return Results.Ok(employer);
                    });

            // update employer info



            //             app.MapPut("/employers/{id:int}", async (int id, UpdateEmployer updateEmployer, EmployerDb dbContext) =>
            // {
            //     var employer = await dbContext.employer.FindAsync(id);
            //     if (employer == null)
            //     {
            //         return Results.NotFound();
            //     }
            //     employer.employer_name
            //     await dbContext.SaveChangesAsync();

            //     return Results.NoContent(); // Return 204 No Content to indicate success
            // });


        }
    }
}
