using APPValidarBoleteriaClientService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APPValidarBoleteriaClientService
{
    //Boleteria.Gateway.API
    public class AuthControllerClientService
    {
        //http://desa-apigateway.boleteriadigital.com.ar/swagger/ui/index#/AuthController
        //string urlBase = "apigateway.boleteriadigital.com.ar";
        string urlBase = "http://desa-apigateway.boleteriadigital.com.ar";

        async public Task<DTO_Respuesta> GetEnpoint(string ente)
        {
            string resource = $"/api/AuthController/Get_Endpoint?Ente={ente}";
            string url = $"{urlBase}{resource}";
    
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode == true)
                {
                    var objContent = await response.Content.ReadFromJsonAsync<DTO_Respuesta>();
                    return objContent;
                }
            }
            return null;
        }
    }
}
