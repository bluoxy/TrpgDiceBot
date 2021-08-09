using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot
{
    public class InvalidCommand : INoPrefixCommand
    {
        public IMessage Message => null;

        public async Task Execute() => await Task.CompletedTask;
    }
}
