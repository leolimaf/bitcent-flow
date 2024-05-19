using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Persistence.Contracts;

public interface IJwtGenarator
{
    string GerarToken(Usuario usuario);
}