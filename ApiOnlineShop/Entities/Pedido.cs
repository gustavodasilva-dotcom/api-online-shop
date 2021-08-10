using System;

namespace ApiOnlineShop.Entities
{
    public class Pedido : Entidade
    {
        public DateTime DataCompra { get; set; }
        public Cliente Cliente { get; set; }
    }
}
