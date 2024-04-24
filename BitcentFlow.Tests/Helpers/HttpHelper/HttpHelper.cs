using System.Text;
using Newtonsoft.Json;

namespace BitcentFlow.Tests.Helpers.HttpHelper;

public class HttpHelper
{
    internal static class UrlsUsuario
    {
        public readonly static string Cadastrar = "/v1/autenticacao/cadastrar";
        public readonly static string Logar = "/v1/autenticacao/logar";
        public readonly static string AtualizarToken = "/v1/autenticacao/atualizar-token";
        public readonly static string Deslogar = "/v1/autenticacao/deslogar";
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