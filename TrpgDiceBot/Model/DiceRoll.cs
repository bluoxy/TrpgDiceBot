using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrpgDiceBot.Model
{
    public class DiceRoll
    {
        private readonly static Random _random = new Random();

        private readonly int[] results;

        public DiceRoll(string command)
        {
            Command = command;

            IsNaturalValue = int.TryParse(command, out var value);
            if (IsNaturalValue)
            {
                results = new int[]{ value };
                IsValid = true;
            }

            if (!TryParseRollCount(command, out var rollCount) ||
                !TryParseDiceCount(command, out var diceCount) ||
                !TryParseSidedCount(command, out var sidedCount))
            {
                return;
            }

            RollCount = rollCount;
            DiceCount = diceCount;
            SidedCount = sidedCount;
            results = Enumerable.Range(0, DiceCount)
                .Select(x => _random.Next(1, SidedCount + 1))
                .ToArray();
            IsValid = true;
        }

        public bool IsNaturalValue { get; } = false;
        public bool IsValid { get; } = false;
        public int DiceCount { get; set; } = 1;
        public int SidedCount { get; } = 6;
        public int RollCount { get; } = 1;
        public string Command { get; }
        public string RollResultMessage => $"{results.Sum()}";
        public string RollResultMessageWithDetails =>  $"{results.Sum()}[{string.Join(",", results)}]";
        public int Sum => results.Sum();

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
