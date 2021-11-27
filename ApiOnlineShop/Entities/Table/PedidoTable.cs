using System;

namespace ApiOnlineShop.Entities.Table
{
    public class PedidoTable
    {
        public int PedidoID { get; set; }

        public int ClienteID { get; set; }

        public string Cliente { get; set; }

        public string Cpf { get; set; }

        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Localidade { get; set; }

        public string Uf { get; set; }

        public string Pais { get; set; }

        public DateTime DataCompra { get; set; }
    }
}
