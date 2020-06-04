using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;
using TypeLibrary;
using System.Collections.Generic;

class TalkslyClient
{
    private readonly string tempName;
    private readonly NetworkStream stream;
    private string command;
    private readonly Server server;

    public TalkslyClient(string tempName, NetworkStream stream, Server server)
    {
        this.tempName = tempName;
        this.stream = stream;
        this.server = server;

        stream.Write(Encoding.ASCII.GetBytes(File.ReadAllText(@"C:\Users\herman.miahkov\Documents\Talksly\Talksly\Talksly\FAMER.txt")));
        selector("help");

        Thread clientThread = new Thread(Run);
        clientThread.Start();
    }

    private void Run()
    {
        using StreamReader streamReader = new StreamReader(stream);

        while (true)
        {
            try
            {
                command = streamReader.ReadLine().Trim();
                Console.WriteLine("Command received from user " + tempName + ": " + command + Environment.NewLine);
                selector(command);
            }
            catch (Exception ex) when (ex is NullReferenceException || ex is IOException)
            {
                Console.WriteLine("Client " + tempName + " disconnected" + Environment.NewLine);
                break;
            }
        }
    }

    private string list(string listInput)
    {
        string output = "";

        switch (listInput)
        {
            case "players":
                foreach (Player player in server.players.Values)
                    output += player.selector("name");
                break;
            case "teams":
                foreach (Team team in server.teams.Values)
                    output += team.selector("name");
                break;
            case "games":
                foreach (KeyValuePair<int, Game> game in server.games)
                    output += "Game " + game.Key + Environment.NewLine + game.Value.selector("date") + game.Value.selector("score");
                break;
            case "seasons":
                foreach (KeyValuePair<string, Season> season in server.seasons)
                    output += "Season " + season.Key + Environment.NewLine + season.Value.selector("duration");
                break;
            default:
                output = "No information found on query " + listInput + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine;
                break;
        }

        return output;
    }

    private void selector(string input)    //player Lionel Messi nationality | team Liverpool players | game 4 score
    {
        string[] command = input.Split(" ");

        try
        {
            switch (command[0])
            {
                case "player":
                    stream.Write(Encoding.ASCII.GetBytes(server.players[command[1] + " " + command[2]].selector(command[3]) + Environment.NewLine));
                    break;
                case "team":
                    stream.Write(Encoding.ASCII.GetBytes(server.teams[command[1]].selector(command[2]) + Environment.NewLine));
                    break;
                case "game":
                    stream.Write(Encoding.ASCII.GetBytes(server.games[int.Parse(command[1])].selector(command[2]) + Environment.NewLine));
                    break;
                case "season":
                    stream.Write(Encoding.ASCII.GetBytes(server.seasons[command[1]].selector(command[2]) + Environment.NewLine));
                    break;
                case "list":
                    stream.Write(Encoding.ASCII.GetBytes(list(command[1]) + Environment.NewLine));
                    break;
                case "help":
                    stream.Write(Encoding.ASCII.GetBytes("Use <player *Name Surname* *command*> to display the relevant information for the chosen player." + Environment.NewLine +
                        "Use <team *Name* *command*> to display the relevant information for the chosen team." + Environment.NewLine +
                        "Use <game *GameID* *command*> to display the relevant information for the chosen game." + Environment.NewLine +
                        "Use <season *Name* *command*> to display the relevant information for the chosen season." + Environment.NewLine +
                        "Use <list players/teams/games/seasons> to display the full list of players/teams/games/seasons" + Environment.NewLine +
                        "Use <help> to get a list of available commands" + Environment.NewLine + Environment.NewLine));
                    break;
                default:
                    stream.Write(Encoding.ASCII.GetBytes("No information found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine + Environment.NewLine));
                    break;
            }
        }
        catch (KeyNotFoundException)
        {
            stream.Write(Encoding.ASCII.GetBytes("No player/team/game/season found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine + Environment.NewLine));
        }
        catch (IndexOutOfRangeException)
        {
            stream.Write(Encoding.ASCII.GetBytes("Please enter the full name of the player " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine + Environment.NewLine));
        }
        catch (FormatException)
        {
            stream.Write(Encoding.ASCII.GetBytes("Please enter a valid player/team/game/season name" + Environment.NewLine + Environment.NewLine));
        }
    }
}