using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Collections.Generic;


namespace ProyectoBiblioteca.Controllers
{
    public class LoginController : Controller
    {
        private HttpClient client;

        public LoginController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://webapiusuariosdefinitive.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string correo, string clave)
        {
            HttpResponseMessage response = await client.GetAsync($"api/persona?correo={correo}&clave={HashHelper.ComputeSha256Hash(clave)}");
            if (response.IsSuccessStatusCode)
            {
                var personasJson = await response.Content.ReadAsStringAsync();
                var personas = JsonConvert.DeserializeObject<List<Persona>>(personasJson);

                var persona = personas.FirstOrDefault(); // Tomamos la primera persona de la lista

                if (persona == null)
                {
                    ViewBag.Error = "Usuario o contraseña no correcta";
                    return View();
                }

                Session["Usuario"] = persona;

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Error = "Error al iniciar sesión. Por favor, inténtelo de nuevo.";
                return View();
            }
        }
    }
}

