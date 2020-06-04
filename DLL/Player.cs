using System;

namespace TypeLibrary
{
    public class Player
    {
        private readonly string name;
        private readonly string surname;
        private readonly string role;
        private readonly string nationality;
        public string team;

        public Player(string name, string surname, string role, string nationality)
        {
            this.name = name;
            this.surname = surname;
            this.role = role;
            this.nationality = nationality;

            team = "";
        }

        public void setTeam(string team)
        {
            this.team = team;
        }

        public string selector(string input)
        {
            switch (input.Trim())
            {
                case "name":
                    return name + " " + surname + Environment.NewLine;
                case "nationality":
                    return nationality + Environment.NewLine;
                case "role":
                    return role + Environment.NewLine;
                case "team":
                    if (team.Equals(""))
                        return "This player has no team" + Environment.NewLine;
                    else
                        return team + Environment.NewLine;
                case "help":
                    return "Use <nationality> to display the player's nationality." + Environment.NewLine +
                        "Use <role> to display the player's role on the field." + Environment.NewLine +
                        "Use <team> to display the player's current team." + Environment.NewLine +
                        "Use <help> to get a list of available commands" + Environment.NewLine;
                default:
                    return "No information found on query " + input + Environment.NewLine + "Use <help> to get a list of available commands" + Environment.NewLine;
            }
        }
    }
}