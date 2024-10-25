using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPValidarBoleteriaClientService.Models
{
    public class DTO_Respuesta
    {
        public DTO_CodigoResultado codigo { get; set; }
        public string mensaje { get; set; }
        public object datos { get; set; }
    }

    public enum DTO_CodigoResultado
    {
        Success = 100,
        Entrada_Disponible = 200,
        Entrada_Utilizada = 300,
        Error = 500
    }
}
