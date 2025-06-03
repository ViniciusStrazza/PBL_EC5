using pbl_is_coming.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace pbl_is_coming.DAO
{
    class KitsDAO : PadraoDAO<KitsViewModel>
    {
        protected override SqlParameter[] CriaParametros(KitsViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("nome", model.Nome);
            parametros[2] = new SqlParameter("situacao", model.Situacao);
            parametros[3] = new SqlParameter("data_ultima_manutencao", model.DtLastMainte);
            parametros[4] = new SqlParameter("descricao_equipamento", model.DescEquip);
            parametros[5] = new SqlParameter("preco_equipamento", model.PrecoEquip);
            parametros[6] = new SqlParameter("imagem", (object)model.Imagem ?? DBNull.Value);
            return parametros;
        }

        protected override KitsViewModel MontaModel(DataRow registro)
        {
            KitsViewModel model = new KitsViewModel();
            model.Id = Convert.ToInt32(registro["id"]);
            model.Nome = registro["nome"].ToString();
            model.Situacao = registro["situacao"].ToString();
            model.DtLastMainte = registro["data_ultima_manutencao"] == DBNull.Value
                ? (DateTime?)null
                : Convert.ToDateTime(registro["data_ultima_manutencao"]);
            model.DescEquip = registro["descricao_equipamento"].ToString();
            model.PrecoEquip = Convert.ToDouble(registro["preco_equipamento"]);
            model.Imagem = registro["imagem"] == DBNull.Value
                              ? null
                              : (byte[])registro["imagem"];
            return model;
        }

        protected override void SetTabela()
        {
            Tabela = "Kits";
        }
    }
}
