﻿

namespace APPValidarBoleteriaNotions.Services;

public class Contexto
{
    public string? Usuario { get; set; }
    public bool Logueado { get; set; } = false;
    public bool Sincronizado { get; set; } = false;
    public string? Ente { get; set; }
    public string? URLEndPoint { get; set; }
}
