
using employers.Model;

namespace employers.Endpoints
{
    public static class EmployerEndpoints
    {
        public static void MapEmployerEndpoints(this IEndpointRouteBuilder app, List<EmploerModel> employers)
        {
            app.MapPost("/employer", (EmploerModel emploerModel) =>
            {
                EmploerModel emploer = new(emploerModel.id, emploerModel.employerName, emploerModel.employerFeild);
                employers.Add(emploer);
                return Results.Ok(emploer);
            });

            app.MapGet("/employers", () =>
            {
                return Results.Ok(employers);
            });

            app.MapGet("/employers/{id}", (int id) =>
            employers.Find(employer => employer.id == id));


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
