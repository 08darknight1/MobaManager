using System.Collections.Generic;

namespace Assets.Scripts
{
    public class LeagueWeek
    {
        private List<LeagueDay> _leagueDays;

        public LeagueWeek(int matchdays)
        {
            _leagueDays = new List<LeagueDay>();

            for(int x = 0; x < matchdays; x++)
            {
                _leagueDays.Add(new LeagueDay());
            }
        }

        public List<LeagueDay> ReturnWeeksLeagueDay()
        {
            return _leagueDays;
        }
    }
}