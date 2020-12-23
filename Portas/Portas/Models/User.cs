using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portas.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Nome obrigatorio")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        [Display(Name = "E-Mail")]
        //[CAMPO IDENTITY]
        public string email { get; set; }

        public string senhaEncry { get; set; }

        [Required(ErrorMessage = "Senha obrigatório")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Senha { get; set; }

        [NotMapped]
        public string Token { get; set; }

    }
}
