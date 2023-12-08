using Bogus;
using MyFinances.Domain.Models;

namespace MyFinances.Tests.Fixtures;

public class DataFixture
{

    public static Usuario GetUser(bool useNewSeed = false)
    {
        return GetUsers(1, useNewSeed).First();
    }
    
    public static List<Usuario> GetUsers(int count, bool useNewSeed = false)
    {
        return GetUsersFaker(useNewSeed).Generate(count);
    }

    private static Faker<Usuario> GetUsersFaker(bool useNewSeed)
    {
        var seed = 0;
        
        if (useNewSeed)
            seed = Random.Shared.Next(10, int.MaxValue);
        
        return new Faker<Usuario>()
            .RuleFor(u => u.Id, () => new Guid())
            .RuleFor(u => u.Nome, faker => faker.Name.FullName())
            .RuleFor(u => u.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.SenhaHash, faker => faker.Internet.Password())
            .RuleFor(u => u.Token, () => null)
            .RuleFor(u => u.ValidadeToken, () => null)
            .RuleFor(u => u.IsAdministrador, () => false)
            .UseSeed(seed);;
            
    }
}