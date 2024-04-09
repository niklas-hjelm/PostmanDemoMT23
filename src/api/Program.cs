using api.DataAccess;
using api.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PeopleContext>(o =>
    o.UseInMemoryDatabase("People")
);

var app = builder.Build();

app.MapGet("/people", (PeopleContext context) =>
{
    return context.People.ToList();
});

app.MapGet("/people/{id}", async (int id, PeopleContext context) =>
{
    return await context.People.FindAsync(id) is Person person
        ? Results.Ok(person)
        : Results.NotFound();
});

app.MapPost("/people", async (Person person, PeopleContext context) =>
{
    await context.People.AddAsync(person);
    await context.SaveChangesAsync();
    return Results.Created($"/people/{person.Id}", person);
});

app.MapPut("/people/{id}", async (int id, Person person, PeopleContext context) =>
{
    if (id != person.Id)
    {
        return Results.BadRequest("Id mismatch");
    }

    context.People.Update(person);
    await context.SaveChangesAsync();
    return Results.Ok(person);
});

app.MapDelete("/people/{id}", async (int id, PeopleContext context) =>
{
    var person = await context.People.FindAsync(id);
    if (person is null)
    {
        return Results.NotFound();
    }

    context.People.Remove(person);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/people/{id}/email", async (int id, string email, PeopleContext context) =>
{
    var person = await context.People.FindAsync(id);
    if (person is null)
    {
        return Results.NotFound();
    }

    person.Email = email;
    await context.SaveChangesAsync();
    return Results.Ok(person);
});

app.Run();