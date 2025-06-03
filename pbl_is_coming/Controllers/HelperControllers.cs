using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace pbl_is_coming.Controllers
{
    public static class HelperControllers
    {
        public static Boolean VerificaUserLogado(ISession session)
        {
            string logado = session.GetString("Logado");
            if (logado == null)
                return false;
            else
                return true;
        }

        public static int? GetIdUsuarioLogado(ISession session)
        {
            return session.GetInt32("IdUsuario");
        }

        public static bool IsAdmin(ISession session)
        {
            return session.GetInt32("Admin") == 1;
        }

    }
}
