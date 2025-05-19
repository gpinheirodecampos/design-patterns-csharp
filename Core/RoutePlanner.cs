// Arquivo: RoutePlanner.cs
// Classe principal que usa o padrão Strategy para calcular rotas

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Strategies;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Core
{
    // Esta classe contém o contexto para o padrão Strategy
    public class RoutePlanner
    {
        private RouteStrategy _strategy;

        public RoutePlanner()
        {
            // Por padrão, inicialmente não temos uma estratégia definida
            _strategy = null;
        }

        public void SetStrategy(RouteStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public Route PlanRoute(string origin, string destination, TransportMode transportMode,
            WeatherCondition weather = WeatherCondition.Sunny,
            TrafficCondition traffic = TrafficCondition.Moderate)
        {
            if (_strategy == null)
                throw new InvalidOperationException("Nenhuma estratégia de rota foi definida.");

            if (string.IsNullOrEmpty(origin))
                throw new ArgumentException("A origem não pode estar vazia", nameof(origin));
            
            if (string.IsNullOrEmpty(destination))
                throw new ArgumentException("O destino não pode estar vazio", nameof(destination));

            // Calculamos a rota usando a estratégia escolhida
            return _strategy.CalculateRoute(origin, destination, transportMode, weather, traffic);
        }
    }
}
