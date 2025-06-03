using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using pbl_is_coming.DAO;
using pbl_is_coming.Models;
using System;
using System.IO;
using System.Linq;

namespace pbl_is_coming.Controllers
{
    public class KitsController : PadraoController<KitsViewModel>
    {
        public KitsController()
        {
            DAO = new KitsDAO();
            GeraProximoId = true;
        }

        protected override void ValidaDados(KitsViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Situacao))
                ModelState.AddModelError("Situacao", "Informe a situação do kit.");
            else
            {
                var validas = new[] { "Em Manutenção", "Com Defeito", "Em Funcionamento" };
                if (!validas.Contains(model.Situacao))
                    ModelState.AddModelError("Situacao", "Situação inválida.");
            }

            if (model.PrecoEquip <= 0)
                ModelState.AddModelError("PrecoEquip", "O preço deve ser maior que zero.");

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "O seu equipamento precisa ter um nome.");

            if (!string.IsNullOrEmpty(model.DescEquip) && model.DescEquip.Length > 100)
                ModelState.AddModelError("DescEquip", "A descrição deve ter no máximo 100 caracteres.");

            if (model.DtLastMainte.HasValue && model.DtLastMainte > DateTime.Now)
                ModelState.AddModelError("DtLastMainte", "A data da manutenção não pode ser futura.");

            // imagem obrigatória sempre
            if (model.ImagemUpload == null || model.ImagemUpload.Length == 0)
                ModelState.AddModelError("ImagemUpload", "A imagem do kit é obrigatória.");

            if (model.ImagemUpload != null)
            {
                var ext = Path.GetExtension(model.ImagemUpload.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    ModelState.AddModelError("ImagemUpload", "Só são aceitos arquivos JPG ou PNG.");
            }
        }

        // Save: converte upload em byte[], define TempData e chama base.Save
        public override IActionResult Save(KitsViewModel model, string operacao)
        {
            if (model.ImagemUpload != null && model.ImagemUpload.Length > 0)
            {
                using var ms = new MemoryStream();
                model.ImagemUpload.CopyTo(ms);
                model.Imagem = ms.ToArray();
            }

            if (operacao == "I")
                TempData["Sucesso"] = "Equipamento criado com sucesso!";
            else
                TempData["Sucesso"] = "Equipamento atualizado com sucesso!";

            return base.Save(model, operacao);
        }

        public override IActionResult Create()
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Login");

            ViewBag.Operacao = "I";
            var model = new KitsViewModel();
            PreencheDadosParaView("I", model);
            return View("Form", model);
        }

        public override IActionResult Edit(int id)
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Login");

            ViewBag.Operacao = "A";
            var model = DAO.Consulta(id);
            PreencheDadosParaView("A", model);
            return View("Form", model);
        }

        public override IActionResult Delete(int id)
        {
            if (!HelperControllers.IsAdmin(HttpContext.Session))
                return RedirectToAction("Index", "Login");
            return base.Delete(id);
        }

        public IActionResult Filtrar(string situacao, string descricao)
        {
            var lista = ((KitsDAO)DAO).Listagem();

            if (!string.IsNullOrEmpty(situacao))
                lista = lista.Where(k => k.Situacao == situacao).ToList();

            if (!string.IsNullOrEmpty(descricao))
                lista = lista.Where(k => k.DescEquip != null &&
                                         k.DescEquip.ToLower().Contains(descricao.ToLower()))
                              .ToList();

            return PartialView("_TabelaKits", lista);
        }
    }
}
