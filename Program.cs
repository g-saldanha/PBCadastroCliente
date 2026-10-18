using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PBCadastroCliente.Data;
using PBCadastroCliente.Services;
using PBCadastroCliente.Validators;
using MassTransit;

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

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ParanaBancoDb"));

builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .Build();

var clientesGroup = app.MapGroup("/api/v{version:apiVersion}/clientes")
    .WithApiVersionSet(versionSet);

app.UseHttpsRedirection();

clientesGroup.MapGet("/{id}", async (Guid id, IClienteService clienteService) =>
{
    var cliente = await clienteService.ObterPorIdAsync(id);

    return cliente is not null ? Results.Ok(cliente) : Results.NotFound(new { message = "Cliente não encontrado." });
});

clientesGroup.MapPost("/",
    async (CriarClienteRequest request, IValidator<CriarClienteRequest> validator, IClienteService clienteService) =>
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var cliente = await clienteService.CadastrarClienteAsync(request);

        return Results.Created($"/api/v1/clientes/{cliente.Id}",
            cliente);
    });

app.Run();

public record CriarClienteRequest(string Nome, string Cpf, string Email);