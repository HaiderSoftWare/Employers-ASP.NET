
using employers.Model;
using Microsoft.EntityFrameworkCore;

namespace employers.Endpoints
{
    public static class EmployerEndpoints
    {
        public static void MapEmployerEndpoints(this IEndpointRouteBuilder app, List<EmploerModel> employers)
        {
            app.MapPost("/employer", (EmploerModel emploerModel) =>
            {
                EmploerModel emploer = new(emploerModel.id, emploerModel.employer_name, emploerModel.employer_feild);
                employers.Add(emploer);
                return Results.Ok(emploer);
            });

            // get employers from db
            app.MapGet("/employers", async (EmployerDb dbContext) =>
{
    var employers = await dbContext.employer.ToListAsync();
    return Results.Ok(employers);
});

            // app.MapGet("/employers/{id}", (int id) =>
            // employers.Find(employer => employer.id == id));
            app.MapGet("/employers/{id:int}", async (int id, EmployerDb dbContext) =>
{
    var employer = await dbContext.employer.FindAsync(id);
    if (employer == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(employer);
});


            // login employer end point
            app.MapPost("/employer/auth", (EmployerAuth employerAuth) =>
            {
                EmployerAuth emploer = new(employerAuth.phone, employerAuth.password);
                return Results.Ok(emploer);
            });

            // update employer info
            app.MapPut("/employers/{id}", (int id, UpdateEmployer updateEmployer) =>
            {

                int index = employers.FindIndex(employer => employer.id == id);
                employers[index] = new EmploerModel(
                    id,
                    updateEmployer.employerName,
                    updateEmployer.employerFeild
                );
            });

        }
    }
}
