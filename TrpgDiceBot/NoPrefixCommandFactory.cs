using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            var noPrefixCommand = content.Split(new char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries)[0];

            if (noPrefixCommand.EndsWith('d'))
            {
                if (int.TryParse(noPrefixCommand[0..^1], out var rollCount) && rollCount > 0)
                {
                    return new DiceRollCommand(_message, rollCount);
                }
            }
            else if (noPrefixCommand.Contains('d'))
            {
                var splits = noPrefixCommand.Split('d');
                if (splits.Count() == 2 && splits.All(x => int.TryParse(x, out var _)))
                {
                    var rollCount = int.Parse(splits[0]);
                    var sidedCount = int.Parse(splits[1]);
                    if(rollCount > 0 && sidedCount > 0)
                    {
                        return new DiceRollCommand(_message, rollCount, sidedCount);
                    }
                }
            }

            return new InvalidCommand();
        }
    }
}
