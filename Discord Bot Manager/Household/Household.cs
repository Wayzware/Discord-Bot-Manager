using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Discord;
using Discord.WebSocket;
using System.IO;

namespace Discord_Bot_Manager.Household
{
    internal class Household
    {
        private readonly string TOKEN = File.ReadAllText("household_token").Trim(); //TODO: Remove this from being hardcoded
        private const char CMD_INDICATOR = '!'; //all commands must start with this character

        private DiscordSocketClient _client;
        private readonly IConsoleSocket _consoleSocket;
        private readonly IStatusSocket _statusSocket;

        private Dictionary<ulong, Chore[]> _chores;
        private Dictionary<ulong, Broadcast[]> _broadcasts;

        public Household(IConsoleSocket console, IStatusSocket status = null)
        {
            _consoleSocket = console;
            _statusSocket = status;
        }

        public void Kill()
        {
            _client = null;
        }

        public async Task Stop()
        {
            await _client.LogoutAsync();
        }

        public async Task Start()
        {
            _consoleSocket.ClearAll();
            _consoleSocket.WriteLine("Starting up Household Bot...", 0);

            _client = new DiscordSocketClient();
            _client.MessageReceived += MessageReceivedAsync;

            try
            {
                await _client.LoginAsync(TokenType.Bot, TOKEN);
                await _client.StartAsync();
                if (_statusSocket != null) _statusSocket.IsActive = true;
                _consoleSocket.WriteLine("Household Bot is online!", 0);
            }
            catch (Exception e)
            {
                _consoleSocket.WriteLine(e.ToString(), 3);
                if(_statusSocket != null) _statusSocket.IsActive = false;
                _client = null;
            }
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Content[0] != CMD_INDICATOR || message.Author.IsBot) return;

            switch (message.Channel)
            {
                case IGuildChannel _:
                    await ProcessSeverCommand(message);
                    break;
                case IDMChannel _:
                    //TODO: implement DM commands
                    break;
                default:
                    await message.Channel.SendMessageAsync("I don't support this type of chat yet");
                    break;
            }
        }

        private async Task ProcessSeverCommand(SocketMessage message) //the big one
        {
            string command = message.Content.Remove(0, 1);
            command += " ";


            string[] commandArr = Wayzbot.CommandToArray(command, ' ');
            if (commandArr.Length == 0)
            {
                await BroadcastInvalidCommandMessage(message.Channel);
                return;
            }

            //each initial command should be called from this if statement
            if (commandArr[0] == "help")
            {
                await BroadcastHelpMessage(message.Channel);
            }
            else if (commandArr[0] == "chores" || commandArr[0] == "chore")
            {
                await ChoresHandler(commandArr.Length > 1 ? commandArr[1..] : new string[0], (IGuildChannel) message.Channel);
            }
            else
            {
                await BroadcastInvalidCommandMessage(message.Channel);
            }

        }

        private async Task LoadGuildChores(ulong guildId)
        {
            //TODO: find file in directory and load its data into the main dictionary
        }

        private async Task LoadAllGuildChores()
        {
            //TODO: for each file in the defined directory
        }

        private async Task SaveGuildChores(ulong guildId)
        {

        }

        private async Task SaveAllGuildChores()
        {
            foreach (ulong guildId in _chores.Keys) await SaveGuildChores(guildId);
        }

        private async Task BroadcastInvalidCommandMessage(ISocketMessageChannel channel)
        {
            await channel.SendMessageAsync("Invalid command. Type `!help` for a list of valid commands");
        }

        private async Task BroadcastHelpMessage(ISocketMessageChannel channel)
        {
            const string helpMessage =
                "**__Available server commands:__**\n" +
                "`!help` | shows you this\n" +
                "`!chores <args, see below>`:\n" +
                "       `delete <chore name>` | deletes a chore\n" +
                "       `done <chore name>` | do this when you complete a chore\n" +
                "       `due` | lists the due chores and who's turn it is\n" +
                "       `edit <chore name> <[-s <every x days>] [-p *<person to toggle>] [-c <current person>]>`\n" +
                "       `list` | lists all of the chores and who currently needs to do them\n" +
                "       `new <name> [-(s)chedule <every x days>] [-(p)eople *<person to toggle>] [-c <current person>]`\n" +
                "       `skip <chore name>` | skips over the current person" +
                "\n" +
                "\n" +
                "**__Available PM commands:__**\n" +
                "`!help` | shows this\n" +
                "`!broadcast <message>` | this will broadcast your message anonymously, at a random point in the next 24 hours, to the house discord server\n" +
                "`!broadcast -now <message>` | this will broadcast your message anonymously and immediately to the house discord server\n" +
                "`!dm <person> <message>` | this will send the person your message anonymously at a random point withing the next 24 hours\n" +
                "`!dm -now <person> <message>`\n" +
                "\n" +
                "(note: <> indicate required args and [] indicate optional args; * indicates this arg supports using * as a wildcard)";

            await channel.SendMessageAsync(helpMessage);
        }

        private async Task ChoresHandler(string[] args, IGuildChannel channel)
        {
            if(_chores == null) _chores = new Dictionary<ulong, Chore[]>();
            if(!_chores.ContainsKey(channel.GuildId)) _chores[channel.GuildId] = new Chore[0];


            if (args.Length == 0)
            {
                await BroadcastInvalidCommandMessage((ISocketMessageChannel) channel);
                return;
            }

            args[0] = args[0].ToLower(); //this should be done for all non-user-defined arguments (for all but things like names)

            if (args[0] == "delete" && args.Length > 1)
            {
                await DeleteChore(args[1], channel);
            }
            else if (args[0] == "done" && args.Length > 1)
            {
                await ChoreDone(args[1], channel);
            }
            else if (args[0] == "due")
            {
                await PrintChores(channel, true);
            }
            else if (args[0] == "edit")
            {
                await EditChore(args[1..], channel);
            }
            else if (args[0] == "list")
            {
                await PrintChores(channel, false);
            }
            else if (args[0] == "new")
            {
                await NewChore(args[1..], channel);
            }
            else if (args[0] == "skip")
            {
                await ChoreSkip(args[1], channel);
            }
            else
            {
                await BroadcastInvalidCommandMessage((ISocketMessageChannel) channel);
            }
        }

        private async Task ChoreSkip(string choreName, IGuildChannel channel)
        {
            bool found = false;
            foreach (var chore in _chores[channel.GuildId])
            {
                if (chore.name == choreName)
                {
                    chore.NextUser();
                    await SaveGuildChores(channel.GuildId);
                    found = true;
                    break;
                }
            }

            await ((ISocketMessageChannel)channel).SendMessageAsync(found
                ? "Successfully skipped person"
                : "Chore not found");
        }

        private async Task NewChore(string[] args, IGuildChannel channel)
        {
            if (args.Length < 1)
            {
                await BroadcastInvalidCommandMessage((ISocketMessageChannel)channel);
                return;
            }

            try
            {
                bool alreadyExists = false;
                foreach (Chore chore in _chores[channel.GuildId])
                {
                    if (chore.name == args[0])
                    {
                        alreadyExists = true;
                    }
                }

                if (!alreadyExists) //it doesnt already exist
                {
                    _chores[channel.GuildId] = _chores[channel.GuildId].Append(new Chore(args[0])).ToArray();
                    int index = _chores[channel.GuildId].Length - 1;
                    if (AlterChore(args[1..], ref _chores[channel.GuildId][index], channel.Guild))
                    {
                        //await SaveGuildChores(channel.GuildId);
                        await ((ISocketMessageChannel) channel).SendMessageAsync("Successfully added chore!");
                    }
                    else
                    {
                        await BroadcastInvalidCommandMessage((ISocketMessageChannel) channel);
                    }
                }
            }
            catch (Exception e)
            {
                await ((ISocketMessageChannel)channel).SendMessageAsync(e.ToString());
            }

        }

        private bool ChoreExists(string name, ulong guildID)
        {
            var chores = _chores[guildID];
            return chores.Any(chore => chore.name == name);
        }

        private async Task EditChore(string[] args, IGuildChannel channel)
        {
            if (args.Length < 2)
            {
                await BroadcastInvalidCommandMessage((ISocketMessageChannel) channel);
                return;
            }

            if (_chores[channel.GuildId].Any(chore => chore.name == args[0]))
            {
                int index = Array.IndexOf(_chores[channel.GuildId], _chores[channel.GuildId].Where(chore => chore.name == args[0]).ToArray()[0]);
                if (AlterChore(args[1..], ref _chores[channel.GuildId][index], channel.Guild))
                {
                    await SaveGuildChores(channel.GuildId);
                    await ((ISocketMessageChannel) channel).SendMessageAsync("Successfully edited chore!");
                }
                else
                {
                    await BroadcastInvalidCommandMessage((ISocketMessageChannel) channel);
                }
            }
        }

        private static bool AlterChore(string[] flags, ref Chore chore, IGuild guild)
        {
            //NOTE: this was written assuming the flags are all of length 2: -flag <arg1>.
            //it will have to be changed to support other lengths
            Chore initial = chore.Copy();

            for (int index = 0; index < flags.Length; index += 2)
            {
                if (!(flags.Length - 1 >= index + 1)) //this "flag" doesnt have at least 1 arg
                {
                    chore = initial;
                    return false; //there was a user syntax error
                }

                if (flags[index] == "-s")
                {
                    if (Int32.TryParse(flags[index + 1], out int x))
                    {
                        chore.Days = x;
                    }
                    else
                    {
                        chore = initial;
                        return false;
                    }
                }
                else if (flags[index] == "-p")
                {
                    if (flags[index + 1] == "*") //wildcard
                    {
                        guild.DownloadUsersAsync();
                        var users = guild.GetUsersAsync().Result.ToArray();
                        chore.ToggleUsers(users.Where(c => c.Nickname != null && !c.IsBot).ToArray());
                    }
                    else
                    {
                        guild.DownloadUsersAsync();
                        var users = guild.GetUsersAsync(CacheMode.AllowDownload).Result.ToArray();
                        chore.ToggleUsers(users.Where(c => c.Nickname != null && !c.IsBot && c.Nickname == flags[index + 1]).ToArray());
                    }
                }
                else if (flags[index] == "-c")
                {
                    var users = guild.GetUsersAsync().Result.ToArray();

                    if (users.Any(c => c.Nickname == flags[index + 1]))
                    {
                        chore.SetCurrentUser(users.Where(c => c.Nickname == flags[index + 1]).ToArray()[0]);
                    }
                    else
                    {
                        chore = initial;
                        return false;
                    }
                }
            }
            return true;
        }

        private async Task DeleteChore(string choreName, IGuildChannel channel)
        {
            Chore[] oldChores = _chores[channel.GuildId];
            Chore[] newChores;
            try
            {
                newChores = oldChores.Where(chore => chore.name != choreName).ToArray();
            }
            catch (Exception e)
            {
                newChores = new Chore[0];
            }

            if (oldChores == newChores)
            {
                await ((ISocketMessageChannel)channel).SendMessageAsync("Chore doesn't exist (or was already deleted)");
                return;
            }
            _chores[channel.GuildId] = newChores;
            await SaveGuildChores(channel.GuildId);
            await ((ISocketMessageChannel)channel).SendMessageAsync("Successfully deleted chore!");
        }

        private async Task ChoreDone(string choreName, IGuildChannel channel)
        {
            bool found = false;
            foreach (var chore in _chores[channel.GuildId])
            {
                if (chore.name == choreName)
                {
                    chore.Done();
                    await SaveGuildChores(channel.GuildId);
                    found = true;
                    break;
                }
            }

            await ((ISocketMessageChannel)channel).SendMessageAsync(found
                ? "Successfully recorded chore"
                : "Chore not found");
        }

        /// <summary>
        /// Prints chores that are due today
        /// </summary>
        /// <param name="channel">
        /// The channel to print on
        /// </param>
        /// <param name="printOnlyDues">
        /// If true, will print only due chores
        /// </param>
        /// <param name="printIfNone">
        /// If false, will not print anything if no chores are due (used upon start up)
        /// </param>
        /// <returns></returns>
        private async Task PrintChores(IGuildChannel channel, bool printOnlyDues, bool printIfNone = true)
        {
            Chore[] choresToPrint = _chores[channel.GuildId]; ;
            if (printOnlyDues)
            {
                choresToPrint = _chores[channel.GuildId].Where(chore => chore.Days != 0)
                    .Where(chore => chore.IsDue()).ToArray();
            }

            if (choresToPrint.Length == 0)
            {
                if (printIfNone)
                {
                    await ((ISocketMessageChannel) channel).SendMessageAsync("No chores are due");
                }
                return;
            }

            string msg = printOnlyDues ? "**The following chores are due:**" : "**Chores:**";

            foreach (Chore chore in choresToPrint)
            {
                msg += "\n" + chore.name;
                msg += chore.GetFormattedUsers();
            }

            await ((ISocketMessageChannel) channel).SendMessageAsync(msg);
        }

        internal async Task Say(string str, ISocketMessageChannel channel)
        {
            await channel.SendMessageAsync(str);
        }

    }
}
