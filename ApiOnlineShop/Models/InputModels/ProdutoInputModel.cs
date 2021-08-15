namespace ApiOnlineShop.Models.InputModels
{
    public class ProdutoInputModel
    {
        public string Nome { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }
        public int CategoriaId { get; set; }
        public int FornecedorId { get; set; }
    }
}
