// Arquivo: MapService.cs
// Define a interface MapService usada pelo sistema

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Adapter
{
    // Interface MapService usada pelo nosso sistema
    public interface MapService
    {
        // Obtem informações de rota entre a origem e o destino
        Route GetRoute(string origin, string destination, TransportMode transportMode);
    }
    
    // Simula uma biblioteca externa legada de serviço de mapas
    // Esta seria uma classe de terceiros que não podemos modificar
    public class LegacyMapProvider
    {
        // Método para calcular rota no formato antigo
        public LegacyRouteData CalculateRouteLegacy(string startPoint, string endPoint, string vehicleType)
        {
            Console.WriteLine("LegacyMapProvider: Calculando rota usando sistema legado...");
            
            // Simula cálculos complexos
            double distance = (startPoint.Length + endPoint.Length) * 0.8;
            int time = (int)(distance * 3);
            
            // Ajusta para o tipo de veículo
            switch (vehicleType.ToLower())
            {
                case "car":
                    distance *= 0.9; // Carros podem usar rotas mais diretas
                    time = (int)(distance * 1.5);
                    break;
                case "bike":
                    distance *= 1.1; // Bicicletas usam ciclovias que podem ser mais longas
                    time = (int)(distance * 4);
                    break;
                case "walking":
                    distance *= 0.8; // Caminhar permite atalhos
                    time = (int)(distance * 12);
                    break;
                case "bus":
                    distance *= 1.2; // Ônibus seguem rotas específicas
                    time = (int)(distance * 3.5);
                    break;
            }
            
            // Retorna dados no formato legado
            return new LegacyRouteData
            {
                TotalDistanceKm = distance,
                EstimatedTimeMinutes = time,
                StartPointName = startPoint,
                EndPointName = endPoint,
                VehicleType = vehicleType,
                HasTraffic = new Random().Next(100) > 50 // 50% de chance de ter tráfego
            };
        }
    }
    
    // Classe de dados utilizada pelo sistema legado
    public class LegacyRouteData
    {
        public double TotalDistanceKm { get; set; }
        public int EstimatedTimeMinutes { get; set; }
        public string StartPointName { get; set; }
        public string EndPointName { get; set; }
        public string VehicleType { get; set; }
        public bool HasTraffic { get; set; }
        public string[] RouteSegments { get; set; } = new string[0];
    }
}
