// Arquivo: CachedRouteProxy.cs
// Implementa o padrão Proxy para cache de rotas

using System;
using System.Collections.Generic;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Factory;
using RouteRecommendationSystem.Singleton;

namespace RouteRecommendationSystem.Proxy
{
    // Implementação do Proxy para cacheamento de rotas
    public class CachedRouteProxy : RouteService
    {
        private readonly RouteService _routeService;
        private readonly Dictionary<string, Route> _cache;
        private readonly int _maxCacheSize;
        
        public CachedRouteProxy(RouteService routeService)
        {
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _cache = new Dictionary<string, Route>();
            
            // Obtém o tamanho máximo do cache das configurações
            var settings = SettingsManager.GetInstance();
            _maxCacheSize = settings.GetSetting<int>("MaxCacheSize");
        }
        
        // Implementação do método para obter rota, com cache
        public Route GetRoute(string origin, string destination, TransportMode transportMode)
        {
            // Gera uma chave única para a rota
            string cacheKey = GenerateCacheKey(origin, destination, transportMode.GetName());
            
            // Verifica se a configuração de cache está habilitada
            var settings = SettingsManager.GetInstance();
            bool cacheEnabled = settings.GetSetting<bool>("CacheEnabled");
            
            if (!cacheEnabled)
            {
                Console.WriteLine("CachedRouteProxy: Cache desabilitado, calculando rota...");
                return _routeService.GetRoute(origin, destination, transportMode);
            }
            
            // Verifica se a rota está em cache
            if (_cache.TryGetValue(cacheKey, out var cachedRoute))
            {
                Console.WriteLine($"CachedRouteProxy: Rota encontrada em cache para {origin} -> {destination} usando {transportMode.GetName()}");
                return cachedRoute;
            }
            
            // Se não está em cache, obtém do serviço real
            Console.WriteLine($"CachedRouteProxy: Cache miss para {origin} -> {destination}, calculando nova rota...");
            Route route = _routeService.GetRoute(origin, destination, transportMode);
            
            // Adiciona ao cache
            if (_cache.Count >= _maxCacheSize)
            {
                // Remove uma entrada aleatória se o cache estiver cheio
                RemoveRandomCacheEntry();
            }
            
            _cache[cacheKey] = route;
            Console.WriteLine($"CachedRouteProxy: Rota adicionada ao cache. Tamanho atual do cache: {_cache.Count}");
            
            return route;
        }
        
        // Método auxiliar para gerar chave de cache
        private string GenerateCacheKey(string origin, string destination, string transportMode)
        {
            return $"{origin}|{destination}|{transportMode}".ToLower();
        }
        
        // Método para remover uma entrada aleatória do cache quando estiver cheio
        private void RemoveRandomCacheEntry()
        {
            if (_cache.Count == 0) return;
            
            // Obtém as chaves do cache
            var keys = new List<string>(_cache.Keys);
            
            // Remove uma entrada aleatória
            var random = new Random();
            int indexToRemove = random.Next(keys.Count);
            
            string keyToRemove = keys[indexToRemove];
            _cache.Remove(keyToRemove);
            
            Console.WriteLine($"CachedRouteProxy: Cache cheio, removida entrada antiga: {keyToRemove}");
        }
        
        // Método para limpar o cache
        public void ClearCache()
        {
            _cache.Clear();
            Console.WriteLine("CachedRouteProxy: Cache limpo");
        }
        
        // Método para obter o número de entradas em cache
        public int GetCacheSize()
        {
            return _cache.Count;
        }
    }
}
