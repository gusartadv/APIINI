using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using APIINI.Models;
using Newtonsoft.Json;
using RestSharp;



namespace APIINI.Controllers
{
    public class HomeController : Controller
    {
        //ServiceReference1. sv = new ServiceReference1.PagosInerface();

        public async Task<ActionResult> Index()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://localhost:5474/api/values");
            //var json = await httpClient.GetStringAsync("http://conveniosweb.azurewebsites.net/api/values");

            List<string> result = json.Split(new char[] { ',' }).ToList();
            List<Convenio> lista = new List<Convenio>();
            foreach (string item in result) {
                lista.Add(new Convenio { IdConvenio=1, Nombre=item });
            }

            //List<Convenio[]> objParams = result.OfType<Convenio[]>().ToList();
            //var conveniosList = JsonConvert.DeserializeObject<List<Convenio>>(json);
            return View(lista);
            
        }

        [HttpPost]
        public ActionResult Index(string radio) {
            Session["Trama"] = radio;

            return View("MedioPago");
        }


        [HttpPost]
        public ActionResult Factura(string radioMP)
        {
            Session["Trama"] = Session["Trama"] + "|" + radioMP;
            return View("Factura");
        }

        [HttpPost]
        public async Task<ActionResult> Consulta(string numFactura)
        {
            Session["Trama"] = Session["Trama"] + "|" + numFactura;
            string texto = string.Empty;
            texto = Convert.ToString(Session["Trama"]);
            string[] lines = { texto };
            //System.IO.File.WriteAllLines(@"C:\Users\gustavoadolfo\Documents\WriteLines.txt", lines);
            string envio = string.Empty;
            envio = "Pagar" + "|" + numFactura + "|" + "1";
            //var httpClient = new HttpClient();

            //StringContent stringContent1 = new StringContent(envio);

            //var json = await httpClient.GetAsync(new Uri("http://localhost:5474/api/Convenio/5"));
            
            

            var client = new RestClient("http://localhost:5474/api/Convenio/" + numFactura);

            var request = new RestRequest(Method.GET);

            request.AddParameter("id", "5");

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            ViewBag.Resultado = content.ToString().Replace('"', ' ').Trim();
            ViewBag.factura = numFactura;
            return View("Consulta");

        }

        [HttpPost]
        public ActionResult Compensar(string numFactura)
        {
            ViewBag.factura = numFactura;
            return View("Compensar");
        }

        [HttpPost]
        public ActionResult Pago(string numFactura)
        {
            ViewBag.factura = numFactura;
            return View("Pago");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}