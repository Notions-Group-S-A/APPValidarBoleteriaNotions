﻿
using System.Text.Json;

namespace APPValidarBoleteriaNotions.Services;

public class ContextoService
{
    private const string FileName = "contexto.json";

    private string FilePath 
    {
        get 
        {
            return Path.Combine(FileSystem.AppDataDirectory, FileName);
        }
    }

    public async Task GuardarContextoAsync(Contexto contexto)
    {
   
        try
        {
            string json = JsonSerializer.Serialize(contexto);
            await File.WriteAllTextAsync(FilePath, json);
        }
        catch (Exception ex)
        {
        }
    }

    public async Task<Contexto?> CargarContextoAsync()
    {
        try
        {
            if (File.Exists(FilePath) == false)
            {
                return new Contexto();
            }
            //return new Contexto();
            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<Contexto>(json);
        }
        catch (Exception ex)
        {
        }

        return new Contexto();
    }

    
}
