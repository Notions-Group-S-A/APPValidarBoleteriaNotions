using APPValidarBoleteriaClientService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace APPValidarBoleteriaClientService
{
    //Boleteria.API
    public class ControlEntradasClientService
    {
        //string urlBase = "https://api.boleteriadigital.com.ar";
        //http://desa-api.boleteriadigital.com.ar/swagger/ui/index#/ControlEntradas
        //http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/Get?Codigo=1&Usuario=1
        //http://desa-api.boleteriadigital.com.ar/api/ControlEntradas/QuemarEntrada/

        string urlBase = "http://desa-api.boleteriadigital.com.ar";

        async public Task<DTO_ControlEntrada_Respuesta> GetEnpoint(string codigo, string usuario)
        {
            string resource = $"/api/ControlEntradas/Get?Codigo={codigo}&Usuario={usuario}";
            string url = $"{urlBase}{resource}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode == true)
                {
                    var objContent = await response.Content.ReadFromJsonAsync<DTO_ControlEntrada_Respuesta>();
                    return objContent;
                }
            }
            return null;
        }

        async public Task GetEnpoint(long id, string usuario)
        {
            string resource = $"/api/ControlEntradas/QuemarEntrada/{id}?Usuario={usuario}";
            string url = $"{urlBase}{resource}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(""));

                if (response.IsSuccessStatusCode == true)
                {
                    return;
                }
            }
        }

    }
}
