namespace ApiOnlineShop.Models.ViewModels
{
    public class DetalhesPedidoViewModel
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public string Nome { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }
        public string Fornecedor { get; set; }
    }
}
