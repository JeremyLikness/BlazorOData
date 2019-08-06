using BlazorOData.Client.Models;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorOData.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ITodoDataAccess, MockTodoData>();
            services.AddSingleton<ITodoDataAccess, TodoSimpleOData>();
            services.AddSingleton<TodoViewModel>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
