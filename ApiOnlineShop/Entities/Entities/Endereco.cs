namespace ApiOnlineShop.Entities.Entities
{
    public class Endereco : Entidade
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }
    }
}