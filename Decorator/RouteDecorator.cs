// Arquivo: RouteDecorator.cs
// Implementa o padrão Decorator para incremento visual e funcional nas rotas

using System;
using System.Collections.Generic;
using RouteRecommendationSystem.Models;

namespace RouteRecommendationSystem.Decorator
{
    // Classe base para todos os decoradores de rota
    public abstract class RouteDecorator : Route
    {
        protected Route _decoratedRoute;
        
        public RouteDecorator(Route decoratedRoute)
        {
            if (decoratedRoute == null)
                throw new ArgumentNullException(nameof(decoratedRoute));
                
            // Copia os atributos da rota original
            _decoratedRoute = decoratedRoute;
            this.Origin = decoratedRoute.Origin;
            this.Destination = decoratedRoute.Destination;
            this.Distance = decoratedRoute.Distance;
            this.EstimatedTime = decoratedRoute.EstimatedTime;
            this.Cost = decoratedRoute.Cost;
            this.CO2Emission = decoratedRoute.CO2Emission;
            this.TransportModeName = decoratedRoute.TransportModeName;
            
            // Copia as instruções da rota original
            this.Instructions = new List<string>(decoratedRoute.Instructions);
        }
        
        // Método para exibir a rota, pode ser sobrescrito pelos decoradores concretos
        public override string Display()
        {
            return _decoratedRoute.Display();
        }
    }
    
    // Decorador para adicionar informações turísticas
    public class TouristInfoDecorator : RouteDecorator
    {
        private readonly Dictionary<string, List<string>> _touristAttractions;
        
        public TouristInfoDecorator(Route decoratedRoute) : base(decoratedRoute)
        {
            // Inicializa com algumas atrações turísticas de exemplo
            _touristAttractions = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"São Paulo", new List<string> {
                    "Museu de Arte de São Paulo (MASP)",
                    "Parque Ibirapuera",
                    "Pinacoteca do Estado",
                    "Mercado Municipal",
                    "Avenida Paulista"
                }},
                {"Rio de Janeiro", new List<string> {
                    "Cristo Redentor",
                    "Pão de Açúcar",
                    "Praia de Copacabana",
                    "Maracanã",
                    "Jardim Botânico"
                }},
                {"Centro", new List<string> {
                    "Catedral da Sé",
                    "Teatro Municipal",
                    "Pateo do Collegio",
                    "Edifício Martinelli",
                    "Mercado Municipal"
                }},
                {"Parque Ibirapuera", new List<string> {
                    "Museu de Arte Moderna",
                    "Pavilhão Japonês",
                    "Planetário",
                    "Museu Afro Brasil",
                    "Auditório Ibirapuera"
                }}
            };
            
            // Adiciona informações turísticas à rota
            AddTouristInfo();
        }
        
        // Sobrescreve o método Display para incluir informações turísticas
        public override string Display()
        {
            return base.Display() + "\n\n=== Informações Turísticas ===\n" + GetTouristInfo();
        }
        
        // Método auxiliar para adicionar informações turísticas às instruções
        private void AddTouristInfo()
        {
            // Adiciona atrações próximas à origem
            AddAttractionsNear(Origin, "origem");
            
            // Adiciona atrações próximas ao destino
            AddAttractionsNear(Destination, "destino");
        }
        
        // Método para adicionar atrações próximas a um local
        private void AddAttractionsNear(string location, string locationType)
        {
            // Verifica se há atrações cadastradas para o local
            if (_touristAttractions.TryGetValue(location, out var attractions))
            {
                // Seleciona algumas atrações aleatórias (no máximo 3)
                var random = new Random();
                int attractionCount = Math.Min(3, attractions.Count);
                
                // Cria uma lista temporária das atrações para que possamos embaralhá-las
                var tempList = new List<string>(attractions);
                
                // Embaralha a lista
                for (int i = 0; i < tempList.Count; i++)
                {
                    int j = random.Next(i, tempList.Count);
                    var temp = tempList[i];
                    tempList[i] = tempList[j];
                    tempList[j] = temp;
                }
                
                // Adiciona as atrações selecionadas às instruções
                Instructions.Add($"Atrações turísticas próximas ao {locationType} ({location}):");
                
                for (int i = 0; i < attractionCount; i++)
                {
                    Instructions.Add($"  - {tempList[i]}");
                }
            }
        }
        
        // Retorna texto com informações turísticas
        private string GetTouristInfo()
        {
            bool hasInfo = false;
            string info = "";
            
            // Verifica se há atrações cadastradas para a origem ou destino
            foreach (var location in new[] { Origin, Destination })
            {
                if (_touristAttractions.TryGetValue(location, out var attractions))
                {
                    hasInfo = true;
                    info += $"Pontos turísticos próximos a {location}:\n";
                    
                    foreach (var attraction in attractions)
                    {
                        info += $"- {attraction}\n";
                    }
                    
                    info += "\n";
                }
            }
            
            if (!hasInfo)
            {
                return "Sem informações turísticas disponíveis para esta rota.";
            }
            
            return info;
        }
    }
    
    // Decorador para adicionar alertas de segurança
    public class SafetyAlertDecorator : RouteDecorator
    {
        // Mapeamento de áreas com alertas de segurança
        private readonly Dictionary<string, string> _safetyAlerts;
        
        public SafetyAlertDecorator(Route decoratedRoute) : base(decoratedRoute)
        {
            // Inicializa com alguns alertas de segurança de exemplo
            _safetyAlerts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"Centro", "Mantenha-se atento em áreas movimentadas, especialmente à noite."},
                {"Parque Ibirapuera", "Evite visitar o parque após o horário de fechamento."},
                {"Estação Sé", "Área com alta incidência de furtos, mantenha pertences seguros."},
                {"Praça da República", "Mantenha-se atento aos seus pertences em horários de pico."}
            };
            
            // Adiciona alertas de segurança à rota
            AddSafetyAlerts();
        }
        
        // Sobrescreve o método Display para incluir alertas de segurança
        public override string Display()
        {
            return base.Display() + "\n\n=== Alertas de Segurança ===\n" + GetSafetyAlerts();
        }
        
        // Método auxiliar para adicionar alertas de segurança às instruções
        private void AddSafetyAlerts()
        {
            // Verifica alertas para a origem
            AddSafetyAlertForLocation(Origin);
            
            // Verifica alertas para o destino
            AddSafetyAlertForLocation(Destination);
        }
        
        // Método para adicionar alerta de segurança para um local
        private void AddSafetyAlertForLocation(string location)
        {
            // Verifica se há alertas cadastrados para o local
            if (_safetyAlerts.TryGetValue(location, out var alert))
            {
                Instructions.Add($"⚠️ ALERTA DE SEGURANÇA para {location}: {alert}");
            }
            
            // Verifica também substrings para locais que podem estar contidos no nome do local
            foreach (var key in _safetyAlerts.Keys)
            {
                if (location.Contains(key, StringComparison.OrdinalIgnoreCase) && 
                    !location.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    Instructions.Add($"⚠️ ALERTA DE SEGURANÇA próximo a {key}: {_safetyAlerts[key]}");
                }
            }
        }
        
        // Retorna texto com alertas de segurança
        private string GetSafetyAlerts()
        {
            bool hasAlerts = false;
            string alerts = "";
            
            // Verifica se há alertas cadastrados para a origem ou destino
            foreach (var location in new[] { Origin, Destination })
            {
                if (_safetyAlerts.TryGetValue(location, out var alert))
                {
                    hasAlerts = true;
                    alerts += $"⚠️ {location}: {alert}\n";
                }
                
                // Verifica também substrings
                foreach (var key in _safetyAlerts.Keys)
                {
                    if (location.Contains(key, StringComparison.OrdinalIgnoreCase) && 
                        !location.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        hasAlerts = true;
                        alerts += $"⚠️ Próximo a {key}: {_safetyAlerts[key]}\n";
                    }
                }
            }
            
            if (!hasAlerts)
            {
                return "Sem alertas de segurança para esta rota.";
            }
            
            return alerts;
        }
    }
}
