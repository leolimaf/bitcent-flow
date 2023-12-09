using Bogus;
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
            .RuleFor(u => u.Id, () => new Guid())
            .RuleFor(u => u.Nome, faker => faker.Name.FullName())
            .RuleFor(u => u.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.SenhaHash, faker => faker.Internet.Password())
            .RuleFor(u => u.Token, () => null)
            .RuleFor(u => u.ValidadeToken, () => null)
            .RuleFor(u => u.IsAdministrador, () => false)
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
            .RuleFor(t => t.Id, () => new Guid())
            .RuleFor(t => t.Descricao, faker => faker.Lorem.Text())
            .RuleFor(t => t.Data, faker => faker.Date.Past())
            .RuleFor(t => t.Valor, faker => faker.Finance.Amount(0.01m))
            .RuleFor(t => t.Tipo, faker => faker.Random.Enum<TipoTransacao>())
            .RuleFor(t => t.IdUsuario, ObterUsuarios(1,true).First().Id) // TODO: NECESSÁRIO AJUSTAR
            .UseSeed(seed);
    }
}