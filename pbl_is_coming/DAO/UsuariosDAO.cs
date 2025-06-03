using pbl_is_coming.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace pbl_is_coming.DAO
{
    class UsuariosDAO : PadraoDAO<UsuariosViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuariosViewModel model)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("id", model.Id);
            parametros[1] = new SqlParameter("nome_usuario", model.Nome);
            parametros[2] = new SqlParameter("login_usuario", model.Login);
            parametros[3] = new SqlParameter("senha_usuario", model.Senha);
            parametros[4] = new SqlParameter("flag_admin", model.FlagAdm ? 1 : 0);
            return parametros;
        }

        protected override UsuariosViewModel MontaModel(DataRow registro)
        {
            UsuariosViewModel model = new UsuariosViewModel();
            model.Id = Convert.ToInt32(registro["id"]);
            model.Nome = registro["nome_usuario"].ToString();
            model.Login = registro["login_usuario"].ToString();
            model.Senha = registro["senha_usuario"].ToString();
            model.FlagAdm = Convert.ToInt32(registro["flag_admin"]) == 1;
            return model;
        }

        protected override void SetTabela()
        {
            Tabela = "Usuarios";
        }

        public UsuariosViewModel ConsultaPorLogin(string login)
        {
            var p = new SqlParameter[]
            {
        new SqlParameter("login_usuario", login)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaPorLogin", p);

            if (tabela.Rows.Count == 0)
                return null;

            return MontaModel(tabela.Rows[0]);
        }
    }
}
