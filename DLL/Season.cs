using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeLibrary
{
    public class Season
    {
        private readonly string name;
        private readonly string duration;

        private readonly Dictionary<string, int> teamsWinRating;
        private readonly Dictionary<string, int> teamsGoalRating;
        private readonly List<Game> games;
        public Season(string name, string duration)
        {
            games = new List<Game>();
            teamsWinRating = new Dictionary<string, int>();
            teamsGoalRating = new Dictionary<string, int>();

            this.name = name;
            this.duration = duration;
        }

        public void parseGame(Game game)
        {
            games.Add(game);

            //win count
            if (teamsWinRating.ContainsKey(game.winner))
                teamsWinRating[game.winner] += 1;
            else
                teamsWinRating.Add(game.winner, 1);

            //goal count
            if (teamsGoalRating.ContainsKey(game.team1.selector("name")))
                teamsGoalRating[game.team1.selector("name")] += game.team1Goals.Sum();
            else
                teamsGoalRating.Add(game.team1.selector("name"), game.team1Goals.Sum());

            if (teamsGoalRating.ContainsKey(game.team2.selector("name")))
                teamsGoalRating[game.team2.selector("name")] += game.team2Goals.Sum();
            else
                teamsGoalRating.Add(game.team2.selector("name"), game.team2Goals.Sum());
        }

        private string sortedWinRating()
        {
            string output = "";

            foreach (KeyValuePair<string, int> team in teamsWinRating.OrderBy(key => key.Value))
            {
                output += team.Key + " " + team.Value + Environment.NewLine;
            }

            return output;
        }

        private string sortedGoalRating()
        {
            string output = "";

            foreach (KeyValuePair<string, int> team in teamsGoalRating.OrderBy(key => key.Value))
            {
                output += team.Key + " " + team.Value + Environment.NewLine;
            }

            return output;
        }

        public string selector(string input)
        {
            switch (input.Trim())
            {
                case "duration":
                    return duration + Environment.NewLine;
                case "name":
                    return name + Environment.NewLine;
                case "goalRating":
                    return sortedGoalRating();
                case "winRating":
                    return sortedWinRating();
                case "help":
                    return "Use <duration> to display the duration of the season" + Environment.NewLine +
                        "Use <name> to display the name of the season" + Environment.NewLine +
                        "Use <winRating> to display the teams rated by win count" + Environment.NewLine +
                        "Use <goalRating> to display the teams rated by goal count" + Environment.NewLine +
                        "Use <help> to get a list of available commands" + Environment.NewLine;
                default:
                    return "No information found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine;
            }
        }
    }
}