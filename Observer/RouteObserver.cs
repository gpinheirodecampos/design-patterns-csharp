// Arquivo: RouteObserver.cs
// Implementa o padrão Observer para notificações relacionadas a rotas

using System;
using System.Collections.Generic;
using RouteRecommendationSystem.Models;

namespace RouteRecommendationSystem.Observer
{
    // Interface para o assunto observável (Subject)
    public interface IRouteSubject
    {
        void RegisterObserver(IRouteObserver observer);
        void RemoveObserver(IRouteObserver observer);
        void NotifyObservers(Route route);
    }
    
    // Interface para os observadores
    public interface IRouteObserver
    {
        void Update(Route route);
    }
    
    // Implementação concreta do Subject para monitorar rotas
    public class RouteMonitor : IRouteSubject
    {
        private readonly List<IRouteObserver> _observers;
        
        public RouteMonitor()
        {
            _observers = new List<IRouteObserver>();
        }
        
        // Registra um novo observador
        public void RegisterObserver(IRouteObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
                
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }
        
        // Remove um observador
        public void RemoveObserver(IRouteObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
                
            _observers.Remove(observer);
        }
        
        // Notifica todos os observadores sobre uma nova rota
        public void NotifyObservers(Route route)
        {
            foreach (var observer in _observers)
            {
                observer.Update(route);
            }
        }
        
        // Método para quando uma nova rota é calculada
        public void RouteCalculated(Route route)
        {
            Console.WriteLine("RouteMonitor: Nova rota calculada, notificando observadores...");
            NotifyObservers(route);
        }
    }
    
    // Observador para registrar histórico de rotas
    public class RouteHistoryObserver : IRouteObserver
    {
        private readonly List<RouteHistoryEntry> _routeHistory;
        private const int MaxHistorySize = 10;
        
        public RouteHistoryObserver()
        {
            _routeHistory = new List<RouteHistoryEntry>();
        }
        
        public void Update(Route route)
        {
            // Cria uma entrada no histórico para a nova rota
            var historyEntry = new RouteHistoryEntry
            {
                Origin = route.Origin,
                Destination = route.Destination,
                TransportModeName = route.TransportModeName,
                Distance = route.Distance,
                Timestamp = DateTime.Now
            };
            
            // Adiciona a entrada ao histórico
            _routeHistory.Add(historyEntry);
            
            // Remove entradas antigas se o histórico estiver cheio
            if (_routeHistory.Count > MaxHistorySize)
            {
                _routeHistory.RemoveAt(0);
            }
            
            Console.WriteLine($"RouteHistoryObserver: Rota de {route.Origin} para {route.Destination} adicionada ao histórico.");
        }
        
        // Método para exibir o histórico de rotas
        public void DisplayHistory()
        {
            Console.WriteLine("\n=== Histórico de Rotas ===");
            
            if (_routeHistory.Count == 0)
            {
                Console.WriteLine("Não há rotas no histórico.");
                return;
            }
            
            foreach (var entry in _routeHistory)
            {
                Console.WriteLine($"{entry.Timestamp.ToString("dd/MM HH:mm")}: {entry.Origin} -> {entry.Destination} ({entry.TransportModeName}, {entry.Distance:F1} km)");
            }
        }
        
        // Método para limpar o histórico
        public void ClearHistory()
        {
            _routeHistory.Clear();
            Console.WriteLine("Histórico de rotas limpo.");
        }
        
        // Classe interna para armazenar informações resumidas de uma rota
        private class RouteHistoryEntry
        {
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string TransportModeName { get; set; }
            public double Distance { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
    
    // Observador para notificações de tráfego
    public class TrafficAlertObserver : IRouteObserver
    {
        // Limiar para considerarmos um trajeto longo
        private const double LongRouteThreshold = 10.0; // km
        
        public void Update(Route route)
        {
            // Analisa a rota e gera alertas relevantes
            
            // Verifica se é uma rota longa
            if (route.Distance > LongRouteThreshold)
            {
                Console.WriteLine($"\nTrafficAlertObserver: ⚠️ ALERTA - Rota longa detectada ({route.Distance:F1} km)!");
                
                // Sugestões baseadas no meio de transporte
                if (route.TransportModeName.Equals("Carro", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Sugestão: Considere verificar as condições de tráfego atuais antes de partir.");
                }
                else if (route.TransportModeName.Equals("Transporte Público", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Sugestão: Verifique os horários de partida para evitar esperas longas.");
                }
                else if (route.TransportModeName.Equals("Bicicleta", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Sugestão: Lembre-se de levar água e proteção solar para trajetos longos de bicicleta.");
                }
                else if (route.TransportModeName.Equals("A Pé", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Sugestão: Trajeto longo para caminhada, considere usar outro meio de transporte.");
                }
            }
            
            // Analisa o tempo de viagem
            if (route.EstimatedTime > 60) // Mais de 1 hora
            {
                Console.WriteLine($"\nTrafficAlertObserver: ⏱️ ALERTA - Tempo de viagem longo: {route.EstimatedTime} minutos!");
                Console.WriteLine("Sugestão: Planeje paradas para descanso durante a viagem.");
            }
        }
    }
}
