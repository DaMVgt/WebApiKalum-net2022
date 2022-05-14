namespace WebApiKalum
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureService(IServiceCollection _services)
        {
            _services.AddControllers();
            // _services.AddDbContext<>();
        }
    }
}