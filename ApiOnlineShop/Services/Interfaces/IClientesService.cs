﻿using ApiOnlineShop.Models.InputModels;
using ApiOnlineShop.Models.ViewModels;
using System.Threading.Tasks;

namespace ApiOnlineShop.Services.Interfaces
{
    public interface IClientesService
    {
        Task<ClienteViewModel> Obter(string cpf);
        Task<ClienteViewModel> Inserir(ClienteInputModel clienteInsert);
        Task<ClienteViewModel> Atualizar(string cpf, ClienteInputModel clienteUpdate);
        Task Deletar(string cpf);
    }
}