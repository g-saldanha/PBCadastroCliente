using PBCadastroCliente.Data;

namespace PBCadastroCliente.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;
    
    public ClienteService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        return await _db.Clientes.FindAsync(id); 
    }

    public Task<Cliente> CadastrarClienteAsync(CriarClienteRequest request)
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
        _db.SaveChangesAsync();
        
        //Mensageria
        
        return Task.FromResult(cliente);
    }
}