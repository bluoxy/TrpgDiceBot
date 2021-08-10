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
        public int DiceCount { get; } = 1;
        public int SidedCount { get; } = 6;
        public int RollCount { get; } = 1;

        public IMessage Message { get; }

        public DiceRollCommand(IMessage message)
        {
            Message = message;
        }

        public DiceRollCommand(IMessage message, int diceCount, int sidedCount = 6, int rollCount = 1)
        {
            Message = message;
            DiceCount = diceCount;
            SidedCount = sidedCount;
            RollCount = rollCount;
        }

        private IEnumerable<int> DiceRoll()
        {
            return Enumerable.Range(0, DiceCount).Select(x => _random.Next(1, SidedCount));
        }

        private string CreateDiceRollResultMessage()
        {
            var results = DiceRoll().ToList();
            return DiceCount == 1 ?
                $"({DiceCount}d{SidedCount}) → {results.Sum()}" :
                $"({DiceCount}d{SidedCount}) → {results.Sum()}[{string.Join(",", results)}] → {results.Sum()}";

        }
        private string CreateDiceRollsResultMessage()
        {
            try
            {
                var resultMessage = RollCount == 1 ?
                    CreateDiceRollResultMessage() :
                    string.Join(Environment.NewLine, Enumerable.Range(1, RollCount).Select(x => $"{x}{CreateDiceRollResultMessage()}"));
                if (resultMessage.Length >= 2000)
                {
                    return "ロール回数を減らしてください";
                }

                return resultMessage;
            }
            catch (OverflowException)
            {
                return "ロール回数を減らしてください";
            }
        }

        public async Task Execute()
        {
            await Message.Channel.SendMessageAsync(CreateDiceRollsResultMessage());
        }
    }
}
