using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbl_is_coming.DAO;

namespace pbl_is_coming.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FazLogin(string usuario, string senha)
        {
            var usuarioDAO = new UsuariosDAO();
            var usuarioBD = usuarioDAO.ConsultaPorLogin(usuario);

            if (usuarioBD != null && usuarioBD.Senha == senha)
            {
                HttpContext.Session.SetString("Logado", "true");
                HttpContext.Session.SetInt32("IdUsuario", usuarioBD.Id);
                HttpContext.Session.SetInt32("Admin", usuarioBD.FlagAdm ? 1 : 0);
                HttpContext.Session.SetString("NomeUsuario", usuarioBD.Nome);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View("Index");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
