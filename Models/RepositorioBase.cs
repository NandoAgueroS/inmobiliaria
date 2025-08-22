namespace inmobiliaria.Models
{
    public class RepositorioBase
    {

        protected readonly IConfiguration configuration;

        protected readonly string connectionString;

        public RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:MySql"];
        }
   } 
}