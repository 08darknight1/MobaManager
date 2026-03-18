using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayoffsManager : MonoBehaviour
{
    public int numberOfPlayoffsTeams;

    public bool playoffHasFinished = false;

    public GameObject playoffStandingPrefab;

    private List<Team> _playoffsTeamList;

    private List<LeagueMatchBestOf> _playoffsMatchesBestOf;

    private List<GameObject> _playoffsStandingObjects;

    private bool _hasSetTeams, _hasSetMatches;

    private UIOrganizer _UIOrganizer;

    private int _currentSeries;

    void Start()
    {
        _UIOrganizer = GameObject.Find("Canvas").GetComponent<UIOrganizer>();
        _playoffsStandingObjects = new List<GameObject>();
    }

    void Update()
    {
        if (_hasSetTeams && _hasSetMatches == false)
        {
            SetPlayoffsStage();

            CreateInitialUIforPlayoffs(false);

            DebugLogOnPlayoffBestOfMatches();

            _hasSetMatches = true;
        }
    }

    private void DebugLogOnPlayoffBestOfMatches()
    {
        Debug.Log("SIZE" + _playoffsMatchesBestOf.Count);

        for(int x = 0; x <  _playoffsMatchesBestOf.Count; x++)
        {
            var teams = _playoffsMatchesBestOf[x].ReturnTeamsPlaying();

            Debug.Log("X VALUE: " + x + " | MATCH: " + teams[0].ReturnTeamName() + " X " + teams[1].ReturnTeamName());
        }
    }

    public void SetTeams(List<Team> teamsToAdd)
    {
        _playoffsTeamList = new List<Team>();

        _playoffsMatchesBestOf = new List<LeagueMatchBestOf>();
        
        for(int x = 0; x < teamsToAdd.Count; x++)
        {
            _playoffsTeamList.Add(teamsToAdd[x]);
            Debug.Log("Added one team to playoffs...");
        }

        _hasSetTeams = true;
    }

    public void CreateInitialUIforPlayoffs(bool deletePreviousUI)
    {
        if (deletePreviousUI)
        {
            var listOfAllPreviousStandings = GameObject.FindGameObjectsWithTag("PlayoffStanding");

            for(int x = 0; x < listOfAllPreviousStandings.Length; x++)
            {
                Destroy(listOfAllPreviousStandings[x]);
            }

            _playoffsStandingObjects = new List<GameObject>();
        }

        var yValue = 100;

        var playoffsMenu = _UIOrganizer.leaguePlayoffsMenu;

        for(int x = 0; x < _playoffsMatchesBestOf.Count; x++)
        {
            var team1 = _playoffsMatchesBestOf[x].ReturnTeamsPlaying()[0];
            var team2 = _playoffsMatchesBestOf[x].ReturnTeamsPlaying()[1];

            var newStanding = Instantiate(playoffStandingPrefab, playoffsMenu.transform);

            _playoffsStandingObjects.Add(newStanding);

            var standingText = newStanding.GetComponent<TextMeshProUGUI>();

            standingText.text = team1.ReturnTeamName() + " OVR(" + team1.ReturnTeamOverrall()+")";
            standingText.text += "\n" + "X" + "\n";
            standingText.text += team2.ReturnTeamName() + " OVR(" + team2.ReturnTeamOverrall() + ")";

            newStanding.transform.localPosition = new Vector3(-320, yValue, 0);

            yValue -= 120;
        }
    }

    public void UpdateUIforPlayoffs()
    {
        Debug.Log("UI Objects Count: " + _playoffsStandingObjects.Count);

        Debug.Log("Matches BestOf Count: " + _playoffsMatchesBestOf.Count);

        for (int x = 0; x < _playoffsStandingObjects.Count; x++)
        {
            Debug.Log("Updating UI with current X value: " + x);

            var team1 = _playoffsMatchesBestOf[x].ReturnTeamsPlaying()[0];
            var team2 = _playoffsMatchesBestOf[x].ReturnTeamsPlaying()[1];

            var team1Score = _playoffsMatchesBestOf[x].ReturnTeamsScore()[0];
            var team2Score = _playoffsMatchesBestOf[x].ReturnTeamsScore()[1];

            var standingText = _playoffsStandingObjects[x].GetComponent<TextMeshProUGUI>();

            standingText.text = team1.ReturnTeamName() + " OVR(" + team1.ReturnTeamOverrall() + ")";
            standingText.text += "\n" + team1Score;
            standingText.text += "\n" + "X" + "\n";
            standingText.text += team2Score + "\n";
            standingText.text += team2.ReturnTeamName() + " OVR(" + team2.ReturnTeamOverrall() + ")";
        }
    }

    public void PlayNextSeries()
    {
        Debug.Log("Tentando jogar a próxima série...");
        _playoffsMatchesBestOf[_currentSeries].PlayNextMatch();
    }

    public LeagueMatch ReturnCurrentMatchInSeries()
    {
        Debug.Log("Current Series: " + _currentSeries);/*
        Debug.Log("Bagulho bugado: " + _playoffsMatchesBestOf[_currentSeries].ReturnCurrentLeagueMatch());*/ 
        return _playoffsMatchesBestOf[_currentSeries].ReturnCurrentLeagueMatch();
    }

    public bool CheckIfCurrentSeriesIsOver()
    {
        var _currentSeriesPlus = _currentSeries + 1;

        Debug.Log("Tamanho do BestOfMatches: " + _playoffsMatchesBestOf.Count + " |CurrentSeries + 1: " + _currentSeriesPlus);

        if (_playoffsMatchesBestOf[_currentSeries].ReturnBestOfWinner() != -1)
        {
            if (_currentSeriesPlus < _playoffsMatchesBestOf.Count)
            {
                _currentSeries++;
            }
            else
            {
                Debug.Log("Retornando como True pra criar as próximas matchups do Playoff...");

                return true;
            }
        }

        return false;
        //Debug.Log("CURRENT SERIES: " + _currentSeries);
    }

    private void SetPlayoffsStage()
    {
        //tem que na verdade primeiro verificar se o valor é par ou impar
        //Do jeito que ta agora, só funciona com os brackets que estão com aqueles números especificos

        var teamsAdded = 0;

        var team1and2 = new List<Team>();

        for (int x = 0; x < numberOfPlayoffsTeams; x++)
        {
            team1and2.Add(_playoffsTeamList[x]);
            teamsAdded++;

            if (teamsAdded >= 2)
            {
                _playoffsMatchesBestOf.Add(new LeagueMatchBestOf(5, team1and2));
                team1and2 = new List<Team>();
                teamsAdded = 0;
            }
        }
    }

    public void SetNextMatchups()
    {
        var teamsAdded = 0;

        var team1and2 = new List<Team>();

        var listToCopyFrom = new List<LeagueMatchBestOf>();

        for (int x = 0; x < _playoffsMatchesBestOf.Count; x++)
        {
            if (_playoffsMatchesBestOf[x].ReturnBestOfWinner() != -1)
            {
                var teamsPlaying = _playoffsMatchesBestOf[x].ReturnTeamsPlaying();
                team1and2.Add(teamsPlaying[_playoffsMatchesBestOf[x].ReturnBestOfWinner()]);
                teamsAdded++;
            }

            if (teamsAdded >= 2)
            {
                listToCopyFrom.Add(new LeagueMatchBestOf(5, team1and2));
                team1and2 = new List<Team>();
                teamsAdded = 0;
            }
        }

        Debug.Log("Teams Added Final Count: " + teamsAdded);

        if(teamsAdded == 1)
        {
            playoffHasFinished = true;
            _UIOrganizer.CreateNextMatchups();
        }
        else
        {
            _currentSeries = 0;

            _playoffsMatchesBestOf = new List<LeagueMatchBestOf>();

            for (int x = 0; x < listToCopyFrom.Count; x++)
            {
                _playoffsMatchesBestOf.Add(listToCopyFrom[x]);
            }
        }
    }
}
