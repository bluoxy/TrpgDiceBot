using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrpgDiceBot
{
    /// <summary>
    ///     PingPongを実行するクラス
    /// </summary>
    public class PingPong : ModuleBase
    {
        /// <summary>
        ///     pingの発言があった場合、pongを返します
        /// </summary>
        /// <returns></returns>
        [Command("ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
    }
}
