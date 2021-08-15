namespace ApiOnlineShop.Models.ViewModels
{
    public class ProdutoViewModel
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Medida { get; set; }
        public float Preco { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string Fornecedor { get; set; }
        public string Cnpj { get; set; }
    }
}
