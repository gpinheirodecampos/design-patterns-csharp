// Arquivo: TransportMode.cs
// Interface e classes para o padrão Factory Method

using System;

namespace RouteRecommendationSystem.Factory
{
    // Interface para os meios de transporte
    public abstract class TransportMode
    {
        // Método para obter a velocidade média do meio de transporte
        public abstract double GetSpeed(); // km/h
        
        // Método para obter o custo por km
        public abstract double GetCost(); // R$ por km
        
        // Método para obter o nome do meio de transporte
        public abstract string GetName();
        
        // Método para obter o fator de emissão de CO2
        public abstract double GetEmissionFactor(); // kg CO2 por km
    }
    
    // Implementação de carro
    public class Car : TransportMode
    {
        public override double GetSpeed() => 40.0; // Velocidade média de 40 km/h
        public override double GetCost() => 0.70; // R$ 0,70 por km (combustível)
        public override string GetName() => "Carro";
        public override double GetEmissionFactor() => 0.12; // 0,12 kg CO2 por km
    }
    
    // Implementação de transporte público
    public class PublicTransport : TransportMode
    {
        public override double GetSpeed() => 25.0; // Velocidade média de 25 km/h
        public override double GetCost() => 4.50; // Custo fixo da tarifa
        public override string GetName() => "Transporte Público";
        public override double GetEmissionFactor() => 0.05; // 0,05 kg CO2 por km (compartilhado)
    }
    
    // Implementação de bicicleta
    public class Bike : TransportMode
    {
        public override double GetSpeed() => 15.0; // Velocidade média de 15 km/h
        public override double GetCost() => 0.0; // Custo zero
        public override string GetName() => "Bicicleta";
        public override double GetEmissionFactor() => 0.0; // Zero emissão de CO2
    }
    
    // Implementação para deslocamento a pé
    public class Walking : TransportMode
    {
        public override double GetSpeed() => 5.0; // Velocidade média de 5 km/h
        public override double GetCost() => 0.0; // Custo zero
        public override string GetName() => "A Pé";
        public override double GetEmissionFactor() => 0.0; // Zero emissão de CO2
    }
}
