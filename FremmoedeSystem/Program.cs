using FremmødeSystem.Components;
using FremmødeSystem.Structs;
using System.Security.Cryptography.X509Certificates;

class Program {
    
    private static void Main(string[] args) {
        StartupConfiguration config = new StartupConfiguration();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        
        string password = "";

        Console.WriteLine($"CerthPath: '{config.CertificationPath}', CertPass: '{password}'");
        builder.WebHost.ConfigureKestrel(options => {
            options.ListenAnyIP(80);
            options.ListenAnyIP(443, listenOptions => {
                listenOptions.UseHttps(new X509Certificate2(config.CertificationPath));
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


