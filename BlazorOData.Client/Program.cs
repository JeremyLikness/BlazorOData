using System.Threading.Tasks;
using BlazorOData.Client.Models;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorOData.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault();
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton<ITodoDataAccess, TodoSimpleOData>();
            builder.Services.AddSingleton<TodoViewModel>();
            await builder.Build().RunAsync();
        }
    }
}
