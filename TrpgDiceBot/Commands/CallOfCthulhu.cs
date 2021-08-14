using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot.Commands
{
    public class CallOfCthulhu : ModuleBase
    {
        private readonly static Random _random = new Random();

        /// <summary>
        /// ヘルプを表示します
        /// </summary>
        /// <returns></returns>
        [Command("ccb")]
        public async Task Battle()
        {
            var rollResult = _random.Next(1, 101);
            await ReplyAsync($"(1D100) → {rollResult}");
        }
        /// <summary>
        /// ヘルプを表示します
        /// </summary>
        /// <returns></returns>
        [Command("ccb")]
        public async Task Battle(int skillValue)
        {
            var rollResult = _random.Next(1, 101);
            var skillResult = "失敗";
            if (rollResult <= skillValue)
            {
                if (rollResult <= 5　&& rollResult <= skillValue / 5)
                {
                    skillResult = "決定的成功/スペシャル";
                }
                else if (rollResult <= skillValue / 5)
                {
                    skillResult = "スペシャル";
                }
                else
                {
                    skillResult = "成功";
                }
            }
            else if(rollResult > 95)
            {
                skillResult = "致命的失敗";
            }

            await ReplyAsync($"(1D100<={skillValue}) → {rollResult} → {skillResult}");
        }
    }
}
