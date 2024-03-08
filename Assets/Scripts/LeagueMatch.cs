using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LeagueMatch
    {
        private Team _team1, _team2;

        private bool _over;

        private int _winner;

        private int _pointsTeam1, _pointsTeam2;

        private List<Lane> _lanesList = new List<Lane>();

        private int objectives;

        private int bonusMultiplyer = 10;

        private UIOrganizer _uiOrganizer;

        public LeagueMatch(Team team1, Team team2)
        {
            _team1 = team1;
            _team2 = team2;
        }

        public void PlayMatch()
        {
            _uiOrganizer = GameObject.Find("Canvas").GetComponent<UIOrganizer>();
            SetUpLane();
            ExecuteLanes();
            ExecuteObjectives();
        }

        public int ReturnWinner()
        {
            if (!_over)
            {
                return -1;
            }

            return _winner;
        }

        public List<Team> ReturnMatchTeams()
        {
            var newTeamsList = new List<Team>();

            newTeamsList.Add(_team1);
            newTeamsList.Add(_team2);

            return newTeamsList;
        }

        public List<int> ReturnTeamsPoints()
        {
            var pointList = new List<int>();

            pointList.Add(_pointsTeam1);
            pointList.Add(_pointsTeam2);

            return pointList;
        }

        public void PrintPlayingTeams()
        {
            Debug.Log(_team1.ReturnTeamName() + " X " + _team2.ReturnTeamName());
        }

        private void SetUpLane()
        {
            var t1Players = _team1.ReturnPlayerRoster();
            var t2Players = _team2.ReturnPlayerRoster();
            var laneName = "";

            for (int x = 0; x < 5; x++)
            {
                switch (x)
                {
                    case 0:
                        laneName = "TOP";
                    break;
                    case 1:
                        laneName = "JNG";
                    break;
                    case 2:
                        laneName = "MID";
                    break;
                    case 3:
                        laneName = "BOT";
                    break;
                    case 4:
                        laneName = "SUP";
                    break;
                }

                _lanesList.Add(new Lane(t1Players[x], t2Players[x], laneName));
            }
        }

        private void ExecuteLanes()
        {
            var t1Roster = _team1.ReturnPlayerRoster();
            var t2Roster = _team2.ReturnPlayerRoster();

            for (int x = 0; x < _lanesList.Count; x++)
            {
                _lanesList[x].SetUpPlayersPercentage();

                _lanesList[x].CalculateLaneWinner();

                var laneWinner = _lanesList[x].ReturnLaneWinner();

                var laneWinnerName = laneWinner.ReturnPlayerNickName();
                    
                var team1Name = _team1.ReturnTeamName();

                var team2Name = _team2.ReturnTeamName();

                var laneLoser = _lanesList[x].ReturnLaneLoser();

                var laneLoserName = laneLoser.ReturnPlayerNickName();

                var laneName = _lanesList[x].ReturnLaneName();

                if (t1Roster[x] == laneWinner)
                {
                    
                    _uiOrganizer.AddNewMessageToMatchLog(laneWinnerName + " from " + team1Name + " won against " + team2Name + "'s " + laneLoserName + ", on the " + laneName + "lane!");
                    _pointsTeam1++;
                    //Debug.Log("Adding points to T1");
                }
                else if (t2Roster[x] == laneWinner)
                {
                    _uiOrganizer.AddNewMessageToMatchLog(laneWinnerName + " from " + team2Name + " won against " + team1Name + "'s " + laneLoserName + ", on the " + laneName + "lane!");
                    _pointsTeam2++;
                    //Debug.Log("Adding points to T2");
                }

                _uiOrganizer.UpdateTeamsScore(_pointsTeam1, _pointsTeam2);

                _uiOrganizer.AddNewMessageToMatchLog(team1Name + " | " + _pointsTeam1 + " X " + _pointsTeam2 + " | " + team2Name);

                //Debug.Log("(" + x + ") - - - SCORE: T1 -> " + _pointsTeam1 + " || " + _pointsTeam2 + " <- T2");
            }
        }

        private void ExecuteObjectives()
        {
            for(int x = 0; x < 8; x++)
            {
                if (_over)
                {
                    break;
                }

                if (x > 3)
                {
                    bonusMultiplyer--;
                }

                if (x <= 6)
                {
                    if (objectives % 2 == 0)
                    {
                        ExecuteDragon();
                    }
                    else
                    {
                        ExecuteBaron();
                    }
                }
                else if(x >= 7)
                {
                    ExecuteAncient();
                }

                CheckForEndGame(x);
            }
        }

        private void CheckForEndGame(int x)
        {
            Debug.Log("(" + x + ") - - - SCORE: T1 -> " + _pointsTeam1 + " || " + _pointsTeam2 + " <- T2");

            if (_pointsTeam1 >= 7 || _pointsTeam2 >= 7)
            {
                if(_pointsTeam2 > _pointsTeam1)
                {
                    _winner = 1;
                    var t2Name = _team2.ReturnTeamName();
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " are the winners!!!");
                }
                else
                {
                    var t1Name = _team1.ReturnTeamName();
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " are the winners!!!");
                }

                _over = true;

                Debug.Log("MATCH OVER!!!!");

                _uiOrganizer.FinishMatch();
            }
        }

        private void ExecuteDragon()
        {
            //Debug.Log("DECIDING DRAGON OBJECTIVE...");

            _uiOrganizer.AddNewMessageToMatchLog("The teams are heading for the dragon...");

            var team1Sum = 0f;
            var team2Sum = 0f;

            for (int x = 0; x < _lanesList.Count; x++)
            {
                var laneName = _lanesList[x].ReturnLaneName();
                var laneWinner = _lanesList[x].ReturnLaneWinner();

                var t1Roster = _team1.ReturnPlayerRoster();
                var t2Roster = _team2.ReturnPlayerRoster();

                var t1ValueToAdd = t1Roster[x].ReturnPlayerOverrall();
                var t2ValueToAdd = t2Roster[x].ReturnPlayerOverrall();

                switch (laneName)
                {
                    case "TOP":
                        t1ValueToAdd = (t1ValueToAdd * 50) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 50) / 100;
                    break;
                    case "MID":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                    break;
                    case "BOT":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                    break;
                    case "SUP":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                    break;
                }

                if (t1Roster[x] == laneWinner)
                {
                    t1ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t1ValueToAdd -= bonusMultiplyer;
                }

                if (t2Roster[x] == laneWinner)
                {
                    t2ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t2ValueToAdd -= bonusMultiplyer;
                }

                team1Sum += t1ValueToAdd;
                team2Sum += t2ValueToAdd;
            }

            team1Sum = team1Sum / 5;
            team2Sum = team2Sum / 5;

            team2Sum = team2Sum * -1;

            var objAdvantage = team1Sum + team2Sum;

            objAdvantage = (float)Math.Round(objAdvantage);

            var t1Percent = 50 + objAdvantage;
            objAdvantage = objAdvantage * -1;
            var t2Percent = 50 + objAdvantage;

            var objWinnerResult = UnityEngine.Random.Range(1, 101);

            Debug.Log("T1%: " + t1Percent + " T2%: " + t2Percent + " ||Result: " + objWinnerResult);

            var t1Name = _team1.ReturnTeamName();
            var t2Name = _team2.ReturnTeamName();

            if (t1Percent > t2Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    Debug.Log("T1 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the dragon safely!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    Debug.Log("T2 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " somehow managed to get the dragon!!!");
                }
            }
            else if(t2Percent > t1Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t2Percent)
                {
                    _pointsTeam2++;
                    //Debug.Log("T2 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the dragon safely!");
                }
                else if (objWinnerResult > t2Percent && objWinnerResult <= 100)
                {
                    _pointsTeam1++;
                    //Debug.Log("T1 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " somehow managed to get the dragon!!!");
                }
            }
            else
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    Debug.Log("T1 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the dragon safely!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    Debug.Log("T2 WON!");
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the dragon safely!");
                }
            }

            objectives++;

            _uiOrganizer.UpdateTeamsScore(_pointsTeam1, _pointsTeam2);

            _uiOrganizer.AddNewMessageToMatchLog(t1Name + " | " + _pointsTeam1 + " X " + _pointsTeam2 + " | " + t2Name);
        }

        private void ExecuteBaron()
        {
            //Debug.Log("DECIDING BARON OBJECTIVE...");

            _uiOrganizer.AddNewMessageToMatchLog("The teams are heading for the baron...");

            var team1Sum = 0f;
            var team2Sum = 0f;

            for (int x = 0; x < _lanesList.Count; x++)
            {
                var laneName = _lanesList[x].ReturnLaneName();
                var laneWinner = _lanesList[x].ReturnLaneWinner();

                var t1Roster = _team1.ReturnPlayerRoster();
                var t2Roster = _team2.ReturnPlayerRoster();

                var t1ValueToAdd = t1Roster[x].ReturnPlayerOverrall();
                var t2ValueToAdd = t2Roster[x].ReturnPlayerOverrall();

                switch (laneName)
                {
                    case "TOP":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                    case "MID":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                    case "BOT":
                        t1ValueToAdd = (t1ValueToAdd * 50) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 50) / 100;
                        break;
                    case "SUP":
                        t1ValueToAdd = (t1ValueToAdd * 50) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 50) / 100;
                        break;
                }

                if (t1Roster[x] == laneWinner)
                {
                    t1ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t1ValueToAdd -= bonusMultiplyer;
                }

                if (t2Roster[x] == laneWinner)
                {
                    t2ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t2ValueToAdd -= bonusMultiplyer;
                }

                team1Sum += t1ValueToAdd;
                team2Sum += t2ValueToAdd;
            }

            team1Sum = team1Sum / 5;
            team2Sum = team2Sum / 5;

            team2Sum = team2Sum * -1;

            var objAdvantage = team1Sum + team2Sum;

            objAdvantage = (float)Math.Round(objAdvantage);

            var t1Percent = 50 + objAdvantage;
            objAdvantage = objAdvantage * -1;
            var t2Percent = 50 + objAdvantage;

            var objWinnerResult = UnityEngine.Random.Range(1, 101);

            var t1Name = _team1.ReturnTeamName();
            var t2Name = _team2.ReturnTeamName();

            if (t1Percent > t2Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the baron safely!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " somehow managed to get the baron!!!");
                }
            }
            else if(t2Percent > t1Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t2Percent)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the baron safely!");
                }
                else if (objWinnerResult > t2Percent && objWinnerResult <= 100)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " somehow managed to get the baron!!!");
                }
            }
            else
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the baron safely!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the baron safely!");
                }
            }

            objectives++;

            _uiOrganizer.UpdateTeamsScore(_pointsTeam1, _pointsTeam2);

            _uiOrganizer.AddNewMessageToMatchLog(t1Name + " | " + _pointsTeam1 + " X " + _pointsTeam2 + " | " + t2Name);
        }

        private void ExecuteAncient()
        {
            Debug.Log("DECIDING ANCIENT OBJETIVE...");

            _uiOrganizer.AddNewMessageToMatchLog("And it all comes down to the Ancient, both teams are going for a final fight!!!");

            var team1Sum = 0f;
            var team2Sum = 0f;

            for (int x = 0; x < _lanesList.Count; x++)
            {
                var laneName = _lanesList[x].ReturnLaneName();
                var laneWinner = _lanesList[x].ReturnLaneWinner();

                var t1Roster = _team1.ReturnPlayerRoster();
                var t2Roster = _team2.ReturnPlayerRoster();

                var t1ValueToAdd = t1Roster[x].ReturnPlayerOverrall();
                var t2ValueToAdd = t2Roster[x].ReturnPlayerOverrall();

                switch (laneName)
                {
                    case "TOP":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                    case "MID":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                    case "BOT":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                    case "SUP":
                        t1ValueToAdd = (t1ValueToAdd * 75) / 100;
                        t2ValueToAdd = (t2ValueToAdd * 75) / 100;
                        break;
                }

                if (t1Roster[x] == laneWinner)
                {
                    t1ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t1ValueToAdd -= bonusMultiplyer;
                }

                if (t2Roster[x] == laneWinner)
                {
                    t2ValueToAdd += bonusMultiplyer;
                }
                else
                {
                    t2ValueToAdd -= bonusMultiplyer;
                }

                team1Sum += t1ValueToAdd;
                team2Sum += t2ValueToAdd;
            }

            team1Sum = team1Sum / 5;
            team2Sum = team2Sum / 5;

            team2Sum = team2Sum * -1;

            var objAdvantage = team1Sum + team2Sum;

            objAdvantage = (float)Math.Round(objAdvantage);

            var t1Percent = 50 + objAdvantage;
            objAdvantage = objAdvantage * -1;
            var t2Percent = 50 + objAdvantage;

            var objWinnerResult = UnityEngine.Random.Range(1, 101);

            var t1Name = _team1.ReturnTeamName();
            var t2Name = _team2.ReturnTeamName();

            if (t1Percent > t2Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the ANCIENT for a final push to win the game!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " somehow got the ANCIENT for a final push to win the game!!!");
                }
            }
            else if(t2Percent > t1Percent)
            {
                if (objWinnerResult > 0 && objWinnerResult <= t2Percent)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the ANCIENT for a final push to win the game!");
                }
                else if (objWinnerResult > t2Percent && objWinnerResult <= 100)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " somehow got the ANCIENT for a final push to win the game!!!");
                }
            }
            else
            {
                if (objWinnerResult > 0 && objWinnerResult <= t1Percent)
                {
                    _pointsTeam1++;
                    _uiOrganizer.AddNewMessageToMatchLog(t1Name + " managed to secure the ANCIENT for a final push to win the game!");
                }
                else if (objWinnerResult > t1Percent && objWinnerResult <= 100)
                {
                    _pointsTeam2++;
                    _uiOrganizer.AddNewMessageToMatchLog(t2Name + " managed to secure the ANCIENT for a final push to win the game!");
                }
            }

            objectives++;

            _uiOrganizer.UpdateTeamsScore(_pointsTeam1, _pointsTeam2);
        }
    }
}

/*

P%1 = 60
    -
P%2 = 40

*Se positivo, vantagem para o P1, se negativo vantagem para o P2

Só fazer a diferença dos valores:

Ex: P1 60 - P2 40
Ex: 20
Ex: %Final= p1 <- 60/40 -> p2

Ex: P1 80 - P2 40
Ex: 40
Ex: %Final= p1 <- 90/10 -> p2

Ex: P1 90 - P2 20
Ex: 70
Ex: %Final= p1 <- 100/000 -> p2 *Valores maiores do que 100, serão capados*

Estágios:

(1)-LanePhase
- Seta todas a probabilidades de rota;
- Roda pra ver quem ganhou;
- Adiciona pontos para o devido time baseado nisso;

(2)-Obj1/Dragao
- Laners que ganharam sua lane, ganham 10 pts bonus para seu overrall;
- Laners que perderam sua lane, ganham -10 pts bonus para seu overrall;
- Disputa do objetivo é feita pela seguinte formula:
    OV - Overrall
    Team1% = (100% JNG.Ov + Bonus) + 
             (75% MID.Ov + Bonus ) +
             (75% BOT.Ov + Bonus ) +
             (75% SUP.Ov + Bonus ) +
             (50% TOP.Ov + Bonus ) / 5

    -

    Team2% = (100% JNG.Ov + Bonus) + 
             (75% MID.Ov + Bonus ) +
             (75% BOT.Ov + Bonus ) +
             (75% SUP.Ov + Bonus ) +
             (50% TOP.Ov + Bonus ) / 5

 *Se positivo, vantagem para o Time1, se negativo vantagem para o Time2
 *Após definido isso, faz se o cálculo da % para ver qual time ganhou;
 *O time que ganhou ganha um ponto;

(3)-Obj2/Barao
- Laners que ganharam sua lane, ganham 10 pts bonus para seu overrall;
- Laners que perderam sua lane, ganham -10 pts bonus para seu overrall;
- Disputa do objetivo é feita pela seguinte formula:
    OV - Overrall
    Team1% = (100% JNG.Ov + Bonus) + 
             (75% MID.Ov + Bonus ) +
             (75% TOP.Ov + Bonus ) +
             (50% BOT.Ov + Bonus ) +
             (50% SUP.Ov + Bonus ) / 5

    -

    Team2% = (100% JNG.Ov + Bonus) + 
             (75% MID.Ov + Bonus ) +
             (75% TOP.Ov + Bonus ) +
             (50% BOT.Ov + Bonus ) +
             (50% SUP.Ov + Bonus ) / 5

 *Se positivo, vantagem para o Time1, se negativo vantagem para o Time2;
 *Após definido isso, faz se o cálculo da % para ver qual time ganhou;
 *O time que ganhou ganha um ponto;

(4)-Contagem de Pontos
- Depois de 7 pontos iniciais distribuídos
- Ve se algum dos times atingiu essa margem
- Se nenhum dos times ainda não conseguiu os pontos, os objetivos vão se repetindo na seguinte ordem:
    OBJ3(Dragão) -> OBJ4(Barão) -> OBJ5(Dragão) -> OBJ6(Barão) -> OBJ7(Dragão) -> OBJ8(Ancião)
    *A partir do terceiro objetivo, os bonus dados aos jogadores diminuem em 1;
    *Nesse ultimo (OBJ6), sempre resultando em um vencedor;

 * */