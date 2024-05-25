using Bogus;
using Bogus.Extensions.Brazil;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Tests.Fixtures;

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
            .RuleFor(u => u.Nome, faker => faker.Person.FirstName)
            .RuleFor(u => u.Sobrenome, faker => faker.Person.LastName)
            .RuleFor(u => u.Email, faker => faker.Person.Email)
            .RuleFor(u => u.Senha, faker => faker.Internet.Password())
            .RuleFor(u => u.SenhaHash, (_, u) => BCrypt.Net.BCrypt.HashPassword(u.Senha))
            .RuleFor(u => u.Celular, faker => faker.Phone.PhoneNumber())
            .RuleFor(u => u.Token, () => null)
            .RuleFor(u => u.ValidadeToken, () => null)
            .UseSeed(seed);
            
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
            .UseSeed(seed);
    }
}