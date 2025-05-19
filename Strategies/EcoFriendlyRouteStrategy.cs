// Arquivo: EcoFriendlyRouteStrategy.cs
// Implementação da estratégia de rota mais ecológica

using System;
using System.Collections.Generic;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Strategies
{
    // Estratégia para calcular a rota mais ecológica
    public class EcoFriendlyRouteStrategy : RouteStrategy
    {
        public Route CalculateRoute(string origin, string destination, TransportMode transportMode, 
            WeatherCondition weather, TrafficCondition traffic)
        {
            Console.WriteLine("Calculando rota mais ECOLÓGICA...");
            
            // Simula o cálculo de rota baseado na minimização de emissão de carbono
            // Em um sistema real, isso usaria dados de mapas e algoritmos reais
            
            Random rand = new Random();
            double distance = SimulateDistance(origin, destination);
            
            // Ajustes para rota ecológica
            if (transportMode is Car)
            {
                // Rota evita áreas congestionadas mesmo que seja mais longa
                if (traffic == TrafficCondition.Heavy || traffic == TrafficCondition.Gridlock)
                {
                    distance *= 1.15; // 15% mais longo para evitar áreas de alta emissão
                }
            }
            
            int timeMultiplier = GetTimeMultiplier(traffic, weather, transportMode);
            int estimatedTime = (int)(distance * timeMultiplier / transportMode.GetSpeed() * 60); // Convertendo para minutos
            double cost = CalculateCost(distance, transportMode);
            
            // Calcula CO2 com otimizações ecológicas
            double co2Emission = CalculateCO2(distance, transportMode, traffic);
            co2Emission *= 0.8; // 20% de redução tomando rotas eco-otimizadas
            
            // Rotas ecológicas priorizam emissões menores mesmo se tempo ou custo forem maiores
            estimatedTime = (int)(estimatedTime * 1.1); // 10% mais tempo potencialmente

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
            route.Instructions.Add("Siga pela rota com menor impacto ambiental");
            route.Instructions.Add("Evite áreas congestionadas para reduzir emissões");
            route.Instructions.Add($"Emissão estimada de CO2: {co2Emission:F2} kg");
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
            
            // Rotas ecológicas podem preferir caminhos alternativos
            if (transport is Car && 
                (traffic == TrafficCondition.Heavy || traffic == TrafficCondition.Gridlock))
            {
                baseMultiplier += 2; // Tomando rotas menos congestionadas mas mais longas
            }
            
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
