using Discord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrpgDiceBot.Model;

namespace TrpgDiceBot.NoPrefixCommands
{
    public class DiceRollCommand : INoPrefixCommand
    {
        private string resultMessage;

        public IMessage Message { get; }

        public DiceRollCommand(IMessage message)
        {
            Message = message;
        }

        public DiceRollCommand(IMessage message, string content)
        {
            Message = message;
            InitializeDiceRolls(content);

        }

        public void InitializeDiceRolls(string content)
        {
            var noPrefixCommand = content.Split(new char[] { ' ', '　' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var diceRolls = noPrefixCommand
                .Split(new char[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new DiceRoll(x));
            if (!diceRolls.All(x => x.IsValid) || !diceRolls.Any() || (diceRolls.Count() == 1 && diceRolls.Single().IsNaturalValue))
            {
                return;
            }

            var rollResultsMessage = new string(noPrefixCommand);
            var formula = new string(noPrefixCommand);
            var hiddenDetails = diceRolls.Count() == 1 && diceRolls.Single().DiceCount == 1;
            foreach (var diceRoll in diceRolls)
            {
                var rollResultMessage = hiddenDetails || diceRoll.IsNaturalValue ? 
                    diceRoll.RollResultMessage :
                    diceRoll.RollResultMessageWithDetails;
                rollResultsMessage = rollResultsMessage.Replace(diceRoll.Command, rollResultMessage);
                formula = formula.Replace(diceRoll.Command, diceRoll.Sum.ToString());
            }

            //式を計算する
            int sum = Convert.ToInt32(formula.Calc<double>());
            if (diceRolls.Count() == 1 && diceRolls.Single().DiceCount == 1)
            {
                resultMessage = $"({noPrefixCommand}) → {sum}";
            }
            else
            {
                resultMessage = $"({noPrefixCommand}) → {rollResultsMessage} → {sum}";
            }
        }

        private string CreateDiceRollsResultMessage()
        {
            try
            {
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
            if (string.IsNullOrEmpty(resultMessage))
            {
                return;
            }

            await Message.Channel.SendMessageAsync(CreateDiceRollsResultMessage());
        }
    }
}
