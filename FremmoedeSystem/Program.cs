using FremmødeSystem.Components;
using System.Security.Cryptography.X509Certificates;

class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        const string certPath = "/run/secrets/simonspedsbjerg.dk.pfx";
        string password = "";

        Console.WriteLine($"CerthPath: '{certPath}', CertPass: '{password}'");
        builder.WebHost.ConfigureKestrel(options => {
            options.ListenAnyIP(80);
            options.ListenAnyIP(443, listenOptions => {
                listenOptions.UseHttps(new X509Certificate2(certPath));
            });
        });

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


