using Firebase.Auth;
using Firebase.Storage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class FirebaseLogica
{
    private readonly string email = "rocklandmaster@gmail.com";
    private readonly string clave = "SistemasDistribuidos123";
    private readonly string ruta = "bibliotecasistdist.appspot.com";
    private readonly string apiKey = "AIzaSyDA0vE-qAgE8mwwBRh_aJbf8Taxzai1_rM";

    public async Task<string> SubirStorage(Stream archivo, string nombre)
    {
        try
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var storage = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                });

            // Subir el archivo
            await storage.Child("IMAGENES_PRODUCTO").Child(nombre).PutAsync(archivo);

            // Obtener la URL de descarga con el token de acceso
            var downloadUrl = await storage.Child("IMAGENES_PRODUCTO").Child(nombre).GetDownloadUrlAsync();

            return downloadUrl; // Devuelve la URL completa con el token
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al subir a Firebase Storage: {ex.Message}");
            return null; // O devolver un valor por defecto
        }
    }
}