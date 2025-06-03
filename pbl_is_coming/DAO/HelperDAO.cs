using System;
using System.Data;
using System.Data.SqlClient;

namespace pbl_is_coming.DAO
{
    static class HelperDAO
    {
        public static void ExecutaSQL(string sql, SqlParameter[] p)
        {
            SqlConnection conexao = ConexaoBD.GetConexao();
            try
            {
                SqlCommand comando = new SqlCommand(sql, conexao);
                if (p != null)
                    comando.Parameters.AddRange(p);
                comando.ExecuteNonQuery();
            }
            finally
            {
                conexao.Close();
            }
        }

        public static DataTable ExecutaSelect(string sql, SqlParameter[] p)
        {
            SqlConnection cx = ConexaoBD.GetConexao();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, cx);
                if (p != null)
                    adapter.SelectCommand.Parameters.AddRange(p);
                DataTable tabela = new DataTable();
                adapter.Fill(tabela);
                return tabela;
            }
            finally
            {
                cx.Close();
            }
        }

        public static object NullAsDbNull(object valor)
        {
            if (valor == null)
                return DBNull.Value;
            else
                return valor;
        }
        public static int ExecutaProc(string nomeProc,
                                      SqlParameter[] parametros,
                                      bool consultaUltimoIdentity = false)
        {
            using (SqlConnection conexao = ConexaoBD.GetConexao())
            {
                using (SqlCommand comando = new SqlCommand(nomeProc, conexao))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        comando.Parameters.AddRange(parametros);
                    comando.ExecuteNonQuery();
                    if (consultaUltimoIdentity)
                    {
                        string sql = "select isnull(@@IDENTITY,0)";
                        comando.CommandType = CommandType.Text;
                        comando.CommandText = sql;
                        int pedidoId = Convert.ToInt32(comando.ExecuteScalar());
                        conexao.Close();
                        return pedidoId;
                    }
                    else
                        return 0;
                }
            }
        }

        public static DataTable ExecutaProcSelect(string nomeProc, SqlParameter[] parametros)
        {
            try
            {
                using (SqlConnection conexao = ConexaoBD.GetConexao())
                using (SqlDataAdapter adapter = new SqlDataAdapter(nomeProc, conexao))
                {
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                        adapter.SelectCommand.Parameters.AddRange(parametros);

                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);
                    return tabela;
                }
            }
            catch (SqlException sqlEx)
            {
                // Aqui você pode logar sqlEx.Message, sqlEx.Number, etc.
                throw new ApplicationException($"Erro ao executar procedure '{nomeProc}': {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Qualquer outro erro inesperado
                throw new ApplicationException($"Erro inesperado em ExecutaProcSelect('{nomeProc}'): {ex.Message}", ex);
            }
        }
    }
}
