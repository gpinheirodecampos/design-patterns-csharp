// Arquivo: SettingsManager.cs
// Implementa o padrão Singleton para gerenciar configurações globais

using System;
using System.Collections.Generic;

namespace RouteRecommendationSystem.Singleton
{
    // Gerenciador de configurações como Singleton thread-safe
    public sealed class SettingsManager
    {
        // Instância única do Singleton
        private static SettingsManager _instance;
        
        // Lock object para garantir thread safety
        private static readonly object _lock = new object();
        
        // Dicionário para armazenar as configurações
        private Dictionary<string, object> _settings;
        
        // Construtor privado para impedir instanciação direta
        private SettingsManager()
        {
            _settings = new Dictionary<string, object>();
            
            // Configurações padrão
            _settings["DefaultStrategy"] = "FastestRoute";
            _settings["DefaultTransport"] = "Car";
            _settings["CacheEnabled"] = true;
            _settings["MaxCacheSize"] = 10;
            _settings["MaxRouteDistance"] = 50.0; // km
            _settings["UseTouristInfo"] = true;
            _settings["ShowSafetyAlerts"] = true;
            _settings["DefaultOrigin"] = "Centro";
            _settings["DefaultDestination"] = "Parque Ibirapuera";
            _settings["DebugMode"] = false;
        }
        
        // Método para obter a instância única do Singleton (thread-safe)
        public static SettingsManager GetInstance()
        {
            // Verificação dupla para melhorar performance
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SettingsManager();
                    }
                }
            }
            
            return _instance;
        }
        
        // Obter uma configuração
        public T GetSetting<T>(string key)
        {
            lock (_lock)
            {
                if (_settings.ContainsKey(key))
                {
                    return (T)_settings[key];
                }
                
                throw new KeyNotFoundException($"Configuração não encontrada: {key}");
            }
        }
        
        // Definir uma configuração
        public void SetSetting(string key, object value)
        {
            lock (_lock)
            {
                _settings[key] = value;
            }
        }
        
        // Verificar se uma configuração existe
        public bool HasSetting(string key)
        {
            lock (_lock)
            {
                return _settings.ContainsKey(key);
            }
        }
        
        // Obter todas as configurações
        public Dictionary<string, object> GetAllSettings()
        {
            lock (_lock)
            {
                // Retorna uma cópia para evitar modificações externas diretamente
                return new Dictionary<string, object>(_settings);
            }
        }
    }
}
