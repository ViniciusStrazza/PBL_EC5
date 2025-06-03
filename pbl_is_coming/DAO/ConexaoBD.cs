using System.Data.SqlClient;

namespace pbl_is_coming.DAO
{
    public static class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            //string strCon = "Data Source=MAQUINARIO-MONS\\SQLSERVER2022; Database=AulaDB; user id=sa; password=123456";
            string strCon = "Data Source=localhost; Database=AulaDB; user id=sa; password=123456";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
