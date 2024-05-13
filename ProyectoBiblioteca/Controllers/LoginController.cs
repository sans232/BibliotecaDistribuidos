using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class LoginController : Controller
    {
        //hasheartr contraseñas
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            // Hash contraseña ingresada por el usuario
            string hashedPassword = HashPassword(clave);

            // Buscar al usuario en la base de datos usando el correo y la contraseña hasheada
            Persona usuario = PersonaLogica.Instancia.Listar()
                .Where(u => u.Correo == correo && u.Clave == hashedPassword && u.oTipoPersona.IdTipoPersona != 3)
                .FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
            }

            Session["Usuario"] = usuario;

            return RedirectToAction("Index", "Admin");
        }
    }
}
