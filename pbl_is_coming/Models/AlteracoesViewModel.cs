using System;

namespace pbl_is_coming.Models
{
    public class AlteracoesViewModel : PadraoViewModel
    {
        public int IdUsuario { get; set; } // Deve ser atribuído como o id do usuario administrador logado
        public string Responsavel { get; set; }
        public DateTime DtAlteracao { get; set; } // Sempre será na data atual
        public double Setpoint { get; set; }
        public string ConfigMaMf { get; set; }
        public string DescAlteracao { get; set; }
    }
}
