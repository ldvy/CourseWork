using System;
using System.Linq;

namespace TypeLibrary
{
    public class Game
    {
        readonly string date;
        public readonly int[] team1Goals;
        public readonly int[] team2Goals;
        public readonly Team team1;
        public readonly Team team2;
        public string winner;

        private Boolean wasPlayed;

        public Game(string date, Team team1, Team team2)                  //announcement
        {
            this.date = date;
            this.team1 = team1;
            this.team2 = team2;

            team1Goals = new int[2];
            team2Goals = new int[2];

            wasPlayed = false;
        }

        public Game(string goals, string date, Team team1, Team team2)             // "3:5:1:2", "2019-01-12", team1, team2
        {
            this.date = date;
            this.team1 = team1;
            this.team2 = team2;

            team1Goals = new int[2];
            team2Goals = new int[2];

            wasPlayed = true;

            parseGoals(goals);
            teamInfoFill();
        }

        public void finished(string goals)
        {
            wasPlayed = true;
            parseGoals(goals);
            teamInfoFill();
        }

        private void parseGoals(string goals)
        {
            string[] parseGoals = goals.Split(':');

            team1Goals[0] = int.Parse(parseGoals[0]);
            team1Goals[1] = int.Parse(parseGoals[1]);
            team2Goals[0] = int.Parse(parseGoals[2]);
            team2Goals[1] = int.Parse(parseGoals[3]);
        }

        private void teamInfoFill()
        {
            int team1Sum = team1Goals.Sum();
            int team2Sum = team2Goals.Sum();

            if (team1Sum > team2Sum)
            {
                winner = team1.selector("name").TrimEnd(Environment.NewLine.ToCharArray());
                team1.newGame("win", team1Goals);
                team2.newGame("loss", team2Goals);
            }
            else if (team1Sum < team2Sum)
            {
                winner = team2.selector("name").TrimEnd(Environment.NewLine.ToCharArray());
                team1.newGame("loss", team1Goals);
                team2.newGame("win", team2Goals);
            }
            else if (team1Sum == team2Sum)
            {
                team1.newGame("draw", team1Goals);
                team2.newGame("draw", team2Goals);
            }
        }

        public string selector(string input)
        {
            switch (input.Trim())
            {
                case "date":
                    return date + Environment.NewLine;
                case "score":
                    if (wasPlayed)
                        return team1Goals.Sum().ToString() + " " + team1.selector("name") +
                            team2Goals.Sum().ToString() + " " + team2.selector("name");
                    else
                        return "This game hasn't been played yet." + Environment.NewLine + "It is announced on " + date + Environment.NewLine;
                case "teams":
                    return team1.selector("name") + team2.selector("name") + Environment.NewLine;
                case "help":
                    return "Use <date> to display the date of the game" + Environment.NewLine +
                        "Use <score> to display the score" + Environment.NewLine +
                        "Use <teams> to display the team names" + Environment.NewLine +
                        "Use <help> to get a list of available commands" + Environment.NewLine;
                default:
                    return "No information found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine;
            }
        }
    }
}