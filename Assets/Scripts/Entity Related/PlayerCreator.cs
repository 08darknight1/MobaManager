using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Assets.Scripts
{
    public class PlayerCreator : MonoBehaviour
    {
        private List<string> _randomNamesPool = new List<string>();
        private List<string> _randomNickNamesPool = new List<string>();
        private List<string> _randomSurNamesPool = new List<string>();
        private List<Player> _freePlayers = new List<Player>();

        private bool _readyToSharePlayers;

        public int NumberOfPlayersToCreate;

        private void Start()
        {
            //Precisa sempre ter a mesma quantidade de nomes e nicknames

            ReadNamesFromCSVFile();

            var roleContMax = NumberOfPlayersToCreate/5; //isso aqui tem que ser definido em um menu

            var roleCont = 0;

            var roleSelected = 0;

            while (true)
            {

                break;
            }

            for (int x = 0; x < NumberOfPlayersToCreate; x++)
            {
                var random1stName = ReturnNewItemFromList(_randomNamesPool, false);
                var randomNickName = ReturnNewItemFromList(_randomNickNamesPool, true);
                var randomSurName = ReturnNewItemFromList(_randomSurNamesPool, false);

                var randomAge = Random.Range(16, 99);

                _freePlayers.Add(new Player(random1stName, randomSurName, randomAge, randomNickName, roleSelected));

                roleCont++;

                if(roleCont >= roleContMax)
                {
                    roleSelected++;
                    roleCont = 0;
                }
            }

            _readyToSharePlayers = true;
        }

        public bool ReturnPlayersReadyForSeason()
        {
            return _readyToSharePlayers;
        }

        public List<Player> ReturnPlayersReadyList()
        {
            return _freePlayers;
        }

        private string ReturnNewItemFromList(List<string> listToSearch, bool removeItemFromList)
        {
            var randomValue = Random.Range(0, 1000);
            var selectedItem = "NnfLis" + randomValue.ToString(); ///means no names from list

            if (listToSearch.Count > 0 )
            {
                randomValue = Random.Range(0, listToSearch.Count - 1);
                selectedItem = listToSearch[randomValue];

                if (removeItemFromList)
                {
                    listToSearch.RemoveAt(randomValue);
                }
            }

            return selectedItem;
        }

        private void ReadNamesFromCSVFile() 
        {
            var TextAsset = Resources.Load("Imported\\ExcelNames") as TextAsset;

            if(TextAsset == null)
            {
                Debug.Log("Couldnt find file with names for the player...");
            }
            else
            {
                Debug.Log("File for the name of the players found!");
            }

            string[] allTextFromFile = TextAsset.text.Split(new string[] {";","\n"}, System.StringSplitOptions.RemoveEmptyEntries);

            int cont = 0;

            for (int x = 0; x < allTextFromFile.Length; x++)
            {
                if (x > 2)
                {
                    switch (cont)
                    {
                        case 0:
                            _randomNamesPool.Add(allTextFromFile[x]);
                            cont++;
                        break;
                        case 1:
                            _randomNickNamesPool.Add(allTextFromFile[x]);
                            cont++;
                        break;
                        case 2:
                            _randomSurNamesPool.Add(allTextFromFile[x]);
                            cont = 0;
                        break;
                    }
                }
            }
        }
    }
}