using System;

namespace ApiOnlineShop.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCompra { get; set; }

    }
}
