namespace APPValidarBoleteriaClientService.Models;

public class DTO_RespuestaEntrada<T>
{
    public DTO_CodigoEntrada codigo { get; set; }
    public string mensaje { get; set; }
    public T datos { get; set; }
}

public class DTO_RespuestaEntrada : DTO_RespuestaEntrada<object>
{
}

