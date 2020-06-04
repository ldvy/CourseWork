using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeLibrary
{
    public class Team
    {
        private readonly List<Player> players;
        private readonly string name;
        private readonly string hometown;

        private int gamesTotal;
        private int gamesWon;
        private int gamesDrawn;
        private int gamesLost;

        private int firstHalfGoals;
        private int secondHalfGoals;
        private int totalGoals;

        public Team(string name, string hometown)
        {
            players = new List<Player>();
            this.name = name;
            this.hometown = hometown;

            gamesTotal = 0;
            gamesWon = 0;
            gamesDrawn = 0;
            gamesLost = 0;

            firstHalfGoals = 0;
            secondHalfGoals = 0;
            totalGoals = 0;
        }

        public void addPlayer(Player playerRef)
        {
            if (playerRef.team.Equals(""))
            {
                players.Add(playerRef);
                playerRef.setTeam(name);

                Console.WriteLine(playerRef.selector("name") + " joined team " + name + Environment.NewLine);
            }
            else
                Console.WriteLine("Player " + name + " already is in team " + playerRef.team + Environment.NewLine);
        }

        public void removePlayer(Player playerRef)
        {
            if (!playerRef.team.Equals(""))
            {
                players.Remove(playerRef);
                playerRef.setTeam("");

                Console.WriteLine(playerRef.selector("name") + " has been removed from team " + name + Environment.NewLine);
            }
            else
                Console.WriteLine("Player " + playerRef.selector("name") + " is not in a team" + Environment.NewLine);

        }

        public void newGame(string gameResult, int[] goals)    // "win", [1, 25]
        {
            addGoals(goals);
            gamesTotal += 1;

            switch (gameResult)
            {
                case "win":
                    gamesWon += 1;
                    break;
                case "draw":
                    gamesDrawn += 1;
                    break;
                case "loss":
                    gamesLost += 1;
                    break;
            }
        }

        private void addGoals(int[] goals)
        {
            firstHalfGoals += goals[0];
            secondHalfGoals += goals[1];
            totalGoals += goals.Sum();
        }

        private string listPlayers()
        {
            string listPlayers = "";

            foreach (Player playerRef in players)
                listPlayers += playerRef.selector("name");

            return listPlayers;
        }

        public string selector(string input)
        {
            switch (input.Trim())
            {
                case "name":
                    return name + Environment.NewLine;
                case "players":
                    return listPlayers();
                case "hometown":
                    return hometown + Environment.NewLine;
                case "gamesTotal":
                    return gamesTotal.ToString() + Environment.NewLine;
                case "gamesWon":
                    return gamesWon.ToString() + Environment.NewLine;
                case "gamesDrawn":
                    return gamesDrawn.ToString() + Environment.NewLine;
                case "gamesLost":
                    return gamesLost.ToString() + Environment.NewLine;
                case "firstHalfGoals":
                    return firstHalfGoals.ToString() + Environment.NewLine;
                case "secondHalfGoals":
                    return secondHalfGoals.ToString() + Environment.NewLine;
                case "totalGoals":
                    return totalGoals.ToString() + Environment.NewLine;
                case "help":
                    return "Use <name> to display the team name" + Environment.NewLine +
                        "Use <players> to display all the players in the team" + Environment.NewLine +
                        "Use <hometown> to display the team's hometown" + Environment.NewLine +
                        "Use <gamesTotal> to display the number of games played" + Environment.NewLine +
                        "Use <gamesWon> to display the number of games won" + Environment.NewLine +
                        "Use <gamesDrawn> to display the number of games drawn" + Environment.NewLine +
                        "Use <gamesLost> to display the number of games lost" + Environment.NewLine +
                        "Use <firstHalfGoals> to display the total number of goals scored in First Half." + Environment.NewLine +
                        "Use <secondHalfGoals> to display the total number of goals scored in Second Half." + Environment.NewLine +
                        "Use <totalGoals> to display the total number of goals." + Environment.NewLine +
                        "Use <help> to get a list of available commands" + Environment.NewLine;
                default:
                    return "No information found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine;
            }
        }
    }
}