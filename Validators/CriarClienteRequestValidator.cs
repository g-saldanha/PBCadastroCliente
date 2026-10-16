using FluentValidation;

namespace PBCadastroCliente.Validators;

public class CriarClienteRequestValidator : AbstractValidator<CriarClienteRequest>
{
    public CriarClienteRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é Obrigatório")
            .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O Email é obrigatório")
            .EmailAddress().WithMessage("O Formato do email é inválido");
        
        RuleFor(x => x.Cpf)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O Cpf é obrigatório")
            .Length(11).WithMessage("O CPF deve conter 11 caracteres")
            .Matches(@"^\d{11}$").WithMessage("O CPF deve conter apenas números")
            .Must(ValidarCpfCalculo).WithMessage("O CPF fornecido é inválido");
    }

    private bool ValidarCpfCalculo(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        }

        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        string digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        }

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}