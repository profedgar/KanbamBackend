using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Cards:Entity
    {

        //        4- Um card terá o seguinte formato:

        //id: int | (guid[c#] | uuid [node])
        //titulo : string,
        //conteudo: string,
        //lista: string

        //6-Para inserir um card o título, o conteúdo e o nome da lista devem estar preenchidos, 
        //    o id não deve conter valor.
        [Required]
        public string titulo { get; set; }

        [Required]
        public string conteudo { get; set; }

        [Required]
        public string lista { get; set; }
    }
}
