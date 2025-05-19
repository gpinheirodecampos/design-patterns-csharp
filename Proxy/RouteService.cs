// Arquivo: RouteService.cs
// Define a interface RouteService e implementação base

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;
using RouteRecommendationSystem.Adapter;

namespace RouteRecommendationSystem.Proxy
{
    // Interface para o serviço de rotas
    public interface RouteService
    {
        Route GetRoute(string origin, string destination, TransportMode transportMode);
    }
    
    // Implementação base do serviço de rota que usa o serviço de mapas
    public class BaseRouteService : RouteService
    {
        private readonly MapService _mapService;
        
        public BaseRouteService(MapService mapService)
        {
            _mapService = mapService ?? throw new ArgumentNullException(nameof(mapService));
        }
        
        // Implementação do método para obter rota
        public Route GetRoute(string origin, string destination, TransportMode transportMode)
        {
            Console.WriteLine("BaseRouteService: Calculando rota...");
            return _mapService.GetRoute(origin, destination, transportMode);
        }
    }
}
