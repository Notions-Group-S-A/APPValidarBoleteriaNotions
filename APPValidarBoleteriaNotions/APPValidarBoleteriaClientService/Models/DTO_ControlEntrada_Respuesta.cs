using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPValidarBoleteriaClientService.Models
{
    public class DTO_ControlEntrada_Respuesta
    {
        public DTO_ControlEntrada_Codigo codigo { get; set; }
        public string mensaje { get; set; }
        public object datos { get; set; }
    }

    public enum DTO_ControlEntrada_Codigo
    {
        Valido = 200,
        Invalido = 300,
        Quemada = 400,
        Inexistente = 500
    }
}
