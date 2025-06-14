﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pbl_is_coming.DAO;
using pbl_is_coming.Models;
using System;

namespace pbl_is_coming.Controllers
{
    public class PadraoController<T> : Controller where T : PadraoViewModel
    {
        protected bool ExigeAutenticacao { get; set; } = true;
        protected PadraoDAO<T> DAO { get; set; }
        protected bool GeraProximoId { get; set; }
        protected string NomeViewIndex { get; set; } = "index";
        protected string NomeViewForm { get; set; } = "form";

        public virtual IActionResult Index()
        {
            try
            {
                var lista = DAO.Listagem();
                return View(NomeViewIndex, lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public virtual IActionResult Create()
        {
            try
            {
                ViewBag.Operacao = "I";
                T model = Activator.CreateInstance<T>();
                PreencheDadosParaView("I", model);
                return View(NomeViewForm, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        protected virtual void PreencheDadosParaView(string Operacao, T model)
        {
            if (GeraProximoId && Operacao == "I")
                model.Id = DAO.ProximoId();
        }
        public virtual IActionResult Save(T model, string Operacao)
        {
            try
            {
                if (!HelperControllers.IsAdmin(HttpContext.Session) && !(typeof(T) == typeof(UsuariosViewModel) && Operacao == "I"))
                    return RedirectToAction("Index", "Login"); // Bloqueia se não for admin

                ValidaDados(model, Operacao);

                if (!ModelState.IsValid)
                {
                    ViewBag.Operacao = Operacao;
                    PreencheDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }

                if (typeof(T) == typeof(UsuariosViewModel) && Operacao == "I")
                {
                    ((UsuariosViewModel)(object)model).FlagAdm = false;
                }

                if (typeof(T) == typeof(AlteracoesViewModel))
                {
                    var alteracao = (AlteracoesViewModel)(object)model;
                    alteracao.IdUsuario = HelperControllers.GetIdUsuarioLogado(HttpContext.Session) ?? 0;
                    alteracao.DtAlteracao = DateTime.Now;
                }

                if (Operacao == "I")
                    DAO.Insert(model);
                else
                    DAO.Update(model);

                return RedirectToAction(NomeViewIndex);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected virtual void ValidaDados(T model, string operacao)
        {
            ModelState.Clear();
            if (operacao == "I" && DAO.Consulta(model.Id) != null)
                ModelState.AddModelError("Id", "Código de identificação em uso.");
            if (operacao == "A" && DAO.Consulta(model.Id) == null)
                ModelState.AddModelError("Id", "Não foram encontrados registros com este código de identificação.");
            if (model.Id <= 0)
                ModelState.AddModelError("Id", "Código de identificação inválido.");
        }
        public virtual IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                var model = DAO.Consulta(id);
                if (model == null)
                    return RedirectToAction(NomeViewIndex);
                else
                {
                    PreencheDadosParaView("A", model);
                    return View(NomeViewForm, model);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public virtual IActionResult Delete(int id)
        {
            try
            {
                DAO.Delete(id);
                return RedirectToAction(NomeViewIndex);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Se exige autenticação e o usuário NÃO está logado,
            // mas há mensagem de sucesso pendente em TempData, deixamos continuar.
            if (ExigeAutenticacao
                && !HelperControllers.VerificaUserLogado(HttpContext.Session)
                && TempData["Sucesso"] == null)
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }

            ViewBag.Logado = true;
            base.OnActionExecuting(context);
        }

    }
}
