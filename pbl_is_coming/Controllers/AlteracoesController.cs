using Microsoft.AspNetCore.Mvc;
using pbl_is_coming.DAO;
using pbl_is_coming.Models;
using System;
using System.Linq;
using System.Globalization;

namespace pbl_is_coming.Controllers
{
    public class AlteracoesController : PadraoController<AlteracoesViewModel>
    {
        public AlteracoesController()
        {
            DAO = new AlteracoesDAO();
            GeraProximoId = true;
        }

        protected override void ValidaDados(AlteracoesViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (model.Setpoint < 0)
                ModelState.AddModelError("Setpoint", "Defina a temperatura desejada.");
            if (model.Setpoint > 100)
                ModelState.AddModelError("Setpoint", "Temperatura excede o limite.");

            if (string.IsNullOrEmpty(model.ConfigMaMf) ||
                (model.ConfigMaMf != "MA" && model.ConfigMaMf != "MF"))
                ModelState.AddModelError("ConfigMaMf", "Use apenas 'MA' ou 'MF'.");
        }

        protected override void PreencheDadosParaView(string Operacao, AlteracoesViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            if (Operacao == "I")
            {
                model.DtAlteracao = DateTime.Now;
                model.IdUsuario = HelperControllers.GetIdUsuarioLogado(HttpContext.Session) ?? 0;
            }
        }

        // Método de filtragem (já existia)
        public IActionResult Filtrar(string config, string data)
        {
            var lista = ((AlteracoesDAO)DAO).Listagem();

            if (!string.IsNullOrEmpty(config))
                lista = lista.Where(a => a.ConfigMaMf == config).ToList();

            if (DateTime.TryParse(data, out var dataMin))
                lista = lista.Where(a => a.DtAlteracao.Date >= dataMin.Date).ToList();

            return PartialView("_TabelaAlteracoes", lista);
        }

        // GET: /Alteracoes/GetUltimaAlteracao
        // Retorna JSON com { setpoint, dtAlteracao, config }
        [HttpGet]
        public IActionResult GetUltimaAlteracao()
        {
            var lista = ((AlteracoesDAO)DAO).Listagem();
            var ultima = lista.OrderByDescending(a => a.DtAlteracao).FirstOrDefault();
            if (ultima == null)
                return NotFound(); // 404 se não houver registro

            // Converte DtAlteracao para UTC ISO-8601:
            string dtUtcIso = ultima.DtAlteracao
                .ToUniversalTime()
                .ToString("o", CultureInfo.InvariantCulture);

            return Json(new
            {
                setpoint = ultima.Setpoint,
                dtAlteracao = dtUtcIso,
                config = ultima.ConfigMaMf
            });
        }

        // ───────────────────────────────────────────────────────────────────────────
        // Override de Save(...) para manter o TempData["Sucesso"] e exibir o modal  
        // Assim que a alteração for criada ou editada, o TempData dispara o modal.
        // ───────────────────────────────────────────────────────────────────────────
        public override IActionResult Save(AlteracoesViewModel model, string operacao)
        {
            if (operacao == "I")
                TempData["Sucesso"] = "Alteração criada com sucesso!";
            else
                TempData["Sucesso"] = "Alteração atualizada com sucesso!";

            return base.Save(model, operacao);
        }
    }
}
