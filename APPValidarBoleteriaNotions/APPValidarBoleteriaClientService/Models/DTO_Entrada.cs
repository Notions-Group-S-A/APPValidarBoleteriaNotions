using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPValidarBoleteriaClientService.Models;

public class DTO_Entrada
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Evento { get; set; }
    public string Funcion { get; set; }
    public string Funcion_Fecha { get; set; }
    public string Sector { get; set; }
    public string Ubicacion { get; set; }
    public string Nombre_Entrada { get; set; }
    public string Texto_Entrada { get; set; }
    public string Ingreso_Fecha { get; set; }
    public int Ingreso_Cantidad { get; set; }
    public bool Quemada { get; set; }//sino esta quemada tengo que mandar esa id.
    public int Id_Relacion_Entradas_ItemCarrito { get; set; }
}
