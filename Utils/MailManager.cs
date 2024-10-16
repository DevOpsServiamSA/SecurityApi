using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace SecurityApi.Utils
{
    public class MailManager
    {
        private readonly string clientId = AppConfig.Configuracion.ClientId; // Id de la aplicación registrada
        private readonly string tenantId = AppConfig.Configuracion.TenantId; // Id del inquilino (Tenant)
        private readonly string clientSecret = AppConfig.Configuracion.ClientSecret; // Secreto del cliente
        private readonly string userEmail = AppConfig.Configuracion.UserMail; // Correo del remitente
        private readonly string smtpServer = AppConfig.Configuracion.ServidorMail;
        private readonly int smtpPort = AppConfig.Configuracion.PuertoMail;


        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string mensaje/*, string nombreArchivo = null*/)
        {
            try
            {
                // Obtener token de acceso
                string accessToken = await GetTokenOAuthAsync();

                // Configurar GraphServiceClient
                var graphClient = new GraphServiceClient(
                    new DelegateAuthenticationProvider(requestMessage =>
                    {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        return Task.CompletedTask;
                    })
                );

                // Crear el mensaje de correo
                var message = new Message
                {
                    Subject = asunto,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = mensaje
                    },
                    ToRecipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = destinatario
                            }
                        }
                    }
                };

                // Enviar el correo utilizando Microsoft Graph API
                await graphClient.Users[userEmail]
                    .SendMail(message, null) // null indica que no se guardará una copia en la carpeta 'Sent'
                    .Request()
                    .PostAsync();

                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error enviando correo: {ex.Message}");
                return false;
            }
        }

        private async Task<string> GetTokenOAuthAsync()
        {
            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
    }
}
