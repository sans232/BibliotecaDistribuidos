using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoBiblioteca.Logica
{
    public class LibroLogica
    {

        private static LibroLogica instancia = null;

        public LibroLogica()
        {

        }

        public static LibroLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LibroLogica();
                }

                return instancia;
            }
        }

        public List<Libro> Listar()
        {
            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection conexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT l.IdLibro, l.Titulo, l.RutaPortada, l.NombrePortada,");
                sb.AppendLine("       a.IdAutor, a.Descripcion AS DescripcionAutor,");
                sb.AppendLine("       c.IdCategoria, c.Descripcion AS DescripcionCategoria,");
                sb.AppendLine("       e.IdEditorial, e.Descripcion AS DescripcionEditorial,");
                sb.AppendLine("       l.Ubicacion, l.Ejemplares, l.Estado");
                sb.AppendLine("FROM LIBRO l");
                sb.AppendLine("INNER JOIN AUTOR a ON a.IdAutor = l.IdAutor");
                sb.AppendLine("INNER JOIN CATEGORIA c ON c.IdCategoria = l.IdCategoria");
                sb.AppendLine("INNER JOIN EDITORIAL e ON e.IdEditorial = l.IdEditorial");

                SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    conexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Libro libro = new Libro
                        {
                            IdLibro = Convert.ToInt32(dr["IdLibro"]),
                            Titulo = dr["Titulo"].ToString(),
                            RutaPortada = dr["RutaPortada"].ToString(), // Asumiendo que es la URL de Firebase
                            NombrePortada = dr["NombrePortada"].ToString(),
                            oAutor = new Autor
                            {
                                IdAutor = Convert.ToInt32(dr["IdAutor"]),
                                Descripcion = dr["DescripcionAutor"].ToString()
                            },
                            oCategoria = new Categoria
                            {
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Descripcion = dr["DescripcionCategoria"].ToString()
                            },
                            oEditorial = new Editorial
                            {
                                IdEditorial = Convert.ToInt32(dr["IdEditorial"]),
                                Descripcion = dr["DescripcionEditorial"].ToString()
                            },
                            Ubicacion = dr["Ubicacion"].ToString(),
                            Ejemplares = Convert.ToInt32(dr["Ejemplares"]),
                            Estado = Convert.ToBoolean(dr["Estado"])
                        };

                        listaLibros.Add(libro);
                    }

                    dr.Close();
                }
                catch (Exception ex)
                {
                    listaLibros = null; // o bien un throw si prefieres propagar el error
                }
            }

            return listaLibros;
        }


        public int Registrar(Libro libro)
        {
            int resultado = 0;
            using (var conexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    var cmd = new SqlCommand("sp_registrarLibro", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("Titulo", libro.Titulo);
                    cmd.Parameters.AddWithValue("RutaPortada", libro.RutaPortada);
                    cmd.Parameters.AddWithValue("NombrePortada", libro.NombrePortada);
                    cmd.Parameters.AddWithValue("IdAutor", libro.oAutor.IdAutor);
                    cmd.Parameters.AddWithValue("IdCategoria", libro.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("IdEditorial", libro.oEditorial.IdEditorial);
                    cmd.Parameters.AddWithValue("Ubicacion", libro.Ubicacion);
                    cmd.Parameters.AddWithValue("Ejemplares", libro.Ejemplares);

                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado

                    resultado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex) // Manejo de errores
                {
                    Console.WriteLine($"Error al registrar el libro: {ex.Message}");
                    resultado = 0; // Si falla, resultado es 0
                }
            }

            return resultado;
        }

        public bool Modificar(Libro objeto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    // Crear el comando SQL y establecer los parámetros necesarios
                    var cmd = new SqlCommand("sp_modificarLibro", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("IdLibro", objeto.IdLibro);
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("IdAutor", objeto.oAutor.IdAutor);
                    cmd.Parameters.AddWithValue("IdCategoria", objeto.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("IdEditorial", objeto.oEditorial.IdEditorial);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("Ejemplares", objeto.Ejemplares);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);

                    // Agregar parámetros para la ruta de la portada y el nombre de la portada
                    cmd.Parameters.AddWithValue("RutaPortada", objeto.RutaPortada);  // Nueva URL de Firebase
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.NombrePortada);  // Nuevo nombre del archivo

                    // Parámetro de salida para obtener el resultado
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    // Verificar el resultado
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine($"Error al modificar el libro: {ex.Message}");
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool ActualizarRutaImagen(Libro objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("IdLibro", objeto.IdLibro);
                    cmd.Parameters.AddWithValue("NombrePortada", objeto.NombrePortada);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from LIBRO where IdLibro = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }


    }
}