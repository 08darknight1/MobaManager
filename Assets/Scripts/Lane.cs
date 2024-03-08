using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Lane
    {
        private Player _player1, _player2;

        private bool _laneOver;

        private float _laneAdvantage;

        private int _laneWinner;

        private string _laneName;

        public Lane(Player player1, Player player2, string laneName)
        {
            _laneName = laneName;
            _player1 = player1;
            _player2 = player2;
        }

        public void SetUpPlayersPercentage()
        {
            var p1Overrall = _player1.ReturnPlayerOverrall();
            var p2Overrall = _player2.ReturnPlayerOverrall();

            Debug.Log("LANE: " + _laneName);
            Debug.Log("P1OVERRALL: " + p1Overrall);
            Debug.Log("P2OVERRALL: " + p2Overrall);

            p2Overrall = p2Overrall * -1;

            _laneAdvantage = p1Overrall + p2Overrall;
        }

        public void CalculateLaneWinner()
        {
            var laneWinnerResult = Random.Range(0, 101);
            var p1Percent = 0f;
            var p2Percent = 0f;

            Debug.Log("LANE ADVANTAGE: " + _laneAdvantage);

            p1Percent = 50 + _laneAdvantage; 
            _laneAdvantage = _laneAdvantage * -1; 
            p2Percent = 50 + _laneAdvantage; 

            Debug.Log("DETERMINANDO O WINNER");
            Debug.Log("P1PERCENT: " + p1Percent);
            Debug.Log("P2PERCENT: " + p2Percent);
            Debug.Log("WINNING NUMBER: " + laneWinnerResult);

            if(p1Percent > p2Percent)
            {
                if(laneWinnerResult > 0 && laneWinnerResult <= p1Percent)
                {
                    _laneWinner = 0;
                    Debug.Log("P1 WON!");
                }
                else if(laneWinnerResult > p1Percent && laneWinnerResult <= 100)
                {
                    _laneWinner = 1;
                    Debug.Log("P2 WON!");
                }
            }
            else
            {
                if (laneWinnerResult > 0 && laneWinnerResult <= p2Percent)
                {
                    _laneWinner = 1;
                    Debug.Log("P2 WON!");
                }
                else if (laneWinnerResult > p2Percent && laneWinnerResult <= 100)
                {
                    _laneWinner = 0;
                    Debug.Log("P1 WON!");
                }
            }

            _laneOver = true;
        }

        public bool ReturnIfLaneOver()
        {
            return _laneOver;   
        }

        public Player ReturnLaneWinner()
        {
            if(!_laneOver )
            {
                return null;
            }

            if(_laneWinner == 1)
            {
                return _player2;
            }

            return _player1;
        }

        public Player ReturnLaneLoser()
        {
            if (!_laneOver)
            {
                return null;
            }

            if (_laneWinner == 0)
            {
                return _player2;
            }

            return _player1;
        }

        public string ReturnLaneName()
        {
            return _laneName; 
        }
    }
}