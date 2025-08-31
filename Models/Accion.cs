namespace inmobiliaria.Models
{
    public sealed record Accion(string value)
    {
        public static readonly Accion Alta = new Accion("Creado");
        public static readonly Accion Baja = new Accion("Eliminado");
        public static readonly Accion Modificacion = new Accion("Modificado");
    }
}