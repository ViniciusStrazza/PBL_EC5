using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using pbl_is_coming.DAO;
using pbl_is_coming.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace pbl_is_coming.Controllers
{
    public class HomeController : Controller
    {
        // ----------------------------------------------------------------
        // Parâmetros FIWARE (endereços/IPs/Pastas)
        // ----------------------------------------------------------------
        //private readonly string fiwareHost = "http://52.206.81.21"; // Nuvem Mauriz - morreu
        private readonly string fiwareHost = "http://52.3.200.72"; // Nuvem Arthur
        private readonly string orionPort = "1026";
        private readonly string sthPort = "8666";
        private readonly string entityId = "urn:ngsi-ld:Temp:002";
        private readonly string attrName = "temperature";

        // ----------------------------------------------------------------
        // GET: /Home/Index
        // ----------------------------------------------------------------
        public IActionResult Index()
        {
            return View();
        }

        // ----------------------------------------------------------------
        // Tratamento dos erros
        // ----------------------------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ----------------------------------------------------------------
        // GET: /Home/Sobre
        // ----------------------------------------------------------------
        public IActionResult Sobre()
        {
            return View();
        }

        // ----------------------------------------------------------------
        // GET: /Home/GetCurrentTemperatureFromFiware
        // Retorna { value, timestamp } para o termômetro em tempo real.
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetCurrentTemperatureFromFiware()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            // A URL NGSI-v2 do Orion para obter apenas o atributo "temperature":
            var url = $"{fiwareHost}:{orionPort}/v2/entities/{entityId}/attrs/{attrName}";
            var res = await client.GetAsync(url);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, "Erro ao chamar Orion");

            var texto = await res.Content.ReadAsStringAsync();
            var obj = JObject.Parse(texto);

            double valor = obj["value"].Value<double>();
            string timestampUtc = obj["metadata"]?["TimeInstant"]?["value"]?.Value<string>()
                               ?? DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);

            return Json(new
            {
                value = valor,
                timestamp = timestampUtc
            });
        }

        // ----------------------------------------------------------------
        // GET: /Home/GetTemperatureHistoryFromLastAlteracao
        //
        // 1) Busca a última Alteração no banco (AlteracoesDAO), obtém DtAlteracao.
        // 2) Adiciona +1 minuto a essa data-hora (UTC) para “pular” o instante exato.
        // 3) Monta a URL do STH-Comet com lastN=100 & dateFrom={DtAlteracao+10sec},
        //     pede ao STH as leituras posteriores a esse instate.
        // 4) Retorna o JSON puro para o front-end (Chart.js).
        // ----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetTemperatureHistoryFromLastAlteracao()
        {
            // 1) Buscar última alteração no DAO:
            var daoAlt = new AlteracoesDAO();
            var listaAlt = daoAlt.Listagem();
            var ultimaAlt = listaAlt
                    .OrderByDescending(a => a.DtAlteracao)
                    .FirstOrDefault();

            // 2) Se houver alteração, usamos exatamente esse instante:
            DateTime utcFrom;
            if (ultimaAlt != null)
            {
                // Passamos o DtAlteracao para UTC e somamos 1 MINUTO
                utcFrom = ultimaAlt.DtAlteracao.ToUniversalTime().AddSeconds(10);
            }
            else
            {
                // Se não houver alteração, “dateFrom = agora” → STH devolve vazio
                utcFrom = DateTime.UtcNow.AddMinutes(1);
            }

            // 3) Formatar para “yyyy-MM-ddTHH:mm:ss.fff” (sem Z, mas com frações)
            string strFrom = utcFrom.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            string dateFromEncoded = Uri.EscapeDataString(strFrom);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            // 4) Montar URL STH-Comet com lastN=100 e dateFrom:
            //     ex: http://<IP>:8666/STH/v1/contextEntities/type/Temp/id/urn:ngsi-ld:Temp:002/attributes/temperature?lastN=100&dateFrom=2023-06-01T12:00:00.000
            var url = $"{fiwareHost}:{sthPort}"
                    + $"/STH/v1/contextEntities/type/Temp/id/{entityId}"
                    + $"/attributes/{attrName}"
                    + $"?lastN=100"
                    + $"&dateFrom={dateFromEncoded}";

            var res = await client.GetAsync(url);
            if (!res.IsSuccessStatusCode)
                return StatusCode((int)res.StatusCode, $"Erro ao chamar STH-Comet (HTTP {res.StatusCode})");

            var texto = await res.Content.ReadAsStringAsync();
            return Content(texto, "application/json");
        }
    }
}
