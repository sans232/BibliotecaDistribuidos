using Newtonsoft.Json;
using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdCategoria == 0) ? CategoriaLogica.Instancia.Registrar(objeto) : CategoriaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = CategoriaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarEditorial()
        {
            List<Editorial> oLista = new List<Editorial>();
            oLista = EditorialLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarEditorial(Editorial objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdEditorial == 0) ? EditorialLogica.Instancia.Registrar(objeto) : EditorialLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarEditorial(int id)
        {
            bool respuesta = false;
            respuesta = EditorialLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarAutor()
        {
            List<Autor> oLista = new List<Autor>();
            oLista = AutorLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarAutor(Autor objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdAutor == 0) ? AutorLogica.Instancia.Registrar(objeto) : AutorLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAutor(int id)
        {
            bool respuesta = false;
            respuesta = AutorLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarLibro()
        {
            var oLista = LibroLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarLibro(string objeto, HttpPostedFileBase imagenArchivo)
        {
            var response = new Response() { resultado = false, mensaje = "" };

            try
            {
                var libro = JsonConvert.DeserializeObject<Libro>(objeto);

            
                if (libro.IdLibro == 0)
                {
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

                    int id = LibroLogica.Instancia.Registrar(libro);
                    libro.IdLibro = id;

                    response.resultado = id > 0;
                    if (!response.resultado)
                    {
                        response.mensaje = "Error al registrar el libro.";
                    }
                }
                else // Modificar un libro existente
                {
                    // Subir una nueva imagen si se ha proporcionado
                    if (imagenArchivo != null)
                    {
                        var firebaseLogica = new FirebaseLogica();
                        var extension = Path.GetExtension(imagenArchivo.FileName);
                        var nombreArchivo = libro.IdLibro + extension;

                        var rutaImagen = await firebaseLogica.SubirStorage(imagenArchivo.InputStream, nombreArchivo);

                        if (string.IsNullOrEmpty(rutaImagen))
                        {
                            response.mensaje = "Error al subir la imagen a Firebase Storage.";
                            return Json(response, JsonRequestBehavior.AllowGet);
                        }

                        libro.RutaPortada = rutaImagen; // Asignar la nueva URL con el token
                        libro.NombrePortada = nombreArchivo;
                    }

                    response.resultado = LibroLogica.Instancia.Modificar(libro);
                    if (!response.resultado)
                    {
                        response.mensaje = "Error al modificar el libro.";
                    }
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
        public JsonResult EliminarLibro(int id)
        {
            bool respuesta = false;
            respuesta = LibroLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarTipoPersona()
        {
            List<TipoPersona> oLista = new List<TipoPersona>();
            oLista = TipoPersonaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarPersona()
        {
            List<Persona> oLista = new List<Persona>();

            oLista = PersonaLogica.Instancia.Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarPersona(Persona objeto)
        {
            bool respuesta = false;
            objeto.Clave = objeto.Clave == null ? "" : objeto.Clave;
            respuesta = (objeto.IdPersona == 0) ? PersonaLogica.Instancia.Registrar(objeto) : PersonaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarPersona(int id)
        {
            bool respuesta = false;
            respuesta = PersonaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


    }
    public class Response
    {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}