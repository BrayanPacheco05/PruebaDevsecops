using System;

namespace CSharpConctactApi.Models
{
    public class ContactoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public  string Email { get; set; }= string.Empty;
        public string Tel { get; set; }= string.Empty;
        public bool Activo {get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public List<ContactoTelefonoModel> ContactosTelefonos { get; set; }
            = new();
    }
}