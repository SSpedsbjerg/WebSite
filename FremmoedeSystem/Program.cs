using FremmødeSystem.Components;

class Program {
    private static void Main(string[] args) {
        Console.WriteLine(args);
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        string? certPath = "";
        string password = "";
        if(!args.Contains("--no-ssl")) {
            certPath = builder.Configuration["Kestrel:Endpoints:Https:Certificate:Path"];
            var certPasswordFile = Environment.GetEnvironmentVariable("CERT_PASSWORD_FILE") ?? "/run/secrets/cert_password";
            password = File.ReadAllText(certPasswordFile).Trim();
        }

        if(!args.Contains("--no-ssl")) {
            builder.WebHost.ConfigureKestrel(options => {
                options.ConfigureHttpsDefaults(https => {
                    https.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, password);
                });
            });
        }


        var app = builder.Build();

        if(!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
        }

        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        app.UseAntiforgery();

        app.MapStaticAssets();

        app.Run();
    }
}


