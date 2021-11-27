using System;
using System.Collections.Generic;

namespace ApiOnlineShop.Models.InputModels
{
    public class PedidoInputModel
    {
        public DateTime DataCompra { get; set; }

        public int ClienteId { get; set; }

        public List<DetalhesPedidoInputModel> DetalhesPedido { get; set; }
    }
}
