// Arquivo: TransportFactory.cs
// Implementa o padrão Factory Method para criar meios de transporte

using System;

namespace RouteRecommendationSystem.Factory
{
    // Factory para criar diferentes tipos de meios de transporte
    public class TransportFactory
    {
        // Método de fábrica para criar instâncias de TransportMode baseadas no tipo
        public TransportMode Create(string type)
        {
            // Normaliza o tipo para minúsculo e remove espaços para facilitar a comparação
            string normalizedType = type.ToLower().Trim();
            
            return normalizedType switch
            {
                "carro" or "car" => new Car(),
                "transporte público" or "onibus" or "ônibus" or "public" or "publictransport" => new PublicTransport(),
                "bicicleta" or "bike" => new Bike(),
                "a pé" or "a pe" or "walking" or "walk" or "pe" or "pé" => new Walking(),
                _ => throw new ArgumentException($"Tipo de transporte não suportado: {type}")
            };
        }
    }
}
