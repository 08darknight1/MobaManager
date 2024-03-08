using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIOrganizer : MonoBehaviour
{

    public GameObject leagueStandingPrefab, leagueMatchUpPrefab;

    public GameObject leagueStandingsMenu, leagueNextMatchesMenu, leagueMatchMenu, leagueEndNoPlayoffsMenu, leaguePlayoffsMenu;

    public RectTransform matchLogRectTransform;

    public TextMeshProUGUI matchLogFullText;

    public Button finishMatchButton;


    private string matchLogFullTextString;

    private LeagueOrganizer _leagueOrganizer;

    private bool _createdStandings, _scrollLogText, _playoffsTime;

    private List<GameObject> _standingObjects = new List<GameObject>();

    private List<GameObject> _nextMatchesObjects = new List<GameObject>();

    private int _currentWeek, _currentDay, _currentMatch;

    private List<Vector3> _standingsPositions = new List<Vector3>();

    private PlayoffsManager _playoffsManager;

    void Start()
    {
        leagueStandingsMenu.SetActive(true);
        leagueMatchMenu.SetActive(false);
        leagueNextMatchesMenu.SetActive(false);
        leagueEndNoPlayoffsMenu.SetActive(false);
        leaguePlayoffsMenu.SetActive(false);
        _leagueOrganizer = GameObject.Find("LeagueAdministrator").GetComponent<LeagueOrganizer>();
        _playoffsManager = GameObject.Find("LeagueAdministrator").GetComponent<PlayoffsManager>();
        matchLogFullTextString = matchLogFullText.text;
    }

    void Update()
    {
        if (!_createdStandings)
        {
            CreateInitialStandings();
            _createdStandings = true;
        }
        else if(leagueMatchMenu.activeSelf)
        {
            if(_scrollLogText)
            {
                if (matchLogRectTransform.sizeDelta.y > 0)
                {
                    var rectLocalPos = matchLogRectTransform.transform.localPosition;
                    var newPos = new Vector3(rectLocalPos.x, matchLogRectTransform.sizeDelta.y, rectLocalPos.z);
                    matchLogRectTransform.transform.localPosition = newPos;
                    _scrollLogText = false;
                }
            }
            else
            {
                if (matchLogFullText.text.Length > matchLogFullTextString.Length)
                {
                    matchLogFullTextString = matchLogFullText.text;
                    _scrollLogText = true;
                }
            }
        }
    }

    private void CreateInitialStandings()
    {
        var yPos = 180;
        var yDecrease = 35;

        var teamList = _leagueOrganizer.ReturnTeamsList();

        for (int x = 0; x < teamList.Count; x++)
        {
            var newObj = Instantiate(leagueStandingPrefab);
            newObj.transform.SetParent(leagueStandingsMenu.transform);
            newObj.transform.localScale = new Vector3(1, 1, 1);
            newObj.transform.localPosition = new Vector3(-235, yPos, 0);
            yPos -= yDecrease;
            _standingObjects.Add(newObj);
        }

        for (int x = 0; x < _standingObjects.Count; x++)
        {
            _standingsPositions.Add(_standingObjects[x].transform.localPosition);
            for(int y = 0; y < _standingObjects[x].transform.childCount; y++)
            {
                if(y == 0)
                {
                    var xPlus = x + 1;
                    _standingObjects[x].transform.GetChild(y).gameObject.GetComponent<TextMeshProUGUI>().text = xPlus.ToString();
                }
                else if(y == 1)
                {
                    _standingObjects[x].transform.GetChild(y).gameObject.GetComponent<TextMeshProUGUI>().text = teamList[x].ReturnTeamName();
                }
                else
                {
                    _standingObjects[x].transform.GetChild(y).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
                }
            }
        }
    }

    public void CreateNextMatchups()
    {
        if (_currentWeek < _leagueOrganizer.LeagueWeeksNumber)
        {
            leagueStandingsMenu.SetActive(false);
            leagueNextMatchesMenu.SetActive(true);

            var yPos = 180;
            var yDecrease = 50;

            var allMatches = _leagueOrganizer.ReturnMatchFromSpecificDayAndWeek(_currentWeek, _currentDay);

            for (int x = 0; x < allMatches.Count; x++)
            {
                var teams = allMatches[x].ReturnMatchTeams();

                var newObj = Instantiate(leagueMatchUpPrefab);
                newObj.transform.SetParent(leagueNextMatchesMenu.transform);
                newObj.transform.localScale = new Vector3(1, 1, 1);
                newObj.transform.localPosition = new Vector3(-150, yPos, 0);
                yPos -= yDecrease;

                if (allMatches[x].ReturnWinner() == 0)
                {
                    newObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = teams[0].ReturnTeamName() + " W ";
                    newObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " L " + teams[1].ReturnTeamName();
                }
                else if (allMatches[x].ReturnWinner() == 1)
                {
                    newObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = teams[0].ReturnTeamName() + " L ";
                    newObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = " W " + teams[1].ReturnTeamName();
                }
                else
                {
                    newObj.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = teams[0].ReturnTeamName();
                    newObj.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = teams[1].ReturnTeamName();
                }

                _nextMatchesObjects.Add(newObj);
            }
        }
        else
        {
            if (_leagueOrganizer.HasPlayoffs)
            {
                _playoffsTime = true;

                var newTeamsList = new List<Team>();
                var fullTeamsList = _leagueOrganizer.ReturnTeamsList();

                for (int x = 0; x < _standingObjects.Count; x++)
                {
                    var standingTeamName = _standingObjects[x].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

                    for (int y = 0; y < fullTeamsList.Count; y++)
                    {
                        if (standingTeamName == fullTeamsList[y].ReturnTeamName())
                        {
                            newTeamsList.Add(fullTeamsList[y]);
                            break;
                        }
                    }
                }

                _playoffsManager.SetTeams(newTeamsList);

                leagueStandingsMenu.SetActive(false);
                leaguePlayoffsMenu.SetActive(true);
            }
            else
            {
                leagueStandingsMenu.SetActive(false);
                leagueEndNoPlayoffsMenu.SetActive(true);

                var childList = new List<TextMeshProUGUI>();

                for(int x = 0; x < leagueEndNoPlayoffsMenu.transform.childCount; x++)
                {
                    var childTMPRO = leagueEndNoPlayoffsMenu.transform.GetChild(x).gameObject.GetComponent<TextMeshProUGUI>();
                    childList.Add(childTMPRO);
                }

                for(int x = 0; x < 3; x++)
                {
                    childList[x].text += _standingObjects[x].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;
                }

                _standingObjects.OrderByDescending(e => int.Parse(e.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text));

                var topScorerValue = _standingObjects[0].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text;
                childList[3].text += _standingObjects[0].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text + " (" + topScorerValue + ")";

                _standingObjects.OrderByDescending(e => int.Parse(e.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text));

                var bestDiffValue = _standingObjects[0].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text;
                childList[4].text += _standingObjects[0].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text + " (" + bestDiffValue + ")";
            }
        }
    }

    public void SetNextMatch()
    {
        if (!_playoffsTime)
        {
            var allMatches = _leagueOrganizer.ReturnMatchFromSpecificDayAndWeek(_currentWeek, _currentDay);

            var nextMatch = allMatches[_currentMatch];

            var nextMatchTeams = nextMatch.ReturnMatchTeams();

            var staticChanges = leagueMatchMenu.transform.GetChild(0);

            var variableChanges = leagueMatchMenu.transform.GetChild(1);

            staticChanges.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[0].ReturnTeamName();
            staticChanges.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[0].ReturnTeamOverrall().ToString();

            staticChanges.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[1].ReturnTeamName();
            staticChanges.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[1].ReturnTeamOverrall().ToString();

            variableChanges.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            variableChanges.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();

            matchLogFullText.text = "";
            matchLogFullTextString = matchLogFullText.text;
        }
        else
        {
            _playoffsManager.CheckIfCurrentSeriesIsOver();

            var nextMatch = _playoffsManager.ReturnCurrentMatchInSeries();

            var nextMatchTeams = nextMatch.ReturnMatchTeams();

            var staticChanges = leagueMatchMenu.transform.GetChild(0);

            var variableChanges = leagueMatchMenu.transform.GetChild(1);

            staticChanges.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[0].ReturnTeamName();
            staticChanges.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[0].ReturnTeamOverrall().ToString();

            staticChanges.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[1].ReturnTeamName();
            staticChanges.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = nextMatchTeams[1].ReturnTeamOverrall().ToString();

            variableChanges.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            variableChanges.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();

            matchLogFullText.text = "";
            matchLogFullTextString = matchLogFullText.text;
        }
    }

    public void AddNewMessageToMatchLog(string messageToAdd)
    {
        matchLogFullText.text += messageToAdd;
        var stringJump = "\n";
        matchLogFullText.text += stringJump;
        matchLogFullText.text += stringJump;
    }

    public void UpdateTeamsScore(int pointsForTeam1, int pointsForTeam2)
    {
        var variableChanges = leagueMatchMenu.transform.GetChild(1);
        variableChanges.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = pointsForTeam1.ToString();
        variableChanges.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = pointsForTeam2.ToString();
    }

    public void PlaySelectedMatch()
    {
        if (!_playoffsTime)
        {
            var allMatches = _leagueOrganizer.ReturnMatchFromSpecificDayAndWeek(_currentWeek, _currentDay);

            var nextMatch = allMatches[_currentMatch];

            nextMatch.PlayMatch();
        }
        else
        {
            _playoffsManager.PlayNextSeries();
        }
    }

    public void FinishMatch()
    {
        finishMatchButton.interactable = true;

        if (!_playoffsTime)
        {
            var currentMatches = _leagueOrganizer.ReturnMatchFromSpecificDayAndWeek(_currentWeek, _currentDay);
            _currentMatch++;

            //Debug.Log("CURRENT MATCH: " + _currentMatch + " | MATCHES SIZE: " + currentMatches.Count);

            if (_currentMatch > currentMatches.Count - 1)
            {
                _currentMatch = 0;
                _currentDay++;
                if (_currentDay > _leagueOrganizer.MatchDaysNumberInWeek - 1)
                {
                    _currentDay = 0;
                    _currentWeek++;
                }
            }
        }
    }

    public void ExitMatch()
    {
        if (!_playoffsTime)
        {
            if (_currentMatch == 0 && _currentDay == 0)
            {
                leagueMatchMenu.SetActive(false);
                leagueStandingsMenu.SetActive(true);
                UpdateLeagueStandingsNumbers();
            }
            else
            {
                leagueMatchMenu.SetActive(false);
                leagueNextMatchesMenu.SetActive(true);
                CreateNextMatchups();
            }
        }
        else
        {
            leagueMatchMenu.SetActive(false);
            leaguePlayoffsMenu.SetActive(true);
            _playoffsManager.UpdateUIforPlayoffs();
        }
    }

    public void DeleteAllTextChilds()
    {
        for(int x = 0; x < _nextMatchesObjects.Count; x++)
        {
            Destroy(_nextMatchesObjects[x]);
        }

        _nextMatchesObjects.Clear();
    }

    private void UpdateLeagueStandingsNumbers()
    {
        var allMatchesSoFar = _leagueOrganizer.ReturnMatchHistory();
        var allMatchesSoFar2 = _leagueOrganizer.ReturnMatchHistory2();

        var trueMatchList = new List<LeagueMatch>();

        for(int x = 0; x < allMatchesSoFar.Count; x++)
        {
            if (allMatchesSoFar[x].ReturnWinner() != -1)
            {
                trueMatchList.Add(allMatchesSoFar[x]);
            }
        }

        if(allMatchesSoFar2 != null && allMatchesSoFar2.Count > 0)
        {
            for(int x = 0; x < allMatchesSoFar2.Count; x++)
            {
                if (allMatchesSoFar2[x].ReturnWinner() != -1)
                {
                    trueMatchList.Add(allMatchesSoFar2[x]);
                }
            }
        }

        //Debug.Log("TRUE MATCH LIST SIZE: " + trueMatchList.Count);

        for(int x = 0; x < _standingObjects.Count; x++)
        {
            _standingObjects[x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            _standingObjects[x].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = 0.ToString();
        }

        for(int x = 0; x < trueMatchList.Count; x++)
        {
            var t1Name = trueMatchList[x].ReturnMatchTeams()[0].ReturnTeamName();
            var t2Name = trueMatchList[x].ReturnMatchTeams()[1].ReturnTeamName();
            var matchPointsList = trueMatchList[x].ReturnTeamsPoints();
            var winner = trueMatchList[x].ReturnWinner();

            var contToBreak = 0;

            for (int y = 0; y < _standingObjects.Count; y++)
            {
                var standingTeamName = _standingObjects[y].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

                //Debug.Log("STANDINGTEAMNAME: |" + standingTeamName + "| X TEAM1NAME: |" + t1Name + "|");
                //Debug.Log("STANDINGTEAMNAME: |" + standingTeamName + "| X TEAM2NAME: |" + t2Name + "|");

                if (standingTeamName == t1Name)
                {
                    var partidas = _standingObjects[y].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var partidasInt = int.Parse(partidas);
                    //Debug.Log("PARTIDASINT BEFORE: " + partidasInt);
                    partidasInt += 1;
                    //Debug.Log("PARTIDASINT AFTER: " + partidasInt);
                    _standingObjects[y].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = partidasInt.ToString();

                    if (winner == 0)
                    {
                        var pontos = _standingObjects[y].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var pontosInt = int.Parse(pontos);
                        pontosInt += 3;
                        _standingObjects[y].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = pontosInt.ToString();

                        var vitorias = _standingObjects[y].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var vitoriasInt = int.Parse(vitorias);
                        vitoriasInt += 1;
                        _standingObjects[y].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = vitoriasInt.ToString();
                    }
                    else
                    {
                        var derrotas = _standingObjects[y].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var derrotasInt = int.Parse(derrotas);
                        derrotasInt += 1;
                        _standingObjects[y].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = derrotasInt.ToString();
                    }

                    var pontosTotal = _standingObjects[y].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var pontosTotalInt = int.Parse(pontosTotal);
                    pontosTotalInt += matchPointsList[0];
                    _standingObjects[y].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = pontosTotalInt.ToString();

                    var pontosDiff = _standingObjects[y].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var pontosDiffInt = int.Parse(pontosDiff);
                    var diffMatch = matchPointsList[0] - matchPointsList[1];
                    pontosDiffInt += diffMatch;
                    _standingObjects[y].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = pontosDiffInt.ToString();

                    contToBreak++;
                }
                else if(standingTeamName == t2Name)
                {
                    var partidas = _standingObjects[y].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var partidasInt = int.Parse(partidas);
                    //Debug.Log("PARTIDASINT BEFORE: " + partidasInt);
                    partidasInt += 1;
                    //Debug.Log("PARTIDASINT AFTER: " + partidasInt);
                    _standingObjects[y].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = partidasInt.ToString();

                    if (winner == 1)
                    {
                        var pontos = _standingObjects[y].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var pontosInt = int.Parse(pontos);
                        pontosInt += 3;
                        _standingObjects[y].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = pontosInt.ToString();

                        var vitorias = _standingObjects[y].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var vitoriasInt = int.Parse(vitorias);
                        vitoriasInt += 1;
                        _standingObjects[y].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = vitoriasInt.ToString();
                    }
                    else
                    {
                        var derrotas = _standingObjects[y].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text;
                        var derrotasInt = int.Parse(derrotas);
                        derrotasInt += 1;
                        _standingObjects[y].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = derrotasInt.ToString();
                    }

                    var pontosTotal = _standingObjects[y].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var pontosTotalInt = int.Parse(pontosTotal);
                    pontosTotalInt += matchPointsList[1];
                    _standingObjects[y].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = pontosTotalInt.ToString();

                    var pontosDiff = _standingObjects[y].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text;
                    var pontosDiffInt = int.Parse(pontosDiff);
                    var diffMatch = matchPointsList[1] - matchPointsList[0];
                    pontosDiffInt += diffMatch;
                    _standingObjects[y].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = pontosDiffInt.ToString();

                    contToBreak++;
                }

                if(contToBreak >= 2)
                {
                    break;
                }
            }

            /*
                Child 0 = Posição
                Child 2 = Pontos
                Child 3 = Partidas
                Child 4 = Vitorias
                Child 5 = Derrotas
                Child 6 = PontosTotal
                Child 7 = Diferencial d Pontos
            * */
        }

        _standingObjects = _standingObjects.OrderByDescending(e => int.Parse(e.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text)).
        ThenByDescending(s => int.Parse(s.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text)).
        ThenByDescending(z => int.Parse(z.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text)).
        ThenByDescending(c => int.Parse(c.transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text)).
        ToList();

        for (int x = 0; x < _standingObjects.Count; x++)
        {
            var xPlus = x + 1;
            _standingObjects[x].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = xPlus.ToString();
            _standingObjects[x].transform.localPosition = _standingsPositions[x];
        }
    }
    
}
