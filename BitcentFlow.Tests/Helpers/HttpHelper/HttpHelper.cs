using System.Text;
using Newtonsoft.Json;

namespace BitcentFlow.Tests.Helpers.HttpHelper;

public class HttpHelper
{
    internal static class UrlsUsuario
    {
        public readonly static string Registrar = "/v1/usuarios/registrar";
        public readonly static string Logar = "/v1/usuarios/logar";
        public readonly static string AtualizarToken = "/v1/usuarios/atualizar-token";
        public readonly static string Deslogar = "/v1/usuarios/deslogar";
    }
    
    internal static class UrlsTransacaoFinanceira
    {
        public readonly static string Adicionar = "/v1/transacoes-financeiras/adicionar";
        public readonly static string ObterPorId = "/v1/transacoes-financeiras/obter-por-id";
        public readonly static string Listar = "/v1/transacoes-financeiras/listar";
        public readonly static string Atualizar = "/v1/transacoes-financeiras/atualizar";
        public readonly static string AtualizarParcialmente = "/v1/transacoes-financeiras/atualizar-parcialmente";
        public readonly static string Remover = "/v1/transacoes-financeiras/remover";
    }
}