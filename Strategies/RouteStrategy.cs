// Arquivo: RouteStrategy.cs
// Interface que define o contrato do padrão Strategy

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;

namespace RouteRecommendationSystem.Strategies
{
    // Interface que define o contrato do padrão Strategy
    public interface RouteStrategy
    {
        Route CalculateRoute(string origin, string destination, TransportMode transportMode, 
            WeatherCondition weather, TrafficCondition traffic);
    }
}
