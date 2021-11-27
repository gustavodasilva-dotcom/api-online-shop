namespace ApiOnlineShop.Entities.Entities
{
    public class Produto : Entidade
    {        
        public string Nome { get; set; }
        
        public string Medida { get; set; }
        
        public double Preco { get; set; }
        
        public int Quantidade { get; set; }
        
        public string Base64 { get; set; }

        public Categoria Categoria { get; set; }
        
        public Fornecedor Fornecedor { get; set; }
    }
}
