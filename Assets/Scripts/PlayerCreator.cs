using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerCreator : MonoBehaviour
    {
        private List<string> _randomNamesPool = new List<string>();
        private List<string> _randomNickNamesPool = new List<string>();
        private List<Player> _freePlayers = new List<Player>();

        private bool _readyToSharePlayers;

        private void Start()
        {
            AddStringToNameList();
            AddStringToNickNamesList();

            var roleCont = 0;
            var roleSelected = 0;

            for (int x = 0; x < _randomNamesPool.Count; x++)
            {
                var randomName = _randomNamesPool[Random.Range(0, _randomNamesPool.Count)];
                var randomNickName = _randomNickNamesPool[Random.Range(0, _randomNamesPool.Count)];
                var randomAge = Random.Range(16, 51);
                _freePlayers.Add(new Player(randomName, randomAge, randomNickName, roleSelected));
                roleCont++;
                if(roleCont >= 10)
                {
                    roleSelected++;
                    roleCont = 0;
                }
                //_freePlayers[x].PrintPlayerStats(false);
            }
            /*
            for (int x = 0; x < _freePlayers.Count; x++)
            {
                _freePlayers[x].PrintPlayerStats(true);
            }*/

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

        private void AddStringToNameList()
        {
            _randomNamesPool.Add("Ryan");
            _randomNamesPool.Add("Philip");
            _randomNamesPool.Add("Caio");
            _randomNamesPool.Add("Jonas");
            _randomNamesPool.Add("Fernando");

            _randomNamesPool.Add("Hernandez");
            _randomNamesPool.Add("Guilherme");
            _randomNamesPool.Add("Paulo");
            _randomNamesPool.Add("Bruno");
            _randomNamesPool.Add("Alexandre");

            /////-10
            
            _randomNamesPool.Add("Eduardo");
            _randomNamesPool.Add("Ricardo");
            _randomNamesPool.Add("Johny");
            _randomNamesPool.Add("Fagundes");
            _randomNamesPool.Add("Juan");

            _randomNamesPool.Add("Silva");
            _randomNamesPool.Add("Gabriel");
            _randomNamesPool.Add("Ademar");
            _randomNamesPool.Add("Adriano");
            _randomNamesPool.Add("Afonso");

            /////-20

            _randomNamesPool.Add("Alberto");
            _randomNamesPool.Add("Alice");
            _randomNamesPool.Add("Baltazar");
            _randomNamesPool.Add("Barbara");
            _randomNamesPool.Add("Bernardo");

            _randomNamesPool.Add("Caetano");
            _randomNamesPool.Add("Carlos");
            _randomNamesPool.Add("Camila");
            _randomNamesPool.Add("Claudio");
            _randomNamesPool.Add("Cristina");

            /////-30
            
            _randomNamesPool.Add("Dafne");
            _randomNamesPool.Add("Dagoberto");
            _randomNamesPool.Add("Daniel");
            _randomNamesPool.Add("Djalma");
            _randomNamesPool.Add("Douglas");

            _randomNamesPool.Add("Eliana");
            _randomNamesPool.Add("Erica");
            _randomNamesPool.Add("Evandro");
            _randomNamesPool.Add("Ester");
            _randomNamesPool.Add("Eustaquio");

            /////-40
            
            _randomNamesPool.Add("Habel");
            _randomNamesPool.Add("Hamilton");
            _randomNamesPool.Add("Helena");
            _randomNamesPool.Add("Hermes");
            _randomNamesPool.Add("Iago");

            _randomNamesPool.Add("Iara");
            _randomNamesPool.Add("Igor");
            _randomNamesPool.Add("Isadora");
            _randomNamesPool.Add("Ivete");
            _randomNamesPool.Add("Israel");

            /////-50

        }

        private void AddStringToNickNamesList()
        {
            _randomNickNamesPool.Add("Ry4n");
            _randomNickNamesPool.Add("Philihp");
            _randomNickNamesPool.Add("XioCa");
            _randomNickNamesPool.Add("sanoj");
            _randomNickNamesPool.Add("nandin");

            _randomNickNamesPool.Add("dezHer");
            _randomNickNamesPool.Add("GuiGui");
            _randomNickNamesPool.Add("Opau1");
            _randomNickNamesPool.Add("brn");
            _randomNickNamesPool.Add("xander");

            /////-10

            _randomNickNamesPool.Add("duduQe");
            _randomNickNamesPool.Add("ricarDeu5");
            _randomNickNamesPool.Add("SilverFox");
            _randomNickNamesPool.Add("gund3s");
            _randomNickNamesPool.Add("j1");

            _randomNickNamesPool.Add("ssalvo");
            _randomNickNamesPool.Add("gab3");
            _randomNickNamesPool.Add("dema5");
            _randomNickNamesPool.Add("dridri");
            _randomNickNamesPool.Add("fonfon");

            /////-20

            _randomNickNamesPool.Add("4lBeer");
            _randomNickNamesPool.Add("crazy4lice");
            _randomNickNamesPool.Add("AzrBalt");
            _randomNickNamesPool.Add("barbs");
            _randomNickNamesPool.Add("berni3");

            _randomNickNamesPool.Add("tanCaeo");
            _randomNickNamesPool.Add("carl1ns");
            _randomNickNamesPool.Add("camcam");
            _randomNickNamesPool.Add("audioClay");
            _randomNickNamesPool.Add("tinaCHR");

            /////-30

            _randomNickNamesPool.Add("enfaD");
            _randomNickNamesPool.Add("dog_0Bert");
            _randomNickNamesPool.Add("nielAd");
            _randomNickNamesPool.Add("alminhaDJ");
            _randomNickNamesPool.Add("L4S_Doug");

            _randomNickNamesPool.Add("ana314");
            _randomNickNamesPool.Add("ricaEEE");
            _randomNickNamesPool.Add("van_dro3");
            _randomNickNamesPool.Add("aESThER");
            _randomNickNamesPool.Add("EuSouTaquio");

            /////-40

            _randomNickNamesPool.Add("halibel<3");
            _randomNickNamesPool.Add("rahmilt0n");
            _randomNickNamesPool.Add("EnaFromHELL");
            _randomNickNamesPool.Add("sherme");
            _randomNickNamesPool.Add("IagaoDaMassa");

            _randomNickNamesPool.Add("IaraAra");
            _randomNickNamesPool.Add("IgorirogI");
            _randomNickNamesPool.Add("dorai_SA");
            _randomNickNamesPool.Add("vete1");
            _randomNickNamesPool.Add("rael1s");

            /////-50
        }
    }
}