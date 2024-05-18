using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Commons.Ordering;
using POS.Application.Extensions.WatchDog;
using POS.Application.Interfaces;
using POS.Application.Services;
using System.Reflection;

namespace POS.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton(configuration);

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IOrderingQuery, OrderingQuery>();
            services.AddTransient<IFileStorageLocalApplication, FileStorageLocalApplication>();

            services.AddScoped<IGenerateExcelApplication, GenerateApplicationExcel>();
            services.AddScoped<ICategoryApplication, CategoryApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IProviderApplication, ProviderApplication>();
            services.AddScoped<IDocumentTypeApplication, DocumentTypeApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IWarehouseApplication, WarehouseApplication>();
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IProductStockApplication, ProductStockApplication>();
            services.AddScoped<IPurchaseApplication, PurchaseApplication>();
            services.AddScoped<IClientApplication, ClientApplication>();
            services.AddScoped<ISaleApplication, SaleApplication>();

            services.AddWatchDog(configuration);
            

            return services;

        }
    }
}
