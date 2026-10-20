
namespace CSharpConctactApi.Models
{
    public class ContactoTelefonoModel
    {
        public int ContactoId { get; set; }
        public ContactoModel Contacto { get; set; } = null!;
        public int TelefonoId { get; set; }
        public TelefonoModel Telefono { get; set; } = null!;
        public string Tipo { get; set; } = "celular";
    }
}