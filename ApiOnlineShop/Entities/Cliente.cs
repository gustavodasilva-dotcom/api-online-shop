namespace ApiOnlineShop.Entities
{
    public class Cliente : Entidade
    {
        public string PrimeiroNome { get; set; }
        public string NomeDoMeio { get; set; }
        public string Sobrenome { get; set; }
        public string Cpf { get; set; }
        public Endereco Endereco { get; set; }
        public InformacoesContato InformacoesContato { get; set; }
    }
}
