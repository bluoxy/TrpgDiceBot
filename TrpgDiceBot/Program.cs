using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TrpgDiceBot
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private static readonly IConfigurationRoot _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json")
                .Build();
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public Program()
        {
            _client = new DiscordSocketClient();
            _client.Log += msg => { Console.WriteLine(msg.ToString()); return Task.CompletedTask; };
            _client.Ready += () => { Console.WriteLine($"{_client.CurrentUser} is Running!!"); return Task.CompletedTask; };
            _client.MessageReceived += OnMessage;
            _commandService = new CommandService();
            _serviceProvider = new ServiceCollection().BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
            await _client.LoginAsync(TokenType.Bot, _configuration["token"]);
            await _client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }


        private async Task OnMessage(SocketMessage message)
        {
            if (!(message is SocketUserMessage userMessage))
            {
                return;
            }

            Console.WriteLine($"{userMessage.Channel.Name} {userMessage.Author.Username}:{userMessage}");
            if (userMessage.Author.IsBot)
            {
                return;
            }

            int argPos = 0;
            if (!(userMessage.HasCharPrefix('!', ref argPos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                var factory = new NoPrefixCommandFactory(message);
                var noPrefixCommand = factory.Create(message.Content);
                await noPrefixCommand.Execute();
                return;
            }

            var context = new CommandContext(_client, userMessage);
            var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
