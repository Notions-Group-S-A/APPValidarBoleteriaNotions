using APPValidarBoleteriaClientService.Models;
using System.Net.Http.Json;

namespace APPValidarBoleteriaClientService;

//Boleteria.API
public class ControlEntradasClientService
{
    #region parametros
    public string URL_Base { get; set; } = "http://desa-api.boleteriadigital.com.ar";
    #endregion

    async public Task<DTO_RespuestaEntrada<DTO_Entrada>> ValidarEntrada(string codigo, string usuario)
    {
        string resource = $"/api/ControlEntradas/Get?Codigo={codigo}&Usuario={usuario}";
        string url = $"{URL_Base}{resource}";

        var dto = new DTO_RespuestaEntrada<DTO_Entrada>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode == true)
                {
                    dto = await response.Content.ReadFromJsonAsync<DTO_RespuestaEntrada<DTO_Entrada>>();
                }
            }
        }
        catch (Exception ex)
        {
            dto.mensaje = ex.Message;
        }
        return dto;
    }

    async public Task<DTO_RespuestaEntrada> QuemarEntrada(long id, string usuario)
    {
        var dto = new DTO_RespuestaEntrada();

        string resource = $"/api/ControlEntradas/QuemarEntrada/{id}?Usuario={usuario}";
        string url = $"{URL_Base}{resource}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(""));

                if (response.IsSuccessStatusCode == true)
                {
                    dto= new DTO_RespuestaEntrada();
                }
            }
        } 
        catch (Exception ex)
        {
            dto = new DTO_RespuestaEntrada();
            dto.mensaje = ex.Message;
        }

        return dto;
    }

    async public Task<DTO_RespuestaEntrada> Login(string usuario, string clave)
    {
        var dto = new DTO_RespuestaEntrada();

        string resource = $"/api/ControlEntradas/Login?Usuario={usuario}&=Clave{clave}";
        string url = $"{URL_Base}{resource}";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(""));

                if (response.IsSuccessStatusCode == true)
                {
                    dto = new DTO_RespuestaEntrada();
                }
            }
        }
        catch (Exception ex)
        {
            dto = new DTO_RespuestaEntrada();
            dto.mensaje = ex.Message;
        }

        return dto;
    }
}
