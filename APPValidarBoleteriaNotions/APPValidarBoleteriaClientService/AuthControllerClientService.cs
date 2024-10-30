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
        string url = $"{urlBase}{resource}";

        try
        {
            /*
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            */
            //using (HttpClient client = new HttpClient(handler))
            using var client = new HttpClient();
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                dto = await response.Content.ReadFromJsonAsync<DTO_Respuesta<DTO_Endpoint>>();                     
            }
        }
        catch (Exception ex) 
        {
            dto.codigo = DTO_CodigoResultado.Error;
            dto.mensaje = ex.Message+ "|"+ex?.StackTrace?.ToString();
        }
        return dto;
    }
}
