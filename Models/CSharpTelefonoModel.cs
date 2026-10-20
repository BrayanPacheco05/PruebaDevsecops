namespace CSharpConctactApi.Models
{
    public class TelefonoModel
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public List<ContactoTelefonoModel> ContactosTelefonos { get; set; }
            = new();
    }
}