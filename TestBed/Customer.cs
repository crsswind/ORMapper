using ObjectRelationalMapper;

namespace TestBed
{
    [DbTable("Customer")]
    public class Customer
    {
        #region Constructors

        public Customer(long id, string firstName, string lastName, int status)
        {
            CustomerId = id;
            Name = firstName;
            Family = lastName;
            Status = status;
        }

        #endregion

        #region Properties

        [OrmKey]
        [DbColumn("Id")]
        public long CustomerId { get; set; }

        [DbColumn("FirstName")]
        public string Name { get; set; }

        [DbColumn("LastName")]
        public string Family { get; set; }

        [DbColumn("Status")]
        public int Status { get; set; }

        #endregion
    }
}