using System.Collections.Generic;

namespace ApiOnlineShop.Models.ViewModels
{
    public class ErroViewModel
    {
        public int StatusCode { get; set; }

        public List<string> MensagensDeErro { get; set; }
    }
}
