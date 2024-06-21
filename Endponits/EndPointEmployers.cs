
using employers.Model;
using Microsoft.EntityFrameworkCore;

namespace employers.Endpoints
{
    public static class EmployerEndpoints
    {
        public static void MapEmployerEndpoints(this IEndpointRouteBuilder app)
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

            // delete employer form db
            app.MapDelete("/employers/{id:int}", async (int id, EmployerDb dbContext) =>
            {
                var employer = await dbContext.employer.FindAsync(id);
                if (employer == null)
                {
                    return Results.NotFound();
                }
                dbContext.employer.Remove(employer);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();

            });

            // update employer information
            app.MapPut("/employers/{id:int}", async (int id, UpdateEmployer updatedEmployer, EmployerDb dbContext) =>
            {
                var employer = await dbContext.employer.FindAsync(id);
                if (employer == null)
                {
                    return Results.NotFound();
                }
                // Create a new instance with updated values
                var updatedEntity = employer with
                {
                    employer_name = updatedEmployer.employer_name,
                    employer_feild = updatedEmployer.employer_feild
                    // Add other properties to update as needed
                };

                // Update the entity in the DbContext
                dbContext.Entry(employer).CurrentValues.SetValues(updatedEntity);

                await dbContext.SaveChangesAsync();

                return Results.Ok(updatedEntity);
            });
            // login new user 
            // create new employer with db
            app.MapPost("/employer", async (NewEmployerLogin userModel, EmployerDb dbContext) =>
                {

                    var login = new UserModel
                    (
                        userModel.id,
                        userModel.phone,
                        userModel.password
                    );

                    dbContext.auth.Add(login);
                    await dbContext.SaveChangesAsync();

                    return Results.Created($"/employer/{login.phone}", login);
                });
        }
    }
}
