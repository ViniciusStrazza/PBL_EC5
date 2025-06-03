using System;

namespace pbl_is_coming.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string msgErro)
        {
            Erro = msgErro;
        }
        public ErrorViewModel()
        { }
        public string Erro { get; set; }

        #region Teste 
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        #endregion
    }
}
