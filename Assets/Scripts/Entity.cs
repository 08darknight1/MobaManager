namespace Assets.Scripts
{
    public class Entity
    {
        private string _name;

        private int _age;

        public Entity(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public string ReturnEntityName()
        {
            return _name;
        }

        public int ReturnEntityAge()
        {
            return _age;
        }
    }
}