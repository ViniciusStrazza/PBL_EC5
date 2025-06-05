using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbl_is_coming.DAO;
using pbl_is_coming.Models;
using System.IO;

namespace pbl_is_coming.Controllers
{
    public class UsuariosController : PadraoController<UsuariosViewModel>
    {
        public UsuariosController()
        {
            DAO = new UsuariosDAO();
            GeraProximoId = true;
            ExigeAutenticacao = false; // permite Create sem login
        }

        // Somente admin pode ver listagem de usuários
        public override IActionResult Index()
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");
            return base.Index();
        }

        // Create continua livre para criar conta
        public override IActionResult Create()
        {
            ViewBag.Operacao = "I";
            var model = new UsuariosViewModel();
            PreencheDadosParaView("I", model);
            return View("Form", model);
        }

        // Só admin pode editar
        public override IActionResult Edit(int id)
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");
            return base.Edit(id);
        }

        // Só admin pode excluir
        public override IActionResult Delete(int id)
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Home");
            return base.Delete(id);
        }

        // **Save: define TempData e delega para PadraoController, mas redireciona de volta ao Form em I**
        public override IActionResult Save(UsuariosViewModel model, string operacao)
        {
            if (operacao == "I")
            {
                // Criação de um novo usuário (sem precisar estar logado)
                model.FlagAdm = false;
                DAO.Insert(model);

                // Mensagem de sucesso
                TempData["Sucesso"] = "Usuário criado com sucesso!";

                // ← Em vez de ir para Index (que é protegido), voltamos ao próprio Create(Form)
                return RedirectToAction("Create", "Usuarios");
            }
            else
            {
                // Atualização — só admins chegam aqui
                TempData["Sucesso"] = "Usuário atualizado com sucesso!";
                return base.Save(model, operacao);
            }
        }

        protected override void ValidaDados(UsuariosViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "Preencha com o nome completo.");
            if (string.IsNullOrEmpty(model.Login))
                ModelState.AddModelError("Login", "Preencha o nome de usuário.");
            else if (model.Login.Length < 5)
                ModelState.AddModelError("Login", "Login deve ter pelo menos 5 caracteres.");
            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Senha", "Preencha a senha de usuário.");
            else if (model.Senha.Length < 6)
                ModelState.AddModelError("Senha", "Senha muito curta.");
        }
    }
}
