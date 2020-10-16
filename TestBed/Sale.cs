using ObjectRelationalMapper;

namespace TestBed
{
    [DbTable("Sale")]
    public class Sale
    {
        #region Constructors

        public Sale(string salesmanId, string firstName, string lastName, string nationality, string nationalId, string phoneNumber, string emailAddress, int age, string sex, string territory)
        {
            SalesManId = salesmanId;
            FirstName = firstName;
            LastName = lastName;
            Nationality = nationality;
            NationalId = nationalId;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Age = age;
            Sex = sex;
            Territory = territory;
        }

        #endregion

        #region Properties

        [OrmKey]
        public string SalesManId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Nationality { get; }

        public string NationalId { get; }

        public string PhoneNumber { get; }

        public string EmailAddress { get; }

        public int Age { get; }

        public string Sex { get; }

        public string Territory { get; }

        #endregion
    }
}