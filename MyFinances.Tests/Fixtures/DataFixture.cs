using Bogus;
using Bogus.Extensions.Brazil;
using MyFinances.Domain.Models;

namespace MyFinances.Tests.Fixtures;

public class DataFixture
{
    public static List<Usuario> ObterUsuarios(int quantidade, bool useNovoUsuario = false)
    {
        return ObterUsuariosFalsos(useNovoUsuario).Generate(quantidade);
    }

    private static Faker<Usuario> ObterUsuariosFalsos(bool useNovoUsuario)
    {
        var seed = 0;
        
        if (useNovoUsuario)
            seed = Random.Shared.Next(10, int.MaxValue);
        
        return new Faker<Usuario>()
            .RuleFor(u => u.Id, Guid.NewGuid)
            .RuleFor(u => u.NomeCompleto, faker => faker.Person.FullName)
            .RuleFor(u => u.CPF, faker => faker.Person.Cpf(false))
            .RuleFor(u => u.Email, faker => faker.Person.Email)
            .RuleFor(u => u.Senha, faker => faker.Internet.Password())
            .RuleFor(u => u.SenhaHash, (faker, u) => BCrypt.Net.BCrypt.HashPassword(u.Senha))
            .RuleFor(u => u.DataDeNascimento, faker => faker.Person.DateOfBirth)
            .RuleFor(u => u.IsEmailVerificado, () => false)
            .RuleFor(u => u.Token, () => null)
            .RuleFor(u => u.ValidadeToken, () => null)
            .RuleFor(u => u.IsAdministrador, () => false)
            .RuleFor(u => u.IdEndereco, Guid.NewGuid)
            .RuleFor(u => u.IdContato, Guid.NewGuid)
            .RuleFor(u => u.Endereco, (faker, u) => ObterEnderecoFalso(u.IdEndereco))
            .RuleFor(u => u.Contato, (faker, u) => ObterContatoFalso(u.IdContato))
            .UseSeed(seed);
            
    }
    private static Faker<Endereco> ObterEnderecoFalso(Guid idEndereco)
    {
        return new Faker<Endereco>()
            .RuleFor(u => u.Id, idEndereco)
            .RuleFor(u => u.Rua, faker => faker.Address.StreetName())
            .RuleFor(u => u.Numero, faker => faker.Address.BuildingNumber())
            .RuleFor(u => u.Complemento, faker => faker.Address.Random.Word())
            .RuleFor(u => u.Bairro, faker => faker.Address.Random.Word())
            .RuleFor(u => u.Cidade, faker => faker.Address.City())
            .RuleFor(u => u.Estado, faker => faker.Address.State())
            .RuleFor(u => u.CEP, faker => faker.Address.ZipCode("#####-###"));
    }
    private static Faker<Contato> ObterContatoFalso(Guid IdContato)
    {
        return new Faker<Contato>()
            .RuleFor(u => u.Id, IdContato)
            .RuleFor(u => u.TelefoneFixo, faker => faker.Phone.PhoneNumber())
            .RuleFor(u => u.Celular, faker => faker.Phone.PhoneNumber());
    }

    public static List<TransacaoFinanceira> ObterTransacoes(int quantidade, bool useNovaTransacao = false)
    {
        return ObterTransacoesFalsas(useNovaTransacao).Generate(quantidade);
    }

    private static Faker<TransacaoFinanceira> ObterTransacoesFalsas(bool useNovaTransacao)
    {
        var seed = 0;
        
        if (useNovaTransacao)
            seed = Random.Shared.Next(10, int.MaxValue);
        
        return new Faker<TransacaoFinanceira>()
            .RuleFor(t => t.Id, Guid.NewGuid)
            .RuleFor(t => t.Descricao, faker => faker.Lorem.Text())
            .RuleFor(t => t.Data, faker => faker.Date.Past())
            .RuleFor(t => t.Valor, faker => faker.Finance.Amount(0.01m))
            .RuleFor(t => t.Tipo, faker => faker.Random.Enum<TipoTransacao>())
            .RuleFor(t => t.IdUsuario, ObterUsuarios(1,true).First().Id) // TODO: NECESSÁRIO AJUSTAR
            .UseSeed(seed);
    }
}