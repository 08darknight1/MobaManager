using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LeagueDay
    {
        private List<LeagueMatch> _matches;

        public LeagueDay()
        {
            _matches = new List<LeagueMatch>();
        }

        public void PlayMatches()
        {
            for(int x = 0; x < _matches.Count; x++)
            {
                _matches[x].PlayMatch();
            }
        }

        public void AddLeagueMatch(LeagueMatch newMatch)
        {
            _matches.Add(newMatch);
        }

        public List<LeagueMatch> ReturnMatchesList()
        {
            return _matches;
        }
    }
}