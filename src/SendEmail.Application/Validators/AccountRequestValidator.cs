using FluentValidation;
using SendEmail.Domain.DTOs.Request;
using System.Text.RegularExpressions;

namespace SendEmail.Application.Validators;

public class AccountRequestValidator : AbstractValidator<AccountRequestDTO>
{
    public AccountRequestValidator()
    {
        RuleFor(x => x.NomeCompleto)
            .Must(nome => ValidaNome(nome)).WithMessage("Nome inválido")
            .MinimumLength(3).WithMessage("Nome deve ter no minimo 3 caracteres")
            .MaximumLength(200).WithMessage("Nome deve ter no maximo 200 caracteres")
            .NotEmpty().WithMessage("Nome deve ter no minimo 3 caracteres")
            .NotNull().WithMessage("Nome deve ter no minimo 3 caracteres")
            .OverridePropertyName("NomeCompleto")
            .WithErrorCode("1");

        RuleFor(x => x.Telefone)
            .Must(x => ValidaNumeroTelefone(x)).WithMessage("Telefone informado está com valor invalido")
            .NotEmpty().WithMessage("Telefone deve ser preenchido")
            .NotNull().WithMessage("Telefone deve ser preenchido")
            .MinimumLength(11).WithMessage("Telefone informado está com valor invalido")
            .MaximumLength(11).WithMessage("Telefone informado está com valor invalido")
            .OverridePropertyName("Telefone")
            .WithErrorCode("2");

        RuleFor(x => x.Email)
            .MinimumLength(8).WithMessage("Email deve ter no minimo 8 caracteres")
            .MaximumLength(200).WithMessage("Email deve ter no maximo 200 caracteres")
            .NotEmpty().WithMessage("Email deve ser preenchido")
            .NotNull().WithMessage("Email deve ser preenchido")
            .Must(p => Regex.IsMatch(p, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                .WithMessage("Email está com o formato incorreto")
            .OverridePropertyName("Email")
            .WithErrorCode("3");

        RuleFor(x => x.Senha)
            .MinimumLength(8).WithMessage("Senha deve ter no minimo 8 caracteres")
            .MaximumLength(100).WithMessage("Senha deve ter no maximo 100 caracteres")
            .NotEmpty().WithMessage("Senha deve ser preenchida")
            .NotNull().WithMessage("Senha deve ser preenchida")
            .Equal(x => x.ConfirmacaoSenha).WithMessage("Senhas devem ser iguais")
            .Must(p => Regex.IsMatch(p, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).{6,}$"))
                .WithMessage("Senha deve ter letras maiusculas, minusculas e numeros")
            .OverridePropertyName("Passord")
            .WithErrorCode("5");

        RuleFor(x => x.ConfirmacaoSenha)
            .MinimumLength(8).WithMessage("Confirmação de senha deve ter no minimo 8 caracteres")
            .MaximumLength(100).WithMessage("Confirmação de senha deve ter no maximo 100 caracteres")
            .NotEmpty().WithMessage("Confirmação de senha deve ser preenchida")
            .NotNull().WithMessage("Confirmação de senha deve ser preenchida")
            .Equal(x => x.Senha).WithMessage("Senhas devem ser iguais")
            .Must(p => Regex.IsMatch(p, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z]).{6,}$"))
                .WithMessage("Senha deve ter letras maiusculas, minusculas, numeros e caracteres especiais EX: [!@#$%.]")
            .OverridePropertyName("ConfirmPassword")
            .WithErrorCode("6");

        RuleFor(x => x.Documento)
            .Must(doc => IsCpfOrCnpj(doc)).WithMessage("Documento deve ser um CPF ou CNPJ válido.")
            .MinimumLength(11).WithMessage("Documento deve ter no minimo 11 caracteres")
            .MaximumLength(14).WithMessage("Documento deve ter no maximo 14 caracteres")
            .NotEmpty().WithMessage("Documento deve ser preenchido")
            .NotNull().WithMessage("Documento deve ser preenchido")
            .OverridePropertyName("Documento")
            .WithErrorCode("7");
    }

    private bool ValidaNumeroTelefone(string numeroTelefone)
    {
        if (numeroTelefone.Length != 11)
            return false;

        if (!numeroTelefone.All(char.IsDigit))
            return false;

        if (string.IsNullOrEmpty(numeroTelefone))
            return false;

        int ddd = int.Parse(numeroTelefone.Substring(0, 2));
        if (ddd < 11 || ddd > 99)
            return false;


        string number = numeroTelefone.Substring(2);
        if (ContemNumerosRepetidos(number))
            return false;

        return true;
    }

    private static bool ContemNumerosRepetidos(string number)
    {
        return number.All(c => c == number[0]);
    }

    private bool IsValidCep(string cep)
        => cep.All(char.IsDigit);
    private bool IsCpfOrCnpj(string documento)
        => IsValidCpf(documento) || IsValidCnpj(documento);

    private bool ValidaNome(string nome)
        => nome.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));

    private bool IsValidCpf(string cpf)
    {
        int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        cpf = cpf.Trim().Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;

        for (int j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                return false;

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }

    private bool IsValidCnpj(string cnpj)
    {
        int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        if (cnpj.Length != 14)
            return false;

        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        string digito = resto.ToString();
        tempCnpj = tempCnpj + digito;
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;

        digito = digito + resto.ToString();

        return cnpj.EndsWith(digito);
    }
}
