using PBCadastroCliente.Data;

namespace PBCadastroCliente.Services;

public interface IClienteService
{
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task<Cliente> CadastrarClienteAsync(CriarClienteRequest request);
}