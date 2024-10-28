using APPValidarBoleteriaClientService.Models;
using System.Net.Http.Json;

namespace APPValidarBoleteriaClientService;

//Boleteria.API
public class ControlEntradasClientService
{
    #region
    //string urlBase = "https://api.boleteriadigital.com.ar";
    //http://desa-api.boleteriadigital.com.ar/swagger/ui/index#/ControlEntradas
    //http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/Get?Codigo=1&Usuario=1
    //http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/QuemarEntrada/
    //http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/Get?Codigo=ES88292900&Usuario=123132
    //curl "http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/Get?Codigo=ES88292900&Usuario=123132"
    #endregion

    string urlBase = "http://desa-api.boleteriadigital.com.ar";

    async public Task<DTO_Respuesta<DTO_Entrada>> GetValidarEntrada(string codigo, string usuario)
    {
        string resource = $"/api/ControlEntradas/Get?Codigo={codigo}&Usuario={usuario}";
        string url = $"{urlBase}{resource}";

        var dto = new DTO_Respuesta<DTO_Entrada>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode == true)
                {
                    dto = await response.Content.ReadFromJsonAsync<DTO_Respuesta<DTO_Entrada>>();
                }
            }
        }
        catch (Exception ex)
        {
            dto.mensaje = ex.Message;
        }
        return dto;
    }

    async public Task<DTO_ControlEntrada_Respuesta> GetQuemarEntrada(long id, string usuario)
    {
        var dto = new DTO_ControlEntrada_Respuesta();

        string resource = $"/api/ControlEntradas/QuemarEntrada/{id}?Usuario={usuario}";
        string url = $"{urlBase}{resource}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(""));

                if (response.IsSuccessStatusCode == true)
                {
                    dto= new DTO_ControlEntrada_Respuesta();
                }
            }
        } 
        catch (Exception ex)
        {
            dto = new DTO_ControlEntrada_Respuesta();
            dto.mensaje = ex.Message;
        }

        return dto;
    }
}
