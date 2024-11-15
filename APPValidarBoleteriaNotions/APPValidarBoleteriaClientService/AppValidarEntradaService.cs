
using APPValidarBoleteriaClientService.Models;
using System.Net.Http.Json;

namespace APPValidarBoleteriaClientService;

//Boleteria.Gateway.API
public class AppValidarEntradaService
{
    //
    //desarrollo
    string urlBase = "http://desa-apigateway.boleteriadigital.com.ar";
    //producción
    //string urlBase = "https://validarentrada.boleteriadigital.com.ar";

    async public Task<DTO_Respuesta<string>> GetEnpoint(string ente)
    {
        var dto = new DTO_Respuesta<string>();

        //login unico para boleterias
        //string resource = $"/api/AuthController/Get_Endpoint?Ente={ente}";

        string resource = $"/api/AppValidarEntrada/Get_Endpoint?Ente={ente}";
        string url = $"{urlBase}{resource}";

        try
        {
            using var client = new HttpClient();
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                dto = await response.Content.ReadFromJsonAsync<DTO_Respuesta<string>>();
            }
            else
            {
                dto.codigo = DTO_CodigoResultado.ERROR_RESPUESTA;
                dto.mensaje = $"{response.StatusCode}:{response.Content}";
            }
        }
        catch (System.Net.Http.HttpRequestException ex)
        {
            dto = new DTO_Respuesta<string>();
            dto.codigo = DTO_CodigoResultado.FALLO_RED;
            dto.mensaje = "Fallo en la conexión, vuelva a intentar";
        }
        catch (Exception ex) 
        {
            dto = new DTO_Respuesta<string>();
            dto.codigo = DTO_CodigoResultado.RESPUESTA_NO_COMPLETA;
            dto.mensaje = ex.Message+ "|"+ex?.StackTrace?.ToString();
        }
        return dto;
    }
}
