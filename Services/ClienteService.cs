using MassTransit;
using PBCadastroCliente.Data;
using ParanaBanco.Messages;

namespace PBCadastroCliente.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public ClienteService(AppDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        return await _db.Clientes.FindAsync(id); 
    }

    public async Task<Cliente> CadastrarClienteAsync(CriarClienteRequest request)
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Cpf = request.Cpf.Replace(".", "").Replace("-", ""),
            Email = request.Email,
            DataCadastro = DateTime.UtcNow,
        };
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        
        Console.WriteLine($"\n[MS1] Preparando para enviar evento ClienteCadastrado para o RabbitMQ. ClienteID: {cliente.Id}");
        
        await _publishEndpoint.Publish(new ClienteCadastrado(cliente.Id, cliente.Cpf, cliente.Email));
        
        Console.WriteLine($"[MS1] Evento enviado com sucesso!\n");
        
        return cliente;
    }
}