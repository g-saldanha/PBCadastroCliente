using FluentValidation.TestHelper;
using PBCadastroCliente.Validators;
using Xunit;

namespace PBCadastroCliente.Tests.Validators;

public class CriarClienteRequestValidatorTests
{
    private readonly CriarClienteRequestValidator _validator;

    public CriarClienteRequestValidatorTests()
    {
        _validator = new CriarClienteRequestValidator();
    }

    [Fact]
    public void Validar_DeveRetornarErro_QuandoNomeForVazio()
    {
        var request = new CriarClienteRequest("", "03350284042", "teste@email.com");

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Nome)
              .WithErrorMessage("O nome é Obrigatório");
    }

    [Fact]
    public void Validar_DeveRetornarErro_QuandoNomeForMenorQue3Caracteres()
    {
        var request = new CriarClienteRequest("Zé", "03350284042", "teste@email.com");

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Nome)
              .WithErrorMessage("O nome deve ter no mínimo 3 caracteres");
    }

    [Theory]
    [InlineData("")]
    [InlineData("emailinvalido")]
    [InlineData("teste@")]
    [InlineData("@dominio.com")]
    public void Validar_DeveRetornarErro_QuandoEmailForInvalido(string emailInvalido)
    {
        var request = new CriarClienteRequest("João Silva", "03350284042", emailInvalido);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("11111111111")]
    [InlineData("ABC45678901")]
    [InlineData("03350284041")]
    public void Validar_DeveRetornarErro_QuandoCpfForInvalido(string cpfInvalido)
    {
        var request = new CriarClienteRequest("João Silva", cpfInvalido, "joao@email.com");

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    public void Validar_NaoDeveRetornarErro_QuandoTodosOsDadosForemValidos()
    {
        var request = new CriarClienteRequest("João Silva", "52998224725", "joao@email.com");

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}