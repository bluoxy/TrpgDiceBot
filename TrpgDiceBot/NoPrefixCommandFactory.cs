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

            if (noPrefixCommand[1..].Count(x => x == 'd') == 1)
            {
                if (!TryParseRollCount(noPrefixCommand, out var rollCount) ||
                    !TryParseDiceCount(noPrefixCommand, out var diceCount) ||
                    !TryParseSidedCount(noPrefixCommand, out var sidedCount))
                {
                    return new InvalidCommand();
                }

                return new DiceRollCommand(_message, diceCount, sidedCount, rollCount);
            }

            return new InvalidCommand();
        }

        private static bool TryParseRollCount(string noPrefixCommand, out int rollCount)
        {
            rollCount = default;

            if (noPrefixCommand[1..].Count(x => x == 'r') == 0)
            {
                rollCount = 1;
                return true;
            }

            if (noPrefixCommand[1..].Count(x => x == 'r') == 1 &&
                int.TryParse(noPrefixCommand.Split('r').First(), out var validArgument))
            {
                rollCount = validArgument;
                return true;
            }

            return false;
        }

        private static bool TryParseDiceCount(string noPrefixCommand, out int diceCount)
        {
            diceCount = default;

            if (noPrefixCommand[1..].Count(x => x == 'd') != 1)
            {
                return false;
            }

            if (noPrefixCommand[1..].Count(x => x == 'r') == 1)
            {
                if (int.TryParse(noPrefixCommand.Split(new[] { 'r', 'd' }).Skip(1).First(), out var validArgument))
                {
                    diceCount = validArgument;
                    return true;
                }

                return false;
            }
            else
            {
                if (int.TryParse(noPrefixCommand.Split('d').First(), out var validArgument))
                {
                    diceCount = validArgument;
                    return true;
                }

                return false;
            }
        }

        private static bool TryParseSidedCount(string noPrefixCommand, out int sidedCount)
        {
            sidedCount = default;

            if (noPrefixCommand[1..].Count(x => x == 'd') != 1)
            {
                return false;
            }

            if (noPrefixCommand.EndsWith('d'))
            {
                sidedCount = 6;
                return true;
            }

            if (int.TryParse(noPrefixCommand.Split('d').Last(), out var validArgument))
            {
                sidedCount = validArgument;
                return true;
            }

            return false;
        }

    }
}
