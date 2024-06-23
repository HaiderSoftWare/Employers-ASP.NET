
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using employers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            // app.MapGet("/employers", async (EmployerDb dbContext) =>
            //     {
            //         var employers = await dbContext.employer.ToListAsync();
            //         return Results.Ok(employers);
            //     });
            app.MapGet("/employers", async (EmployerDb dbContext, HttpRequest request) =>
{
    // Retrieve query parameters
    string? employer_name = request.Query["employer_name"];
    string? employer_feild = request.Query["employer_feild"];

    // Create a queryable collection from the employers table
    var query = dbContext.employer.AsQueryable();

    // Apply filters if query parameters are provided
    if (!string.IsNullOrEmpty(employer_name))
    {
        query = query.Where(e => e.employer_name.Contains(employer_name));
    }

    if (!string.IsNullOrEmpty(employer_feild))
    {
        query = query.Where(e => e.employer_feild.Contains(employer_feild));
    }    // Execute the query and get the results
    var employers = await query.ToListAsync();

    // Return the results
    return Results.Ok(employers);
});

            // get employer by id
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

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("PP3DIPgqoaoevmc9tWLxreFxNf1laJAB1HgDDmvQ+xA=");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
            new Claim(ClaimTypes.Name, login.id.ToString()),
            new Claim(ClaimTypes.MobilePhone, login.phone)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var response = new
                {
                    Status = true,
                    Message = "Login successfully",
                    Token = tokenString,
                    Data = login
                };

                return Results.Ok(response);
            });


            // get all users from db
            app.MapGet("/users", async (EmployerDb dbContext) =>
                {
                    var employers = await dbContext.auth.ToListAsync();
                    return Results.Ok(employers);
                });
            // delete user form db
            app.MapDelete("/user/{id:int}", async (int id, EmployerDb dbContext) =>
        {
            var employer = await dbContext.auth.FindAsync(id);
            if (employer == null)
            {
                return Results.NotFound();
            }
            dbContext.auth.Remove(employer);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();

        });
        }
    }
}
