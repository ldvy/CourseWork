using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using TypeLibrary;

class Server
{
    private readonly int port;
    private int numberConnected = 0;
    public Dictionary<string, Player> players;
    public Dictionary<string, Team> teams;
    public Dictionary<int, Game> games;
    public Dictionary<string, Season> seasons;

    public Server(int port)
    {
        players = new Dictionary<string, Player>();
        initPlayers();

        teams = new Dictionary<string, Team>();
        initTeams();

        games = new Dictionary<int, Game>();
        initGames();

        seasons = new Dictionary<string, Season>();
        initSeasons();

        this.port = port;
        Start();
    }

    private void Start()
    {
        TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        server.Start();

        Console.WriteLine("Server has started on " + server.LocalEndpoint + Environment.NewLine + "Waiting for a connection...", Environment.NewLine);

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();

            numberConnected += 1;

            Console.WriteLine("A client connected from " + client.Client.RemoteEndPoint + Environment.NewLine);
            Console.WriteLine("Connected clients " + numberConnected + Environment.NewLine);

            new TalkslyClient("temp" + numberConnected.ToString(), client.GetStream(), this);
        }
    }

    private void initPlayers()
    {
        players.Add("Cristiano Ronaldo", new Player("Cristiano", "Ronaldo", "Forward", "Portuguese"));
        players.Add("Robert Lewandowski", new Player("Robert", "Lewandowski", "Forward", "Polish"));

        players.Add("Lionel Messi", new Player("Lionel", "Messi", "Forward", "Spanish"));
        players.Add("Jan Oblak", new Player("Jan", "Oblak", "Goalkeeper", "Slovenian"));

        players.Add("Paul Pogba", new Player("Paul", "Pogba", "Midfielder", "French"));
        players.Add("Gabriel Barbosa", new Player("Gabriel", "Barbosa", "Forward", "Brazilian"));
    }

    private void initTeams()
    {
        teams.Add("Liverpool", new Team("Liverpool", "Liverpool, England"));
        teams["Liverpool"].addPlayer(players["Cristiano Ronaldo"]);
        teams["Liverpool"].addPlayer(players["Robert Lewandowski"]);

        teams["Liverpool"].addPlayer(players["Lionel Messi"]);

        teams["Liverpool"].removePlayer(players["Lionel Messi"]);
        teams["Liverpool"].removePlayer(players["Lionel Messi"]);

        teams.Add("Dynamo", new Team("Dynamo", "Kyiv, Ukraine"));
        teams["Dynamo"].addPlayer(players["Jan Oblak"]);
        teams["Dynamo"].addPlayer(players["Lionel Messi"]);

        teams.Add("Arsenal", new Team("Arsenal", "London, England"));
        teams["Arsenal"].addPlayer(players["Paul Pogba"]);
        teams["Arsenal"].addPlayer(players["Gabriel Barbosa"]);
    }

    private void initGames()
    {
        games.Add(1, new Game("3:5:1:2", "12/05/2020", teams["Liverpool"], teams["Dynamo"]));
        Console.WriteLine(games[1].selector("score") + games[1].selector("date"));

        games.Add(2, new Game("20/06/2020", teams["Liverpool"], teams["Arsenal"]));
        Console.WriteLine(games[2].selector("score"));

        games[2].finished("1:5:3:1");
        Console.WriteLine(games[2].selector("score") + games[2].selector("date"));

        games.Add(3, new Game("4:1:5:2", "12/06/2020", teams["Arsenal"], teams["Dynamo"]));
        Console.WriteLine(games[3].selector("score") + games[3].selector("date"));
    }

    private void initSeasons()
    {
        seasons.Add("ProLeague", new Season("ProLeague", "01/04/2020-01/07/2020"));
        seasons["ProLeague"].parseGame(games[1]);
        seasons["ProLeague"].parseGame(games[2]);
        seasons["ProLeague"].parseGame(games[3]);
    }
}