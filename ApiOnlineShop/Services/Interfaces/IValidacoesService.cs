namespace ApiOnlineShop.Services.Interfaces
{
    public interface IValidacoesService
    {
        bool ValidarCep(string cep);

        bool ENumerico(string numerico);

        bool ValidarCnpj(string cnpj);
    }
}
