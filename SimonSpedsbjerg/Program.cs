using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SimonSpedsbjerg.Classes;
using Blazored.Localisation;

namespace SimonSpedsbjerg {
    public class Program {
        public static List<Repo> repos = new List<Repo>();
        public static async Task Main(string[] args) {
            repos.Add(new Repo("AI", "https://github.com/SSpedsbjerg/AI"));
            repos.Add(new Repo("Math", "https://github.com/SSpedsbjerg/MathStuff"));
            repos.Add(new Repo("Holdfast Discordbot", "https://github.com/SSpedsbjerg/8DanskeDiscordBot"));
            repos.Add(new Repo("This Website", null));

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddBlazoredLocalisation();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
            }
        }
    }
