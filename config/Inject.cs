using luchito_net.Config.DataProvider;
using luchito_net.Config.DataProvider.Interfaces;
using luchito_net.Repository;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using luchito_net.Service.Interfaces;




namespace luchito_net.Config
{
    public static class Inject
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IDapperWrapper, DapperWrapper>();
        }
    }
}
