using System;
using System.Collections.Generic;
using System.Threading;
using RouteRecommendationSystem.Models;
using RouteRecommendationSystem.Strategies;
using RouteRecommendationSystem.Core;
using RouteRecommendationSystem.Factory;
using RouteRecommendationSystem.Singleton;
using RouteRecommendationSystem.Proxy;
using RouteRecommendationSystem.Adapter;
using RouteRecommendationSystem.Decorator;
using RouteRecommendationSystem.Observer;
using RouteRecommendationSystem.Facade;

namespace RouteRecommendationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                MostrarMenuPrincipal();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro inesperado: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Menu principal do aplicativo
        static void MostrarMenuPrincipal()
        {
            // Configuração do sistema de rota
            var settings = SettingsManager.GetInstance();
            var routePlanner = new RoutePlanner();
            var mapService = new ExternalMapAdapter(new LegacyMapProvider());
            var routeService = new CachedRouteProxy(new BaseRouteService(mapService));
            
            // Aplicando o padrão de Facade
            var appFacade = new AppFacade(routePlanner, routeService, settings);

            bool sair = false;
            
            // Loop principal do menu
            while (!sair)
            {
                Console.Clear();
                Console.WriteLine("===========================================");
                Console.WriteLine("   SISTEMA DE RECOMENDAÇÃO DE ROTAS");
                Console.WriteLine("===========================================");
                Console.WriteLine("1. Calcular Nova Rota");
                Console.WriteLine("2. Ver Histórico de Rotas");
                Console.WriteLine("3. Configurações do Sistema");
                Console.WriteLine("4. Sobre o Sistema");
                Console.WriteLine("5. Sair");
                Console.Write("\nEscolha uma opção: ");
                
                string opcao = Console.ReadLine();
                
                switch (opcao)
                {
                    case "1":
                        CalcularRota(appFacade);
                        break;
                    case "2":
                        appFacade.ShowRouteHistory();
                        EsperarTecla();
                        break;
                    case "3":
                        MostrarMenuConfiguracoes(settings);
                        break;
                    case "4":
                        MostrarSobre();
                        EsperarTecla();
                        break;
                    case "5":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
            
            Console.WriteLine("\nObrigado por utilizar o Sistema de Recomendação de Rotas!");
        }
        
        // Menu para calcular uma nova rota
        static void CalcularRota(AppFacade appFacade)
        {
            Console.Clear();
            Console.WriteLine("=== CÁLCULO DE NOVA ROTA ===\n");
            
            // Solicita origem
            Console.Write("Digite o local de origem: ");
            string origem = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(origem))
            {
                Console.WriteLine("Origem inválida!");
                EsperarTecla();
                return;
            }
            
            // Solicita destino
            Console.Write("Digite o local de destino: ");
            string destino = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(destino))
            {
                Console.WriteLine("Destino inválido!");
                EsperarTecla();
                return;
            }
            
            // Mostra as opções de transporte
            Console.WriteLine("\nOpções de Transporte:");
            Console.WriteLine("1. Carro");
            Console.WriteLine("2. Transporte Público");
            Console.WriteLine("3. Bicicleta");
            Console.WriteLine("4. A Pé");
            Console.Write("\nEscolha uma opção de transporte: ");
            
            string opcaoTransporte = Console.ReadLine();
            string tipoTransporte;
            
            // Converte a opção em tipo de transporte
            switch (opcaoTransporte)
            {
                case "1": tipoTransporte = "Carro"; break;
                case "2": tipoTransporte = "Transporte Público"; break;
                case "3": tipoTransporte = "Bicicleta"; break;
                case "4": tipoTransporte = "A Pé"; break;
                default:
                    Console.WriteLine("Opção inválida!");
                    EsperarTecla();
                    return;
            }
            
            // Mostra as opções de estratégia
            Console.WriteLine("\nEstratégias de Rota:");
            Console.WriteLine("1. Rota mais Rápida");
            Console.WriteLine("2. Rota mais Curta");
            Console.WriteLine("3. Rota mais Econômica");
            Console.WriteLine("4. Rota mais Ecológica");
            Console.Write("\nEscolha uma estratégia: ");
            
            string opcaoEstrategia = Console.ReadLine();
            string nomeEstrategia;
            
            // Converte a opção em tipo de estratégia
            switch (opcaoEstrategia)
            {
                case "1": nomeEstrategia = "FastestRoute"; break;
                case "2": nomeEstrategia = "ShortestRoute"; break;
                case "3": nomeEstrategia = "EconomicalRoute"; break;
                case "4": nomeEstrategia = "EcoFriendlyRoute"; break;
                default:
                    Console.WriteLine("Opção inválida!");
                    EsperarTecla();
                    return;
            }
            
            // Atualiza a estratégia nas configurações
            var settings = SettingsManager.GetInstance();
            settings.SetSetting("DefaultStrategy", nomeEstrategia);
            
            Console.WriteLine("\nCalculando sua rota, por favor aguarde...");
            Thread.Sleep(1500); // Simula um tempo de processamento
            
            try
            {
                // Usa a fachada para calcular e exibir a rota com todos os aprimoramentos
                appFacade.ShowRouteWithEnhancements(origem, destino, tipoTransporte);
                EsperarTecla();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao calcular rota: {ex.Message}");
                EsperarTecla();
            }
        }
        
        // Menu de configurações
        static void MostrarMenuConfiguracoes(SettingsManager settings)
        {
            bool voltar = false;
            
            while (!voltar)
            {
                Console.Clear();
                Console.WriteLine("=== CONFIGURAÇÕES DO SISTEMA ===\n");
                
                // Exibe as configurações atuais
                var configuracoesAtuais = settings.GetAllSettings();
                Console.WriteLine("Configurações Atuais:");
                foreach (var config in configuracoesAtuais)
                {
                    Console.WriteLine($"- {config.Key}: {config.Value}");
                }
                
                Console.WriteLine("\nOpções:");
                Console.WriteLine("1. Alterar Estratégia Padrão");
                Console.WriteLine("2. Alterar Transporte Padrão");
                Console.WriteLine("3. Ativar/Desativar Informações Turísticas");
                Console.WriteLine("4. Ativar/Desativar Alertas de Segurança");
                Console.WriteLine("5. Ativar/Desativar Cache de Rotas");
                Console.WriteLine("6. Voltar ao Menu Principal");
                
                Console.Write("\nEscolha uma opção: ");
                string opcao = Console.ReadLine();
                
                switch (opcao)
                {
                    case "1":
                        AlterarEstrategiaPadrao(settings);
                        break;
                    case "2":
                        AlterarTransportePadrao(settings);
                        break;
                    case "3":
                        bool usaTurismo = settings.GetSetting<bool>("UseTouristInfo");
                        settings.SetSetting("UseTouristInfo", !usaTurismo);
                        Console.WriteLine($"Informações Turísticas {(!usaTurismo ? "ativadas" : "desativadas")}.");
                        EsperarTecla();
                        break;
                    case "4":
                        bool usaAlertas = settings.GetSetting<bool>("ShowSafetyAlerts");
                        settings.SetSetting("ShowSafetyAlerts", !usaAlertas);
                        Console.WriteLine($"Alertas de Segurança {(!usaAlertas ? "ativados" : "desativados")}.");
                        EsperarTecla();
                        break;
                    case "5":
                        bool usaCache = settings.GetSetting<bool>("CacheEnabled");
                        settings.SetSetting("CacheEnabled", !usaCache);
                        Console.WriteLine($"Cache de Rotas {(!usaCache ? "ativado" : "desativado")}.");
                        EsperarTecla();
                        break;
                    case "6":
                        voltar = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        EsperarTecla();
                        break;
                }
            }
        }
        
        // Método para alterar a estratégia padrão
        static void AlterarEstrategiaPadrao(SettingsManager settings)
        {
            Console.WriteLine("\nEscolha a estratégia padrão:");
            Console.WriteLine("1. Rota mais Rápida (FastestRoute)");
            Console.WriteLine("2. Rota mais Curta (ShortestRoute)");
            Console.WriteLine("3. Rota mais Econômica (EconomicalRoute)");
            Console.WriteLine("4. Rota mais Ecológica (EcoFriendlyRoute)");
            
            Console.Write("\nEscolha uma opção: ");
            string opcao = Console.ReadLine();
            
            switch (opcao)
            {
                case "1":
                    settings.SetSetting("DefaultStrategy", "FastestRoute");
                    break;
                case "2":
                    settings.SetSetting("DefaultStrategy", "ShortestRoute");
                    break;
                case "3":
                    settings.SetSetting("DefaultStrategy", "EconomicalRoute");
                    break;
                case "4":
                    settings.SetSetting("DefaultStrategy", "EcoFriendlyRoute");
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    EsperarTecla();
                    return;
            }
            
            Console.WriteLine("Estratégia padrão alterada com sucesso!");
            EsperarTecla();
        }
        
        // Método para alterar o transporte padrão
        static void AlterarTransportePadrao(SettingsManager settings)
        {
            Console.WriteLine("\nEscolha o transporte padrão:");
            Console.WriteLine("1. Carro");
            Console.WriteLine("2. Transporte Público");
            Console.WriteLine("3. Bicicleta");
            Console.WriteLine("4. A Pé");
            
            Console.Write("\nEscolha uma opção: ");
            string opcao = Console.ReadLine();
            
            switch (opcao)
            {
                case "1":
                    settings.SetSetting("DefaultTransport", "Car");
                    break;
                case "2":
                    settings.SetSetting("DefaultTransport", "PublicTransport");
                    break;
                case "3":
                    settings.SetSetting("DefaultTransport", "Bike");
                    break;
                case "4":
                    settings.SetSetting("DefaultTransport", "Walking");
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    EsperarTecla();
                    return;
            }
            
            Console.WriteLine("Transporte padrão alterado com sucesso!");
            EsperarTecla();
        }
        
        // Método para mostrar informações sobre o sistema
        static void MostrarSobre()
        {
            Console.Clear();
            Console.WriteLine("=== SOBRE O SISTEMA ===\n");
            Console.WriteLine("Sistema de Recomendação de Rotas - ETAPA-2");
            Console.WriteLine("Versão: 1.0.0");
            Console.WriteLine("\nImplementação de múltiplos padrões de design:");
            Console.WriteLine("1. Strategy - Diferentes estratégias de cálculo de rota");
            Console.WriteLine("2. Factory Method - Criação de diferentes meios de transporte");
            Console.WriteLine("3. Singleton - Gerenciador de configurações globais");
            Console.WriteLine("4. Proxy - Cache de rotas para melhor performance");
            Console.WriteLine("5. Adapter - Integração com serviço de mapas legado");
            Console.WriteLine("6. Decorator - Aprimoramento de rotas com informações adicionais");
            Console.WriteLine("7. Observer - Monitoramento e notificações sobre rotas");
            Console.WriteLine("8. Facade - Simplificação da interface para cálculo de rotas");
            Console.WriteLine("\nDesenvolvido para a disciplina de POO-II");
            Console.WriteLine("UNIFESP - Universidade Federal de São Paulo");
        }
        
        // Método auxiliar para esperar por uma tecla
        static void EsperarTecla()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
