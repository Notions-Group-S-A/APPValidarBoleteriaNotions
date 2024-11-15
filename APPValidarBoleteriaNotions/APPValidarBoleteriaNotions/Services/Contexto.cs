

namespace APPValidarBoleteriaNotions.Services;

public class Contexto
{
    public string? Usuario { get; set; }
    public bool IsAuthenticated { get; set; } = false;
    public bool IsSincronizado { get; set; } = false;
    public string? Ente { get; set; }
    public string? URLEndPoint { get; set; }
}

