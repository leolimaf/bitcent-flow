using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Persistence.Contracts;

public interface IGeradorJwt
{
    string GerarAccessToken(Usuario usuario);
}