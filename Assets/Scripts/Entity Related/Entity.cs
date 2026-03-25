namespace Assets.Scripts
{
    public class Entity
    {
        private string _firstName, _surName;

        private int _age;

        public Entity(string firstName, string surName, int age)
        {
            _firstName = firstName;
            _surName = surName;
            _age = age;
        }

        public string ReturnEntityFirstName()
        {
            return _firstName;
        }

        public string ReturnEntitySurName()
        {
            return _surName;
        }
        public string ReturnEntityCompleteName()
        {
            var nameToReturn = ("" + _firstName + " " + _surName);
            return nameToReturn;
        }

        public int ReturnEntityAge()
        {
            return _age;
        }
    }
}