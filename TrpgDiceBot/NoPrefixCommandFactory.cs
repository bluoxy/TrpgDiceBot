using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrpgDiceBot.NoPrefixCommands;

namespace TrpgDiceBot
{
    public class NoPrefixCommandFactory
    {
        private readonly IMessage _message;

        public NoPrefixCommandFactory(IMessage message)
        {
            _message = message;
        }

        public INoPrefixCommand Create(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new InvalidCommand();
            }

            return new DiceRollCommand(_message, content);
        }
    }
}
