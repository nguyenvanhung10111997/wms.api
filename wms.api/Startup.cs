using Autofac;
using wms.api.Configurations;
using wms.business.Services.Interfaces;
using wms.ids.business.Configs;
using wms.infrastructure;
using wms.infrastructure.Configurations;

namespace wms.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettingRegister.Binding(configuration);
            HostBuilderItem.ConfigurationItem = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            HostBuilderItem.ServiceCollectionItem = services;
        }

        //this method call by the runtime. when use register use AutofacServiceProviderFactory in function startup in program.cs
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutoFacModule>();
            builder.RegisterDBConnection();
            HostBuilderItem.ContainerBuilderItem = builder;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            HostBuilderItem.ApplicationBuilderItem = app;
            app.RegisterAppPermission();
        }
    }
}
