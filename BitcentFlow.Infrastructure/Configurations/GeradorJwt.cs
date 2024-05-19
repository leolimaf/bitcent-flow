using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;

namespace BitcentFlow.Infrastructure.Configurations;

public class GeradorJwt(AppDbContext context) : IGeradorJwt
{
    public string GerarAccessToken(Usuario usuario)
    {
        throw new NotImplementedException();
    }
}