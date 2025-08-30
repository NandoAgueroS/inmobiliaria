namespace inmobiliaria.Repositories
{
    public class RepositorioBase
    {

        protected readonly IConfiguration _configuration;

        protected readonly string connectionString;

        protected RepositorioBase(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration["ConnectionStrings:MySql"];
        }
   } 
}