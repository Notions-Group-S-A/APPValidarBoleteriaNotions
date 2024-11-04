namespace APPValidarBoleteriaClientService.Models
{
    public enum DTO_CodigoEntrada
    {
        Valido = 200,         //vigente, puede pasar. 
        Invalido = 300,       //vencida
        Quemada = 400,        //usada  
        Inexistente = 500,    //no encontrada 
        //
        NO_SUCESS=600,
    }
}
