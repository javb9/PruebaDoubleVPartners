using Back.Interfaces;
using Back.Repositories;
using System.Runtime.CompilerServices;

namespace Back.assets
{
    public static class StartupTrasient
    {
        public static void configureTrasient(this IServiceCollection services)
        {
            services.AddTransient<IParametric, Parametric>(); //data paraemtrica
            services.AddTransient<IBill, Bill>(); //facturas
        }
    }
}
