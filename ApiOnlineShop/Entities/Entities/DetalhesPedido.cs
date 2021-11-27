namespace ApiOnlineShop.Entities.Entities
{
    public class DetalhesPedido : Entidade
    {
        public int Quantidade { get; set; }
        public Produto Produto { get; set; }
        public Pedido Pedido { get; set; }
    }
}
