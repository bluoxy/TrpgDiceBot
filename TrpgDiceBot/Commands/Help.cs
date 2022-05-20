using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot
{
    public class Help : ModuleBase
    {
        /// <summary>
        /// ヘルプを表示します
        /// </summary>
        /// <returns></returns>
        [Command("help")]
        public async Task Display()
        {
            var helps = new []
            {
                "XdY … Y面ダイスをX個振ります",
                "1d6 … 6面ダイスを1個振ります",
                "ZrXdY … Z回Y面ダイスをX個振ります",
                "3r1d6 … 3回6面ダイスを1個振ります",
                "1d6+2d6 … 1d6と2d6を足した合計を表示します",
                "1d6-2d6 … 1d6と2d6を引いた合計を表示します",
                "!ping … pong"
            };
            await ReplyAsync(string.Join(Environment.NewLine, helps));
        }
    }
}
