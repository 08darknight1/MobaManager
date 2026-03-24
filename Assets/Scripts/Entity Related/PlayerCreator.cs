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

        private void Start()
        {
            //Precisa sempre ter a mesma quantidade de nomes e nicknames

            ReadNameList();

            var roleCont = 0;
            var roleSelected = 0;
            var namePoolSize = _randomNamesPool.Count;

            for (int x = 0; x < namePoolSize; x++)
            {
                //Debug.Log("RoleSelected :" + roleSelected + "| Role Count: " + roleCont);

                var randomValue = Random.Range(0, _randomNamesPool.Count - 1);

                var randomName = _randomNamesPool[randomValue];
                _randomNamesPool.RemoveAt(randomValue);

                var randomNickName = _randomNickNamesPool[randomValue];
                _randomNickNamesPool.RemoveAt(randomValue);

                var randomAge = Random.Range(16, 99);

                _freePlayers.Add(new Player(randomName, randomAge, randomNickName, roleSelected));

                roleCont++;

                if(roleCont >= 10)
                {
                    roleSelected++;
                    roleCont = 0;
                }
            }

            //Debug.Log("Free Players Size: " + _freePlayers.Count);

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

        private void ReadNameList() 
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
                //Debug.Log("Name found at [" + x + "]: " + allTextFromFile[x]);

                //If feito para ele pular as 3 primeiras categorias do cabeçalho

                if (x > 2)
                {
                    switch (cont)
                    {
                        case 0:
                            _randomNamesPool.Add(allTextFromFile[x]);
                            //Debug.Log("Added " + allTextFromFile[x] + " to 1st names list!");
                            cont++;
                            break;
                        case 1:
                            _randomNickNamesPool.Add(allTextFromFile[x]);
                            //Debug.Log("Added " + allTextFromFile[x] + " to Nick Names list!");
                            cont++;
                            break;
                        case 2:
                            _randomSurNamesPool.Add(allTextFromFile[x]);
                            //Debug.Log("Added " + allTextFromFile[x] + " to 2nd names list!");
                            cont = 0;
                            break;
                    }
                }
            }

            /*
            Debug.Log("1st Names List Size: " + _randomNamesPool.Count);

            for (int y = 0; y < _randomNamesPool.Count; y++)
            {
                Debug.Log("1st Names found[" + y + "]: " + _randomNamesPool[y]);
            }

            Debug.Log("NickNames List Size: " + _randomNickNamesPool.Count);

            for (int y = 0; y < _randomNickNamesPool.Count; y++)
            {
                Debug.Log("NickNames found[" + y + "]: " + _randomNickNamesPool[y]);
            }

            Debug.Log("SurNames List Size: " + _randomSurNamesPool.Count);

            for (int y = 0; y < _randomSurNamesPool.Count; y++)
            {
                Debug.Log("SurNames found[" + y + "]: " + _randomSurNamesPool[y]);
            }*/
        }
    }
}