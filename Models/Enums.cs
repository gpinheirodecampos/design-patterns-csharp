// Arquivo: Enums.cs
// Contém as enumerações utilizadas no sistema de recomendação de rotas

using System;

namespace RouteRecommendationSystem.Models
{
    // Enumeração para diferentes tipos de transporte
    public enum TransportType
    {
        Car,        // Carro
        PublicTransport,  // Transporte Público
        Bicycle,    // Bicicleta
        Walking     // A pé
    }

    // Enumeração para condições climáticas
    public enum WeatherCondition
    {
        Sunny,      // Ensolarado
        Rainy,      // Chuvoso
        Cloudy,     // Nublado
        Snowy       // Neve
    }

    // Enumeração para condições de tráfego
    public enum TrafficCondition
    {
        Light,      // Leve
        Moderate,   // Moderado
        Heavy,      // Intenso
        Gridlock    // Congestionado
    }
}
