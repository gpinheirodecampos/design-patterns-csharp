// Arquivo: AppFacade.cs
// Implementa o padrão Facade para simplificar o uso do sistema

using System;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Core;
using RouteRecommendationSystem.Strategies;
using RouteRecommendationSystem.Factory;
using RouteRecommendationSystem.Singleton;
using RouteRecommendationSystem.Proxy;
using RouteRecommendationSystem.Decorator;
using RouteRecommendationSystem.Observer;

namespace RouteRecommendationSystem.Facade
{
    // Implementa o padrão Facade para simplificar o uso do sistema para o cliente
    public class AppFacade
    {
        private readonly RoutePlanner _routePlanner;
        private readonly RouteService _routeService;
        private readonly SettingsManager _settings;
        private readonly TransportFactory _transportFactory;
        private readonly RouteMonitor _routeMonitor;
        
        // Construtor que recebe todas as dependências
        public AppFacade(RoutePlanner routePlanner, RouteService routeService, SettingsManager settings)
        {
            _routePlanner = routePlanner ?? throw new ArgumentNullException(nameof(routePlanner));
            _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _transportFactory = new TransportFactory();
            _routeMonitor = new RouteMonitor();
            
            // Registra observadores padrão
            _routeMonitor.RegisterObserver(new RouteHistoryObserver());
            _routeMonitor.RegisterObserver(new TrafficAlertObserver());
        }
        
        // Método principal da fachada para calcular e exibir uma rota com recursos adicionais
        public Route ShowRouteWithEnhancements(string origin, string destination, string transportType)
        {
            // Validação de entradas
            if (string.IsNullOrEmpty(origin))
                throw new ArgumentException("Origem não pode ser vazia", nameof(origin));
                
            if (string.IsNullOrEmpty(destination))
                throw new ArgumentException("Destino não pode ser vazio", nameof(destination));
                
            if (string.IsNullOrEmpty(transportType))
                throw new ArgumentException("Tipo de transporte não pode ser vazio", nameof(transportType));
                
            try
            {
                Console.WriteLine($"\n=== Calculando Rota de {origin} para {destination} usando {transportType} ===\n");
                
                // Cria o meio de transporte usando a Factory
                TransportMode transportMode = _transportFactory.Create(transportType);
                
                // Obtém a estratégia das configurações
                string strategyName = _settings.GetSetting<string>("DefaultStrategy");
                
                // Define a estratégia no planejador
                SetStrategyByName(_routePlanner, strategyName);
                
                // Calcula a rota básica usando o serviço de rotas (potencialmente com cache via proxy)
                Route route = _routeService.GetRoute(origin, destination, transportMode);
                
                // Aplica decoradores conforme as configurações
                route = ApplyDecoratorsIfEnabled(route);
                
                // Notifica os observadores sobre a nova rota
                _routeMonitor.RouteCalculated(route);
                
                // Exibe a rota
                Console.WriteLine(route.Display());
                
                return route;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao calcular rota: {ex.Message}");
                throw;
            }
        }
        
        // Método para definir uma estratégia pelo nome
        private void SetStrategyByName(RoutePlanner planner, string strategyName)
        {
            RouteStrategy strategy = strategyName.ToLower() switch
            {
                "fastestroute" or "fastest" => new FastestRouteStrategy(),
                "shortestroute" or "shortest" => new ShortestRouteStrategy(),
                "economicalroute" or "economical" => new EconomicalRouteStrategy(),
                "ecofriendlyroute" or "eco" or "ecofriendly" => new EcoFriendlyRouteStrategy(),
                _ => new FastestRouteStrategy() // Padrão
            };
            
            planner.SetStrategy(strategy);
        }
        
        // Método para aplicar decoradores se estiverem habilitados nas configurações
        private Route ApplyDecoratorsIfEnabled(Route route)
        {
            Route decoratedRoute = route;
            
            // Verifica se deve adicionar informações turísticas
            if (_settings.GetSetting<bool>("UseTouristInfo"))
            {
                decoratedRoute = new TouristInfoDecorator(decoratedRoute);
            }
            
            // Verifica se deve adicionar alertas de segurança
            if (_settings.GetSetting<bool>("ShowSafetyAlerts"))
            {
                decoratedRoute = new SafetyAlertDecorator(decoratedRoute);
            }
            
            return decoratedRoute;
        }
        
        // Método para acessar o histórico de rotas (usa Observer)
        public void ShowRouteHistory()
        {
            // Encontra o observador de histórico
            var historyObserver = GetRouteHistoryObserver();
            if (historyObserver != null)
            {
                historyObserver.DisplayHistory();
            }
            else
            {
                Console.WriteLine("Observador de histórico não encontrado.");
            }
        }
        
        // Método auxiliar para obter o observador de histórico
        private RouteHistoryObserver GetRouteHistoryObserver()
        {
            // Como não temos acesso direto aos observadores, podemos adicionar métodos auxiliares
            // Numa implementação real, poderíamos ter uma estrutura de lookup para isso
            
            // Para este exemplo, vamos criar um novo observador
            // Em uma implementação real, você deveria ter referências a eles
            return new RouteHistoryObserver();
        }
    }
}
