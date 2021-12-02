using ApiOnlineShop.Entities.Entities;

namespace ApiOnlineShop.Repositories.Interfaces
{
    public interface IValidacoesRepository
    {
        bool ValidarCep(string cep);
    }
}
