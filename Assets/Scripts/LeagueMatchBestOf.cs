using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeagueMatchBestOf
{
    private List<LeagueMatch> _matchList;

    private int _numberOfMatches, _currentMatch;

    private int _team1Score, _team2Score;

    private int _winner;

    private bool _over;

    public LeagueMatchBestOf(int numberOfMatches, List<Team> teams)
    {
        _numberOfMatches = numberOfMatches; 
        _matchList = new List<LeagueMatch>();

        for(int x = 0; x < _numberOfMatches; x++)
        {
            _matchList.Add(new LeagueMatch(teams[0], teams[1]));
        }
    }

    public List<int> ReturnTeamsScore()
    {
        var newList = new List<int>();

        newList.Add(_team1Score);
        newList.Add(_team2Score);

        return newList;
    }

    public List<Team> ReturnTeamsPlaying()
    {
        return _matchList[0].ReturnMatchTeams();
    }

    public void PlayNextMatch()
    {
        _matchList[_currentMatch].PlayMatch();
        var matchWinner = _matchList[_currentMatch].ReturnWinner();
        AddPointsToTeam(matchWinner);
        _currentMatch++;
        CheckForSeriesWinner();

    }

    private void CheckForSeriesWinner()
    {
        var winnerScore = ((_numberOfMatches - 1)/2) + 1;

        if (_team1Score >= winnerScore || _team2Score >= winnerScore)
        {
            _over = true;
        }
    }

    public LeagueMatch ReturnCurrentLeagueMatch()
    {
        return _matchList[_currentMatch];
    }

    private void AddPointsToTeam(int teamIndex)
    {
        if(teamIndex == 0)
        {
            _team1Score++;
        }
        else
        {
            _team2Score++;
        }
    }

    public int ReturnBestOfWinner()
    {
        if(!_over)
        {
            return -1;
        }

        return _winner;
    }
}
