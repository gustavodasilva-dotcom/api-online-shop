namespace ApiOnlineShop.Models.ViewModels
{
    public class ProdutoViewModel
    {    
        public int ProdutoID { get; set; }
        
        public string Nome { get; set; }
        
        public string Medida { get; set; }
        
        public double Preco { get; set; }

        public string Base64 { get; set; }

        public int CategoriaID { get; set; }

        public string Categoria { get; set; }
        
        public string Descricao { get; set; }
        
        public string Fornecedor { get; set; }
        
        public string Cnpj { get; set; }
    }
}
