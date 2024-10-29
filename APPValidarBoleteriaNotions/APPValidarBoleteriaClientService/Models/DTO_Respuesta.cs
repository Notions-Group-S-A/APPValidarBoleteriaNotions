namespace APPValidarBoleteriaClientService.Models;

public class DTO_Respuesta<T>
{
    public DTO_CodigoResultado codigo { get; set; }
    public string mensaje { get; set; }
    public T datos { get; set; }
}

public class DTO_Respuesta : DTO_Respuesta<object>
{
}

