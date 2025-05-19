// Arquivo: Route.cs
// Classe de modelo para armazenar informações da rota

using System;
using System.Collections.Generic;

namespace RouteRecommendationSystem.Models
{
    // Classe Rota para armazenar informações da rota
    public class Route
    {
        public string Origin { get; set; }      // Origem
        public string Destination { get; set; } // Destino
        public double Distance { get; set; }    // Distância em quilômetros
        public int EstimatedTime { get; set; }  // Tempo estimado em minutos
        public double Cost { get; set; }        // Custo em unidades monetárias (R$)
        public double CO2Emission { get; set; } // Emissão de CO2 em kg
        public List<string> Instructions { get; set; } = new List<string>(); // Instruções da rota
        public string TransportModeName { get; set; } // Nome do modo de transporte usado

        public Route()
        {
            // Construtor padrão
        }

        public Route(string origin, string destination)
        {
            Origin = origin;
            Destination = destination;
        }

        // Método para representação da rota em formato de string
        public virtual string Display()
        {
            return $"Rota de {Origin} para {Destination}:\n" +
                   $"Meio de Transporte: {TransportModeName}\n" +
                   $"Distância: {Distance:F2} km\n" +
                   $"Tempo Estimado: {EstimatedTime} minutos\n" +
                   $"Custo: R$ {Cost:F2}\n" +
                   $"Emissão de CO2: {CO2Emission:F2} kg";
        }

        // Clone a rota (útil para decoradores)
        public Route Clone()
        {
            return new Route
            {
                Origin = this.Origin,
                Destination = this.Destination,
                Distance = this.Distance,
                EstimatedTime = this.EstimatedTime,
                Cost = this.Cost,
                CO2Emission = this.CO2Emission,
                Instructions = new List<string>(this.Instructions),
                TransportModeName = this.TransportModeName
            };
        }
    }
}
