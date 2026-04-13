using PasteleriaWebApp.Data.Infrastructure;
using PasteleriaWebApp.Data.Repositories;

namespace PasteleriaWebApp.Data.Services
{
    public static class InyectorDependencias
    {
        public static void Inyeccion(this IServiceCollection services)
        {
            services.AddScoped<IProducto, ProductoRepository>();
            services.AddScoped<ICategoria, CategoriaRepository>();
        }
    }
}
