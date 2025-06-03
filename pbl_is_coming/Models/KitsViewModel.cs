using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbl_is_coming.Models
{
    public class KitsViewModel : PadraoViewModel
    {
        public string Nome { get; set; }
        public string Situacao { get; set; }
        public DateTime? DtLastMainte { get; set; }
        public string DescEquip {  get; set; }
        public double PrecoEquip {  get; set; }
        [NotMapped]
        public IFormFile ImagemUpload { get; set; }
        // para armazenar no banco
        public byte[] Imagem { get; set; }
    }
}
