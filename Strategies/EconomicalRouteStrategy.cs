// Arquivo: EconomicalRouteStrategy.cs
// Implementação da estratégia de rota mais econômica

using System;
using System.Collections.Generic;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Strategies
{
    // Estratégia para calcular a rota mais econômica
    public class EconomicalRouteStrategy : RouteStrategy
    {
        public Route CalculateRoute(string origin, string destination, TransportMode transportMode, 
            WeatherCondition weather, TrafficCondition traffic)
        {
            Console.WriteLine("Calculando rota mais ECONÔMICA...");
            
            // Simula o cálculo de rota baseado na minimização de custo
            // Em um sistema real, isso usaria dados de mapas e algoritmos reais
            
            Random rand = new Random();
            double distance = SimulateDistance(origin, destination);
            
            // Rota econômica pode ajustar com base no tipo de transporte
            if (transportMode is Car)
            {
                // Otimiza para eficiência de combustível
                distance *= 1.05; // Rota ligeiramente mais longa para evitar pedágios
            }
            
            int timeMultiplier = GetTimeMultiplier(traffic, weather, transportMode);
            int estimatedTime = (int)(distance * timeMultiplier / transportMode.GetSpeed() * 60); // Convertendo para minutos
            
            // Calcula custo com otimizações econômicas
            double cost = CalculateCost(distance, transportMode);
            cost *= 0.85; // 15% de desconto evitando pedágios, usando direção econômica, etc.
            
            double co2Emission = CalculateCO2(distance, transportMode, traffic);

            var route = new Route
            {
                Origin = origin,
                Destination = destination,
                Distance = distance,
                EstimatedTime = estimatedTime,
                Cost = cost,
                CO2Emission = co2Emission,
                TransportModeName = transportMode.GetName()
            };
            
            // Adiciona instruções específicas para esta estratégia
            route.Instructions.Add($"Inicie a rota em {origin}");
            route.Instructions.Add("Siga pela rota com menor custo total");
            route.Instructions.Add("Evite pedágios e opte por vias econômicas");
            route.Instructions.Add($"Custo estimado: R$ {cost:F2}");
            route.Instructions.Add($"Chegue ao seu destino {destination}");

            return route;
        }

        // Métodos auxiliares para simulação
        private double SimulateDistance(string origin, string destination)
        {
            // Em um sistema real, isso usaria dados de mapas reais
            // Aqui estamos simulando com base nos comprimentos das strings para demonstração
            return (origin.Length + destination.Length) * 0.5;
        }

        private int GetTimeMultiplier(TrafficCondition traffic, WeatherCondition weather, TransportMode transport)
        {
            int baseMultiplier = 2;
            
            // Ajuste para tráfego
            switch (traffic)
            {
                case TrafficCondition.Light: baseMultiplier += 0; break;
                case TrafficCondition.Moderate: baseMultiplier += 1; break;
                case TrafficCondition.Heavy: baseMultiplier += 3; break;
                case TrafficCondition.Gridlock: baseMultiplier += 5; break;
            }
            
            // Ajuste para clima
            switch (weather)
            {
                case WeatherCondition.Sunny: baseMultiplier += 0; break;
                case WeatherCondition.Cloudy: baseMultiplier += 0; break;
                case WeatherCondition.Rainy: baseMultiplier += 1; break;
                case WeatherCondition.Snowy: baseMultiplier += 3; break;
            }
            
            // Rotas econômicas podem ser mais lentas
            baseMultiplier += 1;
            
            return baseMultiplier;
        }

        private double CalculateCost(double distance, TransportMode transportMode)
        {
            if (transportMode is PublicTransport)
            {
                return transportMode.GetCost(); // Custo fixo para transporte público
            }
            
            return distance * transportMode.GetCost(); // Custo por km para outros modos
        }

        private double CalculateCO2(double distance, TransportMode transportMode, TrafficCondition traffic)
        {
            double baseEmission = distance * transportMode.GetEmissionFactor();
            
            // Tráfego afeta emissão
            switch (traffic)
            {
                case TrafficCondition.Light: return baseEmission * 1.0;
                case TrafficCondition.Moderate: return baseEmission * 1.2;
                case TrafficCondition.Heavy: return baseEmission * 1.5;
                case TrafficCondition.Gridlock: return baseEmission * 2.0;
                default: return baseEmission;
            }
        }
    }
}
