using pbl_is_coming.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbl_is_coming.DAO
{
    class AlteracoesDAO : PadraoDAO<AlteracoesViewModel>
    {
        protected override SqlParameter[] CriaParametros(AlteracoesViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("id_usuario", model.IdUsuario);
            parametros[2] = new SqlParameter("data_alteracao", model.DtAlteracao);
            parametros[3] = new SqlParameter("setpoint", model.Setpoint);
            parametros[4] = new SqlParameter("config_ma_mf", model.ConfigMaMf);
            parametros[5] = new SqlParameter("descricao_alteracao", model.DescAlteracao);
            return parametros;
        }

        protected override AlteracoesViewModel MontaModel(DataRow registro)
        {
            AlteracoesViewModel model = new AlteracoesViewModel();
            model.Id = Convert.ToInt32(registro["id"]);
            model.IdUsuario = Convert.ToInt32(registro["id_usuario"]);
            model.DtAlteracao = Convert.ToDateTime(registro["data_alteracao"]);
            model.Setpoint = Convert.ToDouble(registro["setpoint"]);
            model.ConfigMaMf = registro["config_ma_mf"].ToString();
            model.DescAlteracao = registro["descricao_alteracao"].ToString();
            if (registro.Table.Columns.Contains("Responsável"))
                model.Responsavel = registro["Responsável"].ToString();
            return model;
        }

        protected override void SetTabela()
        {
            Tabela = "Alteracoes";
            NomeSpListagem = "spListagemAlteracoes";
        }
    }
}
