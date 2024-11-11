namespace APPValidarBoleteriaClientService.Models
{
    public enum DTO_CodigoEntrada
    {
        Valido = 200,         //vigente, puede pasar. 
        Invalido = 300,       //vencida
        Quemada = 400,        //usada  
        Inexistente = 500,    //no encontrada 
        //
        RESPUESTA_NO_COMPLETA=1600,
        ERROR_RESPUESTA = 1700,
        FALLO_RED = 1800,
    }
}
