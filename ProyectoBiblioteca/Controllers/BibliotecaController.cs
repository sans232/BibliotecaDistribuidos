using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class BibliotecaController : Controller
    {
        // GET: Biblioteca
        private FirebaseLogica cnfirebase = new FirebaseLogica();
        public ActionResult Libros()
        {
            return View();
        }

        public ActionResult Autores()
        {
            return View();
        }

        public ActionResult Editorial()
        {
            return View();
        }

        public ActionResult Categoria()
        {
            return View();
        }

        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://webapibibliloteca2.azurewebsites.net/api/";
        private readonly string _apiBaseUrl2 = "https://webapiusuariosdefinitive.azurewebsites.net/api/";

        public BibliotecaController()
        {
            _httpClient = new HttpClient();
        }

       

        // CATEGORIA METHODS

        [HttpGet]
        public async Task<JsonResult> ListarCategoria()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl + "categoria");
            var data = await response.Content.ReadAsStringAsync();
            var categorias = JsonConvert.DeserializeObject<List<Categoria>>(data);
            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarCategoria(Categoria objeto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(objeto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (objeto.IdCategoria == 0)
                {
                    response = await _httpClient.PostAsync(_apiBaseUrl + "categoria", content);
                }
                else
                {
                    response = await _httpClient.PutAsync(_apiBaseUrl + $"categoria/{objeto.IdCategoria}", content); ;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error en GuardarCategoria: {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al guardar la categoría.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EliminarCategoria(int id)
        {
            try
            {
                // Asegúrate de que la URL esté bien formada
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}categoria/{id}");
                response.EnsureSuccessStatusCode(); // Lanza una excepción si no se recibe un código de éxito

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (HttpRequestException ex)
            {
                // Captura y muestra cualquier problema de solicitud HTTP
                Console.WriteLine($"Error en EliminarEditorial (HttpRequestException): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar editorial (HttpRequestException).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier otro tipo de error
                Console.WriteLine($"Error en EliminarAutor (Exception): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar editorial (Exception).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // EDITORIAL METHODS

        [HttpGet]
        public async Task<JsonResult> ListarEditorial()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl + "editorial");
            var data = await response.Content.ReadAsStringAsync();
            var editorials = JsonConvert.DeserializeObject<List<Editorial>>(data);
            return Json(new { data = editorials }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarEditorial(Editorial objeto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(objeto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (objeto.IdEditorial == 0)
                {
                    response = await _httpClient.PostAsync(_apiBaseUrl + "editorial", content);
                }
                else
                {
                    response = await _httpClient.PutAsync(_apiBaseUrl + $"editorial/{objeto.IdEditorial}", content); ;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error en Guardar Editorial: {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al guardar la editorial.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EliminarEditorial(int id)
        {
            try
            {
                // Asegúrate de que la URL esté bien formada
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}editorial/{id}");
                response.EnsureSuccessStatusCode(); // Lanza una excepción si no se recibe un código de éxito

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (HttpRequestException ex)
            {
                // Captura y muestra cualquier problema de solicitud HTTP
                Console.WriteLine($"Error en EliminarEditorial (HttpRequestException): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar editorial (HttpRequestException).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier otro tipo de error
                Console.WriteLine($"Error en EliminarAutor (Exception): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar editorial (Exception).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // AUTOR METHODS

        [HttpGet]
        public async Task<JsonResult> ListarAutor()
        {
            var response = await _httpClient.GetAsync(_apiBaseUrl + "autor");
            var data = await response.Content.ReadAsStringAsync();
            var autors = JsonConvert.DeserializeObject<List<Autor>>(data);
            return Json(new { data = autors }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarAutor(Autor objeto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(objeto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (objeto.IdAutor == 0)
                {
                    response = await _httpClient.PostAsync(_apiBaseUrl + "autor", content);
                }
                else
                {
                    response = await _httpClient.PutAsync(_apiBaseUrl + $"autor/{objeto.IdAutor}", content); ;
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error en Guardar Editorial: {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al guardar la editorial.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EliminarAutor(int id)
        {
            try
            {
                // Asegúrate de que la URL esté bien formada
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}autor/{id}");
                response.EnsureSuccessStatusCode(); // Lanza una excepción si no se recibe un código de éxito

                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (HttpRequestException ex)
            {
                // Captura y muestra cualquier problema de solicitud HTTP
                Console.WriteLine($"Error en EliminarAutor (HttpRequestException): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar el autor (HttpRequestException).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier otro tipo de error
                Console.WriteLine($"Error en EliminarAutor (Exception): {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar el autor (Exception).", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // LIBRO METHODS

        [HttpGet]
        public async Task<JsonResult> ObtenerLibro(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl + $"libro/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var libro = JsonConvert.DeserializeObject<Libro>(result);

                return Json(new { libro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerLibro: {ex.Message}");
                return Json(new { libro = (Libro)null, mensaje = "Ocurrió un error al obtener el libro.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ListarLibro()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl + "libro");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var listaLibros = JsonConvert.DeserializeObject<List<Libro>>(result);

                return Json(new { data = listaLibros }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarLibro: {ex.Message}");
                return Json(new { data = new List<Libro>(), mensaje = "Ocurrió un error al listar los libros.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<JsonResult> GuardarLibro(string objeto, HttpPostedFileBase imagenArchivo)
        {
            var response = new Response() { resultado = false, mensaje = "" };

            try
            {
                var libro = JsonConvert.DeserializeObject<Libro>(objeto);

                if (imagenArchivo != null)
                {
                    var firebaseLogica = new FirebaseLogica();
                    var extension = Path.GetExtension(imagenArchivo.FileName);
                    var nombreArchivo = libro.Titulo.Replace(" ", "_") + extension;

                    var rutaImagen = await firebaseLogica.SubirStorage(imagenArchivo.InputStream, nombreArchivo);

                    if (string.IsNullOrEmpty(rutaImagen))
                    {
                        response.mensaje = "Error al subir la imagen a Firebase Storage.";
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }

                    libro.RutaPortada = rutaImagen;
                    libro.NombrePortada = nombreArchivo;
                }

                var json = JsonConvert.SerializeObject(libro);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse;
                if (libro.IdLibro == 0)
                {
                    apiResponse = await _httpClient.PostAsync(_apiBaseUrl + "libro", content);
                }
                else
                {
                    apiResponse = await _httpClient.PutAsync(_apiBaseUrl + $"libro/{libro.IdLibro}", content);
                }

                if (apiResponse.IsSuccessStatusCode)
                {
                    var result = await apiResponse.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<bool>(result);
                    response.resultado = resultado;

                    if (!resultado)
                    {
                        response.mensaje = libro.IdLibro == 0 ? "Error al registrar el libro." : "Error al modificar el libro.";
                    }
                }
                else
                {
                    response.mensaje = $"Error en la respuesta del servidor: {apiResponse.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                response.resultado = false;
                response.mensaje = $"Error durante la operación: {ex.Message}";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public async Task<JsonResult> EliminarLibro(int id)
        {
            try
            {
                // Realizar la solicitud DELETE a la API externa
                var response = await _httpClient.DeleteAsync(_apiBaseUrl + $"libro/{id}");
                response.EnsureSuccessStatusCode();

                // Leer la respuesta de la API externa
                var result = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                // Devolver el resultado como JSON
                return Json(new { resultado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error en EliminarLibro: {ex.Message}");
                return Json(new { resultado = false, mensaje = "Ocurrió un error al eliminar el libro.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ListarTipoPersona()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl2 + "tipopersona");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var tiposPersona = JsonConvert.DeserializeObject<List<TipoPersona>>(result);

                return Json(new { data = tiposPersona }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarTipoPersona: {ex.Message}");
                return Json(new { data = new List<TipoPersona>(), mensaje = "Ocurrió un error al listar los tipos de persona.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public async Task<JsonResult> ListarPersona()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl2 + "persona");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var personas = JsonConvert.DeserializeObject<List<Persona>>(result);

                return Json(new { data = personas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ListarPersona: {ex.Message}");
                return Json(new { data = new List<Persona>(), mensaje = "Ocurrió un error al listar las personas.", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GuardarPersona(Persona objeto)
        {
            bool respuesta = false;
            objeto.Clave = objeto.Clave == null ? "" : objeto.Clave;
            respuesta = (objeto.IdPersona == 0) ? PersonaLogica.Instancia.Registrar(objeto) : PersonaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> EliminarPersona(int id)
        {
            var response = new Response() { resultado = false, mensaje = "" };

            try
            {
                var apiResponse = await _httpClient.DeleteAsync($"{_apiBaseUrl2}persona/{id}");
                apiResponse.EnsureSuccessStatusCode();

                var result = await apiResponse.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<bool>(result);

                response.resultado = resultado;
                if (!resultado)
                {
                    response.mensaje = "Error al eliminar la persona.";
                }
            }
            catch (Exception ex)
            {
                response.resultado = false;
                response.mensaje = $"Error durante la operación: {ex.Message}";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }
    public class Response
    {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}