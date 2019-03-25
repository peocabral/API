using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PLWebAPI.Models
{
    public class Funcionarios
    {
        public int id { get; set; }
        public string matricula { get; set; }
        public string nome { get; set; }
        public string area { get; set; }
        public string cargo { get; set; }
        public decimal salario_bruto { get; set; }
        public DateTime data_de_admissao { get; set; }
    }
}
