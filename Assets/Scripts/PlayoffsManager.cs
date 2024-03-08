using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum NumberOfPlayoffsTeams {n2, n3, n4, n5, n6};

public class PlayoffsManager : MonoBehaviour
{
    public NumberOfPlayoffsTeams numberOfPlayoffsTeams;

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

            /*
            var team1And4 = new List<Team>();
            team1And4.Add(_playoffsTeamList[0]);
            team1And4.Add(_playoffsTeamList[3]);
            _playoffsMatchesBestOf.Add(new LeagueMatchBestOf(5, team1And4));

            var team2And3 = new List<Team>();
            team2And3.Add(_playoffsTeamList[1]);
            team2And3.Add(_playoffsTeamList[2]);
            _playoffsMatchesBestOf.Add(new LeagueMatchBestOf(5, team2And3));

            var team5And6 = new List<Team>();
            team5And6.Add(_playoffsTeamList[4]);
            team5And6.Add(_playoffsTeamList[5]);
            _playoffsMatchesBestOf.Add(new LeagueMatchBestOf(5, team5And6));*/

            CreateInitialUIforPlayoffs();

            _hasSetMatches = true;

            DebugLogOnPlayoffBestOfMatches();
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
        }

        _hasSetTeams = true;
    }

    private void CreateInitialUIforPlayoffs()
    {
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
        for (int x = 0; x < _playoffsStandingObjects.Count; x++)
        {
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
        _playoffsMatchesBestOf[_currentSeries].PlayNextMatch();
    }

    public LeagueMatch ReturnCurrentMatchInSeries()
    {
        Debug.Log("Current Series: " + _currentSeries);
        Debug.Log("Bagulho bugado: " + _playoffsMatchesBestOf[_currentSeries].ReturnCurrentLeagueMatch());
        return _playoffsMatchesBestOf[_currentSeries].ReturnCurrentLeagueMatch();
    }

    public void CheckIfCurrentSeriesIsOver()
    {
        if (_playoffsMatchesBestOf[_currentSeries].ReturnBestOfWinner() != -1)
        {
            _currentSeries++;
        }

        Debug.Log("CURRENT SERIES: " + _currentSeries);
    }

    private void SetPlayoffsStage()
    {
        switch (numberOfPlayoffsTeams)
        {
            case NumberOfPlayoffsTeams.n2:
                var team1and2 = new List<Team>();
                team1and2.Add(_playoffsTeamList[0]);
                team1and2.Add(_playoffsTeamList[1]);
                _playoffsMatchesBestOf.Add(new LeagueMatchBestOf(5, team1and2));
            break;
        }
    }

    private void SetNextBestOfMatches()
    {

    }
}
