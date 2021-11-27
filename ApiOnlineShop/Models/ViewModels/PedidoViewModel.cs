using System;
using System.Collections.Generic;

namespace ApiOnlineShop.Models.ViewModels
{
    public class PedidoViewModel
    {
        public int PedidoId { get; set; }
        
        public int ClienteId { get; set; }
        
        public string Cliente { get; set; }
        
        public string Cpf { get; set; }
        
        public DateTime DataCompra { get; set; }

        public EnderecoViewModel Endereco { get; set; }

        public List<DetalhesPedidoViewModel> DetalhesPedido { get; set; }
    }
}
