using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeagueMatchBestOf
{
    private List<LeagueMatch> _matchList;

    private int _numberOfMatches, _currentMatch;

    private int[] _teamsScore;

    private int _winner;

    private bool _over;

    public LeagueMatchBestOf(int numberOfMatches, List<Team> teams)
    {
        _numberOfMatches = numberOfMatches; 
        _matchList = new List<LeagueMatch>();
        _teamsScore = new int[2];

        for(int x = 0; x < _numberOfMatches; x++)
        {
            _matchList.Add(new LeagueMatch(teams[0], teams[1]));
        }
    }
    public int[] ReturnTeamsScore()
    {
        return _teamsScore;
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
        /*
        Debug.Log("MatchList Size: " + _matchList.Count);
        Debug.Log("MatchList Size/2: " + _matchList.Count / 2);
        Debug.Log("WinnerScore for this Best of Series: " + ((_numberOfMatches - 1) / 2) + 1);*/

        var winnerScore = (_matchList.Count / 2) + 1;

        Debug.Log("WinnerScore is " + winnerScore + " | Team1Score: " + _teamsScore[0] + " | Team2Score: " + _teamsScore[1]);

        for(int x = 0; x < _teamsScore.Length; x++)
        {
            if(_teamsScore[x] >= winnerScore)
            {
                _winner = x;
                _over = true;
                break;
            }
        }
    }

    public LeagueMatch ReturnCurrentLeagueMatch()
    {
        return _matchList[_currentMatch];
    }

    private void AddPointsToTeam(int teamIndex)
    {
        _teamsScore[teamIndex]++;
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
