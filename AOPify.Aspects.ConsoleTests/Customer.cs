namespace AOPify.Aspects.ConsoleTests
{
    public class Customer
    {
        private readonly int _id;

        public Customer(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }
    }
}