namespace pbl_is_coming.Models
{
    public class UsuariosViewModel : PadraoViewModel
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool FlagAdm { get; set; } // e NÃO mais int
    }
}
