namespace Assets.Scripts
{
    public class Stat 
    {
        private string _statName;

        private int _statCurrentValue, _statMaxValue;

        public Stat(string statName, int statMaxValue)
        {
            _statName = statName;
            _statMaxValue = statMaxValue;
        }

        public void SetStatCurrentValue(int valueToSet)
        {
            _statCurrentValue = valueToSet;
        }

        public string ReturnStatName()
        {
            return _statName; 
        }

        public int ReturnStatCurrentValue()
        {
            return _statCurrentValue;   
        }

        public int ReturnStatMaxValue()
        {
            return _statMaxValue;
        }
    }
}