using APPValidarBoleteriaClientService.Models;
using System.Net.Http.Json;

namespace APPValidarBoleteriaClientService;

//Boleteria.Gateway.API
public class AuthControllerClientService
{
    //
    //string URL_Base = "apigateway.boleteriadigital.com.ar";
    string urlBase = "http://desa-apigateway.boleteriadigital.com.ar";

    async public Task<DTO_Respuesta<DTO_Endpoint>> GetEnpoint(string ente)
    {
        var dto = new DTO_Respuesta<DTO_Endpoint>();
        string resource = $"/api/AuthController/Get_Endpoint?Ente={ente}";
        //string resource = $"/api/AppValidarEntradas/Get_Endpoint?Ente={ente}";
        string url = $"{urlBase}{resource}";

        try
        {
            using var client = new HttpClient();
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                dto = await response.Content.ReadFromJsonAsync<DTO_Respuesta<DTO_Endpoint>>();
            }
            else
            {
                dto.codigo = DTO_CodigoResultado.Error;
                dto.mensaje = $"{response.StatusCode}:{response.Content}";
            }
        }
        catch (Exception ex) 
        {
            dto = new DTO_Respuesta<DTO_Endpoint>();
            dto.codigo = DTO_CodigoResultado.NoRespuesta;
            dto.mensaje = ex.Message+ "|"+ex?.StackTrace?.ToString();
        }
        return dto;
    }
}
