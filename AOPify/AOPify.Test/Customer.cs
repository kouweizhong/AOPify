namespace AOPify.Test
{
    public class Customer
    {
        private readonly int _customerID;

        public Customer(int customerID)
        {
            _customerID = customerID;
        }

        public int CustomerID
        {
            get { return _customerID; }
        }
    }
}