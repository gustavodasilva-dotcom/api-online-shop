namespace ApiOnlineShop.Entities
{
    public class Fornecedor : Entidade
    {
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Contato { get; set; }
        public Endereco Endereco { get; set; }
    }
}