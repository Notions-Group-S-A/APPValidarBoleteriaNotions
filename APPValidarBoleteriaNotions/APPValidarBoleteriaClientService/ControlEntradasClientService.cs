using APPValidarBoleteriaClientService.Models;
using System.Net.Http.Json;
using System.Text.Json;

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
            using var client = new HttpClient();
            
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                string content= await response.Content.ReadAsStringAsync();
                dto = await response.Content.ReadFromJsonAsync<DTO_RespuestaEntrada<DTO_Entrada>>();
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
            using var client = new HttpClient();
            
            var response = await client.PostAsync(url, new StringContent(""));

            if (response.IsSuccessStatusCode == true)
            {
                dto= new DTO_RespuestaEntrada();
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

        //string resource = $"/api/ControlEntradas/Login?Usuario={usuario}&Clave={clave}";
        string resource = $"Login?Usuario={usuario}&Clave={clave}";
        string url = $"{URL_Base}{resource}";
        //$"http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/Login?Usuario={usuario}&Clave={clave}";
        //$"

        try
        {
            using var client = new HttpClient();
            
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode == true)
            {
                //dto = await response.Content.ReadFromJsonAsync<DTO_RespuestaEntrada<bool>>();
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false // Ignora mayúsculas/minúsculas
                };
                dto = JsonSerializer.Deserialize<DTO_RespuestaEntrada>(content, options);
                return dto;
            }

            return new DTO_RespuestaEntrada() { codigo = DTO_CodigoEntrada.NO_SUCESS };
            
        }
        catch (Exception ex)
        {
            dto = new DTO_RespuestaEntrada();
            dto.codigo = DTO_CodigoEntrada.NO_SUCESS;
            dto.mensaje = ex.Message;
        }

        return dto;
    }
}
