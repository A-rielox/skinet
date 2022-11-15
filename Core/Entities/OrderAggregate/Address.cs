namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        // NO tiene Id xq no va a ser una tabla, va a ser una prop dentro de una tabla ( la de order )

        public Address()
        {// sin parametros p' no tener problemas en la migracion
        }

        public Address(string firstName, string lastName, string street,
            string city, string state, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
