namespace ApiOnlineShop.Services.Interfaces
{
    public interface IValidacoesService
    {
        bool ValidarCep(string cep);

        bool ValidarUf(string uf);

        bool ENumerico(string numerico);

        bool ValidarCnpj(string cnpj);

        bool ValidarCpf(string cpf);

        bool ValidarEmail(string email);
    }
}
