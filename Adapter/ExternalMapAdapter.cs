// Arquivo: ExternalMapAdapter.cs
// Implementa o padrão Adapter para adaptar o serviço de mapa legado à nossa interface

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Adapter
{
    // Implementa o padrão Adapter para adaptar o serviço de mapa legado
    public class ExternalMapAdapter : MapService
    {
        private readonly LegacyMapProvider _legacyMapProvider;
        
        public ExternalMapAdapter(LegacyMapProvider legacyMapProvider)
        {
            _legacyMapProvider = legacyMapProvider ?? throw new ArgumentNullException(nameof(legacyMapProvider));
        }
        
        // Implementação do método GetRoute que converte chamada para o formato legado
        public Route GetRoute(string origin, string destination, TransportMode transportMode)
        {
            // Converte o tipo de transporte para a string esperada pelo sistema legado
            string vehicleType = ConvertTransportModeToVehicleType(transportMode);
            
            // Chama o sistema legado
            LegacyRouteData legacyData = _legacyMapProvider.CalculateRouteLegacy(origin, destination, vehicleType);
            
            // Converte a resposta legada para o formato do nosso sistema
            Route route = new Route
            {
                Origin = legacyData.StartPointName,
                Destination = legacyData.EndPointName,
                Distance = legacyData.TotalDistanceKm,
                EstimatedTime = legacyData.EstimatedTimeMinutes,
                TransportModeName = transportMode.GetName(),
                // Calculamos o custo baseado na distância e tipo de transporte
                Cost = CalculateCost(legacyData.TotalDistanceKm, transportMode),
                // Calculamos a emissão de CO2 baseado na distância e tipo de transporte
                CO2Emission = CalculateCO2Emission(legacyData.TotalDistanceKm, transportMode)
            };
            
            // Adiciona instruções de rota
            route.Instructions.Add($"Inicie a rota em {origin}");
            route.Instructions.Add($"Siga por {legacyData.TotalDistanceKm:F1} km usando {transportMode.GetName()}");
            
            if (legacyData.HasTraffic)
            {
                route.Instructions.Add("Atenção: Há tráfego no caminho!");
                // Aumenta o tempo em 20% se houver tráfego
                route.EstimatedTime = (int)(route.EstimatedTime * 1.2);
            }
            
            route.Instructions.Add($"Chegue ao seu destino {destination}");
            
            return route;
        }
        
        // Método auxiliar para converter nosso TransportMode para o formato de string do sistema legado
        private string ConvertTransportModeToVehicleType(TransportMode transportMode)
        {
            return transportMode switch
            {
                Car => "car",
                Bike => "bike",
                Walking => "walking",
                PublicTransport => "bus",
                _ => throw new ArgumentException("Tipo de transporte não suportado")
            };
        }
        
        // Método auxiliar para calcular o custo da rota
        private double CalculateCost(double distance, TransportMode transportMode)
        {
            if (transportMode is PublicTransport)
            {
                return transportMode.GetCost(); // Custo fixo para transporte público
            }
            
            return distance * transportMode.GetCost(); // Custo por km para outros modos
        }
        
        // Método auxiliar para calcular a emissão de CO2
        private double CalculateCO2Emission(double distance, TransportMode transportMode)
        {
            return distance * transportMode.GetEmissionFactor();
        }
    }
}
