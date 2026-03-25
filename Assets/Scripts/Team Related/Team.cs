using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Team
    {
        private string _teamName;

        private float _teamOverrall;
        
        private List<Player> _playerList = new List<Player>();

        public Team(string teamName)
        {
            _teamName = teamName;
        }

        public void RegisterNewPlayer(Player newPlayer)
        {
            _playerList.Add(newPlayer);
            SetNewTeamOverall();
        }

        public string ReturnTeamName()
        {
            return _teamName;
        }

        public List<Player> ReturnPlayerRoster()
        {
            return _playerList;
        }

        public void PrintTeam(bool printPlayers)
        {
            Debug.Log("|| --- TEAM: " + _teamName + " ||OVERRALL: " + _teamOverrall.ToString("n0") + " ||PLAYERS: ");

            if (printPlayers)
            {
                for (int x = 0; x < _playerList.Count; x++)
                {
                    _playerList[x].PrintPlayerStats(false);
                }
            }
        }

        private void SetNewTeamOverall()
        {
            if(_playerList.Count > 1)
            {
                float overSum = 0;

                for(int x = 0; x < _playerList.Count; x++)
                {
                    overSum += _playerList[x].ReturnPlayerOverrall();
                }

                _teamOverrall = overSum / _playerList.Count;
            }
        }

        public float ReturnTeamOverrall()
        {
            return _teamOverrall;
        }
    }
}