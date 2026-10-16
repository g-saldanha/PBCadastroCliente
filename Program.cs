using Asp.Versioning;
using FluentValidation;
using PBCadastroCliente.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi0
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddValidatorsFromAssemblyContaining<CriarClienteRequestValidator>();

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .Build();

var clientesGroup = app.MapGroup("/api/v{version:apiVersion}/clientes")
    .WithApiVersionSet(versionSet);

app.UseHttpsRedirection();

clientesGroup.MapGet("/{id}", (Guid id) => Results.Ok(new { Id = id, Nome = "Cliente Teste" }));

clientesGroup.MapPost("/", async (CriarClienteRequest request, IValidator<CriarClienteRequest> validator) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    var novoId = Guid.NewGuid();

    return Results.Created($"/api/v1/clientes/{novoId}",
        new { id = novoId, message = "Cliente cadastrado com sucesso" });
});

app.Run();

public record CriarClienteRequest(string Nome, string Cpf, string Email);