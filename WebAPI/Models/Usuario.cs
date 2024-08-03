namespace WebAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string numCliente { get; set; }
        public string nombre { get; set; }
        public string apellido_p { get; set; }
        public string apellido_m { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string nacimiento { get; set; }
    }
}
