using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Entity
    {
        public enum playerRole { TOP, JNG, MID, BOT, SUP };


        private string _nickName;

        private int _playerLevel;

        private float _playerOverrall;

        private List<Stat> _playerGameStats = new List<Stat>();

        private playerRole _playerRole;

        public Player(string name, int age, string nickName, int playerRole) : base(name, age)
        {
            _nickName = nickName; 
            AddPlayerStatsToList();
            GenerateRandomStats();
            switch(playerRole)
            {
                case 0:
                    _playerRole = Player.playerRole.TOP;
                break;
                case 1:
                    _playerRole = Player.playerRole.JNG;
                break;
                case 2:
                    _playerRole = Player.playerRole.MID;
                break;
                case 3:
                    _playerRole = Player.playerRole.BOT;
                break;
                case 4:
                    _playerRole = Player.playerRole.SUP;
                break;
            }
        }

        private void AddPlayerStatsToList()
        {
            _playerGameStats.Add(new Stat("Skill", 100));
            _playerGameStats.Add(new Stat("TeamWork", 100));
            _playerGameStats.Add(new Stat("Adaptability", 100));
            _playerGameStats.Add(new Stat("Intelligence", 100));
            _playerGameStats.Add(new Stat("Off-rolling", 100));
            _playerLevel = (Random.Range(1, 6));
        }

        public void GenerateRandomStats()
        {
            var valueSum = 0;

            for (int x = 0; x < _playerGameStats.Count; x++)
            {
                var maxSkillValue = _playerGameStats[x].ReturnStatMaxValue() + 1;

                switch (_playerLevel)
                {
                    case 1:
                        _playerGameStats[x].SetStatCurrentValue(Random.Range(10, maxSkillValue - 75));
                    break;
                    case 2:
                        _playerGameStats[x].SetStatCurrentValue(Random.Range(maxSkillValue - 75, maxSkillValue - 60));
                    break;
                    case 3:
                        _playerGameStats[x].SetStatCurrentValue(Random.Range(maxSkillValue - 60, maxSkillValue - 40));
                    break;
                    case 4:
                        _playerGameStats[x].SetStatCurrentValue(Random.Range(maxSkillValue - 40, maxSkillValue - 20));
                    break;
                    case 5:
                        _playerGameStats[x].SetStatCurrentValue(Random.Range(maxSkillValue - 20, maxSkillValue));
                    break;
                }

                valueSum += _playerGameStats[x].ReturnStatCurrentValue();
            }

            _playerOverrall = valueSum / _playerGameStats.Count;
        }

        public void PrintPlayerStats(bool printStats)
        {
            if (printStats)
            {
                Debug.Log(ReturnEntityName() + "`" + _nickName + "` ||ROLE: " + _playerRole.ToString() + " ||Overrall: " + _playerOverrall + " ||LVL: " + _playerLevel + " || STATS: ");
                for (int x = 0; x < _playerGameStats.Count; x++)
                {
                    Debug.Log(_playerGameStats[x].ReturnStatName() + ": " + _playerGameStats[x].ReturnStatCurrentValue());
                }
            }
            else
            {
                Debug.Log(ReturnEntityName() + "`" + _nickName + "` ||ROLE: " + _playerRole.ToString() + " ||Overrall: " + _playerOverrall + " ||LVL: " + _playerLevel);
            }
        }

        public void SetNewNickName(string valueToSet)
        {
            _nickName = valueToSet; 
        }

        public string ReturnPlayerNickName()
        {
            return _nickName;
        }

        public string ReturnPlayerRole()
        {
            return _playerRole.ToString();
        }

        public float ReturnPlayerOverrall()
        {
            return _playerOverrall;
        }
    }
}