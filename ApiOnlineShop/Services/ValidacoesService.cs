using System;
using System.Text.RegularExpressions;
using System.Net.Mail;
using ApiOnlineShop.Services.Interfaces;
using ApiOnlineShop.Repositories.Interfaces;

namespace ApiOnlineShop.Services
{
    public class ValidacoesService : IValidacoesService
    {
        private readonly IValidacoesRepository _validacoesRepository;

        public ValidacoesService(IValidacoesRepository validacoesRepository)
        {
            _validacoesRepository = validacoesRepository;
        }

        public bool ValidarCep(string cep)
        {
            try
            {
                return _validacoesRepository.ValidarCep(cep);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarUf(string uf)
        {
            try
            {
                if (!uf.ToUpper().Length.Equals(2))
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ENumerico(string numerico)
        {
            try
            {
                return (!Regex.IsMatch(numerico, @"^\d+$"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarCnpj(string cnpj)
        {
            try
            {
                if (!Regex.IsMatch(cnpj, @"^\d+$"))
                    return false;

                if (!cnpj.Length.Equals(14))
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarCpf(string cpf)
        {
            try
            {
                if (!Regex.IsMatch(cpf, @"^\d+$"))
                    return false;

                if (!cpf.Length.Equals(11))
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarEmail(string email)
        {
            try
            {
                var valida = new MailAddress(email);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
