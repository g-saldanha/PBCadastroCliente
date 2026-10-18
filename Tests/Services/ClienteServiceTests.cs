using Microsoft.EntityFrameworkCore;
using PBCadastroCliente.Data;
using PBCadastroCliente.Services;
using Xunit;

namespace PBCadastroCliente.Tests.Services;

public class ClienteServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CadastrarAsync_DeveSalvarClienteNoBanco_E_RetornarClienteGerado()
    {
        var dbContext = GetDbContext();
        var service = new ClienteService(dbContext);
        var request = new CriarClienteRequest("Maria Silva", "123.456.789-00", "maria@email.com");

        var resultado = await service.CadastrarClienteAsync(request);

        Assert.NotNull(resultado);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Maria Silva", resultado.Nome);
        Assert.Equal("12345678900", resultado.Cpf);
        Assert.Equal("maria@email.com", resultado.Email);
        Assert.True(resultado.DataCadastro <= DateTime.UtcNow);

        var clienteNoBanco = await dbContext.Clientes.FindAsync(resultado.Id);
        
        Assert.NotNull(clienteNoBanco);
        Assert.Equal(resultado.Id, clienteNoBanco.Id);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarCliente_QuandoEleExistirNoBanco()
    {
        var dbContext = GetDbContext();
        var clienteExistente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = "João Silva",
            Cpf = "09876543211",
            Email = "joao@email.com",
            DataCadastro = DateTime.UtcNow
        };
        
        dbContext.Clientes.Add(clienteExistente);
        await dbContext.SaveChangesAsync();

        var service = new ClienteService(dbContext);

        var resultado = await service.ObterPorIdAsync(clienteExistente.Id);

        Assert.NotNull(resultado);
        Assert.Equal(clienteExistente.Id, resultado.Id);
        Assert.Equal(clienteExistente.Nome, resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarNull_QuandoClienteNaoExistir()
    {
        var dbContext = GetDbContext();
        var service = new ClienteService(dbContext);
        var idInexistente = Guid.NewGuid();

        var resultado = await service.ObterPorIdAsync(idInexistente);

        Assert.Null(resultado);
    }
}