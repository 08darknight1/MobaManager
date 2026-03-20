using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LeagueOrganizer : MonoBehaviour
    {
        public int LeagueWeeksNumber, MatchDaysNumberInWeek;

        public bool HasPlayoffs;

        private TeamCreator _teamCreator;

        private List<LeagueWeek> _weekList;

        private List<LeagueMatch> _matchHistory;

        private bool createWeeks;

        private int matchesRoundCont;

        void Start()
        {
            _teamCreator = GameObject.Find("LeagueAdministrator").GetComponent<TeamCreator>();
            _weekList = new List<LeagueWeek>();
            _matchHistory = new List<LeagueMatch>();
        }

        void Update()
        {
            if (!createWeeks && _teamCreator.ReturnIfSetupIsComplete())
            {
                for (int x = 0; x < LeagueWeeksNumber; x++)
                {
                    _weekList.Add(new LeagueWeek(MatchDaysNumberInWeek));
                }

                createWeeks = true;

                SetupWeeksDaysMatches();
            }
        }

        public List<LeagueMatch> ReturnMatchHistory()
        {
            return _matchHistory;
        }

        public List<LeagueMatch> ReturnMatchFromSpecificDayAndWeek(int week, int day)
        {
            var leagueMatchList = _weekList[week].ReturnWeeksLeagueDay();
            return leagueMatchList[day].ReturnMatchesList();
        }

        private void SetupWeeksDaysMatches()
        {
            for (int x = 0; x < _weekList.Count; x++)
            {
                var weekDayList = _weekList[x].ReturnWeeksLeagueDay();

                for(int y = 0; y < weekDayList.Count; y++)
                {
                    SetAndAddMatchToList(_matchHistory, weekDayList[y].ReturnMatchesList());
                }
            }
        }

        public void AddPlayOffsMatch(LeagueMatch leagueMatch)
        {
            _matchHistory.Add(leagueMatch);
        }

        private void SetAndAddMatchToList(List<LeagueMatch> matchHistory, List<LeagueMatch> dayMatches)
        {
            var teamsList = _teamCreator.ReturnTeamsSigned();

            var teamsListCopy = new List<Team>();

            for(int x = 0; x < teamsList.Count; x++)
            {
                teamsListCopy.Add(teamsList[x]);
            }

            var team1 = teamsListCopy[0];

            teamsListCopy.RemoveAt(0);

            var newMatch = new LeagueMatch(team1, teamsListCopy[matchesRoundCont]);
            dayMatches.Add(newMatch);
            matchHistory.Add(newMatch);

            var extraMatches = 0;

            var forSize = teamsListCopy.Count / 2;

            //Debug.Log("FORSIZE: " + forSize);

            for (int x = 0; x < forSize; x++)
            {
                var nextValue = AddToIndex(teamsListCopy, matchesRoundCont);
                var prevValue = SubtractFromIndex(teamsListCopy, matchesRoundCont);

                var contOp = 0;
                var contOp2 = 0;

                if (contOp < extraMatches)
                {
                    while (contOp < extraMatches)
                    {
                        nextValue = AddToIndex(teamsListCopy, nextValue);
                        contOp++;
                    }
                }

                if (contOp2 < extraMatches)
                {
                    while (contOp2 < extraMatches)
                    {
                        prevValue = SubtractFromIndex(teamsListCopy, prevValue);
                        contOp2++;
                    }
                }

                //Debug.Log("TEAM LIST COPY SIZE: " + teamsListCopy.Count);
                //Debug.Log("NEXT VALUE: " + nextValue + " ||PREV VALUE: " + prevValue);
                //Debug.Log("TEAM NEXT VALUE: " + teamsListCopy[nextValue].ReturnTeamName() + " ||TEAM PREV VALUE: " + teamsListCopy[prevValue].ReturnTeamName());

                var newMatch2 = new LeagueMatch(teamsListCopy[nextValue], teamsListCopy[prevValue]);
                dayMatches.Add(newMatch2);
                matchHistory.Add(newMatch2);

                extraMatches++;
            }

            //Debug.Log("FINISHED SETTING UP DAY MATCHES!!!!");

            matchesRoundCont++;

            if (matchHistory.Count >= 45)
            {
                matchesRoundCont = 0;
            }
        }

        private int AddToIndex(List<Team> teamList, int indexNumber)
        {
            var indexNumberCopy = indexNumber;
            var valueToReturn = 0;

            if(indexNumberCopy < teamList.Count - 1)
            {
                indexNumberCopy++;
                valueToReturn = indexNumberCopy;
            }

            return valueToReturn;
        }

        private int SubtractFromIndex(List<Team> teamList, int indexNumber)
        {
            var indexNumberCopy = indexNumber;
            var valueToReturn = teamList.Count - 1;

            if (indexNumberCopy > 0)
            {
                indexNumberCopy--;
                valueToReturn = indexNumberCopy;
            }

            return valueToReturn;
        }

        public List<Team> ReturnTeamsList()
        {
            return _teamCreator.ReturnTeamsSigned();
        }
    }
}

