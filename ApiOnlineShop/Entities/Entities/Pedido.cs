using System;
using System.Collections.Generic;

namespace ApiOnlineShop.Entities.Entities
{
    public class Pedido : Entidade
    {
        public DateTime DataCompra { get; set; }

        public Cliente Cliente { get; set; }

        public List<DetalhesPedido> DetalhesPedido { get; set; }
    }
}
