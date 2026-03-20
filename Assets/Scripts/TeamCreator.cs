using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamCreator : MonoBehaviour
{
    private PlayerCreator _entityAdministrator;

    private List<Team> _teamsList = new List<Team>();

    private List<string> _randomTeamNames = new List<string>();

    private int _setup = -1;

    void Start()
    {
        AddNamesToStringList();
        _entityAdministrator = GameObject.Find("LeagueAdministrator").GetComponent<PlayerCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_setup == -1)
        {
            AddNewTeams();
        }
        else if(_setup == 0)
        {
            if (_entityAdministrator.ReturnPlayersReadyForSeason())
            {
                AddPlayersToTeamsSetup();
            }
        }
        else if(_setup == 1)
        {
            //Debug.Log("PRONTO PARA A TEMPORADA");

            _teamsList = _teamsList.OrderByDescending(e => e.ReturnTeamOverrall()).ToList();

            for(int x = 0; x < _teamsList.Count; x++)
            {
                _teamsList[x].PrintTeam(false);
            }

            _setup++;
        }
    }

    public bool ReturnIfSetupIsComplete()
    {
        if(_setup >= 1)
        {
            return true;
        }

        return false;
    }

    public List<Team> ReturnTeamsSigned()
    {
        return _teamsList;
    }

    private void AddPlayersToTeamsSetup()
    {
        var toplaners = GetAllPlayersFromSpecificPos("TOP");
        var jnglaners = GetAllPlayersFromSpecificPos("JNG");
        var midlaners = GetAllPlayersFromSpecificPos("MID");
        var botlaners = GetAllPlayersFromSpecificPos("BOT");
        var suplaners = GetAllPlayersFromSpecificPos("SUP");

        for(int x = 0; x < _teamsList.Count; x++)
        {
            var newRoster = RegisterNewTeamRoster(toplaners, jnglaners, midlaners, botlaners, suplaners);

            for(int y = 0; y < newRoster.Count; y++)
            {
                _teamsList[x].RegisterNewPlayer(newRoster[y]);
            }
        }

        _setup++;
    }

    private List<Player> RegisterNewTeamRoster(List<Player> toplist, List<Player> jnglist, List<Player> midlist, List<Player> botlist , List<Player> suplist)
    {
        /*Debug.Log("TOP LIST SIZE: " + toplist.Count);
        Debug.Log("JNG LIST SIZE: " + jnglist.Count);
        Debug.Log("MID LIST SIZE: " + midlist.Count);
        Debug.Log("BOT LIST SIZE: " + botlist.Count);
        Debug.Log("SUP LIST SIZE: " + suplist.Count);*/

        var newTeam = new List<Player>();

        var indexSelected = Random.Range(0, toplist.Count - 1);

        for (int x = 0; x < 5; x++)
        {
            switch (x)
            {
                case 0:
                    newTeam.Add(toplist[indexSelected]);
                    toplist.Remove(toplist[indexSelected]);
                break;
                case 1:
                    indexSelected = Random.Range(0, jnglist.Count - 1);
                    newTeam.Add(jnglist[indexSelected]);
                    jnglist.Remove(jnglist[indexSelected]);
                break;
                case 2:
                    indexSelected = Random.Range(0, midlist.Count - 1);
                    newTeam.Add(midlist[indexSelected]);
                    midlist.Remove(midlist[indexSelected]);
                break;
                case 3:
                    indexSelected = Random.Range(0, botlist.Count - 1);
                    newTeam.Add(botlist[indexSelected]);
                    botlist.Remove(botlist[indexSelected]);
                break;
                case 4:
                    indexSelected = Random.Range(0, suplist.Count - 1);
                    newTeam.Add(suplist[indexSelected]);
                    suplist.Remove(suplist[indexSelected]);
                break;
            }
        }

        return newTeam;
    }

    private List<Player> GetAllPlayersFromSpecificPos(string role)
    {
        var specificLaners = new List<Player>();
        var allPlayers = _entityAdministrator.ReturnPlayersReadyList();
        for (int y = 0; y < allPlayers.Count; y++)
        {
            if (allPlayers[y].ReturnPlayerRole() == role)
            {
                specificLaners.Add(allPlayers[y]);
            }
        }

        specificLaners = specificLaners.OrderByDescending(e => e.ReturnPlayerOverrall()).ToList();

        return specificLaners;
    }

    private void AddNewTeams()
    {
        var teamDebugLogCont = 0;

        for (int x = 0; x < 10; x++)
        {
            var teamName = _randomTeamNames[Random.Range(0, _randomTeamNames.Count)];

            if (_teamsList.Count >= 1)
            {
                while (true)
                {
                    var breakIt = false;

                    for (int y = 0; y < _teamsList.Count; y++)
                    {
                        if (teamName == _teamsList[y].ReturnTeamName())
                        {
                            breakIt = false;
                            break;
                        }
                        else
                        {
                            breakIt = true;
                        }
                    }

                    if (!breakIt)
                    {
                        teamName = _randomTeamNames[Random.Range(0, _randomTeamNames.Count)];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _teamsList.Add(new Team(teamName));

            //Debug.Log("NEW TEAM REGISTERED: " + _teamsList[teamDebugLogCont].ReturnTeamName());
            teamDebugLogCont++;
        }

        _setup++;
    }

    private void AddNamesToStringList()
    {
        _randomTeamNames.Add("Astronautas");
        _randomTeamNames.Add("Tigers");
        _randomTeamNames.Add("Falcons");
        _randomTeamNames.Add("Bons D'Bola");
        _randomTeamNames.Add("Batatinhas");

        _randomTeamNames.Add("Raptors");
        _randomTeamNames.Add("Juriscreido");
        _randomTeamNames.Add("Nothinghummingun");
        _randomTeamNames.Add("Black Castle");
        _randomTeamNames.Add("The Square Table");

        /////-10

        _randomTeamNames.Add("Round Chairs");
        _randomTeamNames.Add("Goblets");
        _randomTeamNames.Add("Bowling Squad");
        _randomTeamNames.Add("Ribby Boys");
        _randomTeamNames.Add("The Runners");

        _randomTeamNames.Add("Sky Nomads");
        _randomTeamNames.Add("Wild Mages");
        _randomTeamNames.Add("Zealous Warriors");
        _randomTeamNames.Add("Feras da Quadra");
        _randomTeamNames.Add("Doom Robots");

        /////-20
        
        _randomTeamNames.Add("Fogueirense");
        _randomTeamNames.Add("Amigos do Yuri");
        _randomTeamNames.Add("Ouro Verde");
        _randomTeamNames.Add("Açougueiros da XV");
        _randomTeamNames.Add("São Juracicaba");

        _randomTeamNames.Add("Red5");
        _randomTeamNames.Add("Thresher Maw");
        _randomTeamNames.Add("Ghost of Zera");
        _randomTeamNames.Add("Fist Fury");
        _randomTeamNames.Add("A P 3 X");

        /////-30
    }
}
