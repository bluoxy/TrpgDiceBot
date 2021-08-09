using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot
{
    public class DiceRollCommand : INoPrefixCommand
    {
        private readonly static Random _random = new Random();
        public int RollCount { get; } = 1;
        public int SidedCount { get; } = 6;

        public IMessage Message { get; }

        public DiceRollCommand(IMessage message)
        {
            Message = message;
        }

        public DiceRollCommand(IMessage message, int rollCount, int sidedCount = 6)
        {
            Message = message;
            RollCount = rollCount;
            SidedCount = sidedCount;
        }

        public DiceRollCommand(IMessage message, string content)
        {
            Message = message;
            if (content.EndsWith('d'))
            {
                if (int.TryParse(content.Substring(0, content.Length - 1), out var rollCount))
                {
                    RollCount = rollCount;
                }
            }
            else if (content.Contains('d'))
            {
                var splits = content.Split('d');
                if (splits.Count() == 2 && splits.All(x => int.TryParse(x, out var _)))
                {
                    RollCount = int.Parse(splits[0]);
                    SidedCount = int.Parse(splits[1]);
                }
            }
        }

        private IEnumerable<int> DiceRoll()
        {
            return Enumerable.Range(0, RollCount).Select(x => _random.Next(1, SidedCount));
        }

        private string CreateDiceRollResultMessage()
        {
            try
            {
                var results = DiceRoll();
                var resultMessage = RollCount == 1 ?
                    $"({RollCount}d{SidedCount}) → {results.Sum()}" :
                    $"({RollCount}d{SidedCount}) → {results.Sum()}[{string.Join(",", results)}] → {results.Sum()}";
                if(resultMessage.Length >= 2000)
                {
                    return "ロール回数を減らしてください";
                }

                return resultMessage;
            }
            catch(OverflowException)
            {
                return "ロール回数を減らしてください";
            }
        }

        public async Task Execute()
        {
            await Message.Channel.SendMessageAsync(CreateDiceRollResultMessage());
        }
    }
}
