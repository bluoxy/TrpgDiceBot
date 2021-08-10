using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot.NoPrefixCommands
{
    public interface INoPrefixCommand
    {
        IMessage Message { get; }
        Task Execute();
    }
}
