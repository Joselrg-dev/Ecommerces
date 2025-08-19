using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ecommerce.CpCommons
{
    public class SeguridadHelpers
    {
        /// <summary>
        /// Genera una clave aleatoria de 8 caracteres.
        /// Basada en GUID → asegura unicidad, pero no complejidad fuerte.
        /// </summary>
        /// <returns>Cadena de texto aleatoria con un valor igual a 8 caracteres</returns>
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);
            return clave;
        }


        /// <summary>
        /// Calcula el hash SHA-256 de un texto.
        /// Usado para almacenar contraseñas de manera irreversible.
        /// </summary>
        /// <remarks>Nota: No incluye SALT → podría ser vulnerable a ataques rainbow table.</remarks>
        /// <param name="texto"></param>
        /// <returns>Cadena cuyo valor es el mismo que el de la instancia</returns>
        public static string GetSHA256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Envía un correo electrónico usando Gmail SMTP.
        /// ¡Atención! Las credenciales están en código → deben moverse a configuración segura.
        /// </summary>
        /// <param name="correo">Destinatario: indica quien envia el email.</param>
        /// <param name="asunto">Argumento: indica el contenido del correo.</param>
        /// <param name="mensaje">Texto: Cuerpo del correo electronico a tratar.</param>
        /// <returns>true si la petición de enviar correo es exitosa; false si dicha petición falla</returns>
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool result;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("jr703596@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("jr7035960@gmail.com", "ddsw rkue hiih neiq"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                _ = ex.Message; // Se ignora la excepción (mala práctica → loguear en el futuro).
            }

            return result;
        }

        // Convierte un archivo en su representación Base64.
        // Devuelve un string y un flag booleano indicando éxito/fracaso.
        public static string ConversionBase64(string ruta, out bool conversion)
        {
            string textobase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textobase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false; // Falla si el archivo no existe o no hay permisos.
            }
            return textobase64;
        }
    }
}
