namespace SecurityApi
{
    public class AppConfig
    {
        public class Configuracion
        {
            public static string Website { get; set; } = null!;
            public static string ServidorMail { get; set; } = null!;
            public static int PuertoMail { get; set; }
            public static bool EnableSSLMail { get; set; }
            public static string UserMail { get; set; } = null!;
            public static string PasswordMail { get; set; } = null!;
            public static string DestinoEmailJefatura { get; set; } = null!;
            public static string DestinoEmailGerencia { get; set; } = null!;
            public static string DestinoEmailRRHH { get; set; } = null!;
            public static string ClientId { get; set; } = null!;
            public static string TenantId { get; set; } = null!;
            public static string ClientSecret { get; set; } = null!;
        }
    }
}
