using System;
using System.Collections.Generic;
using System.Linq;
using LogicalExpressionGenerator;
using ObjectRelationalMapper;

namespace TestBed
{
    internal class Program
    {
        #region Event Handlers

        public static void customers_RowAdded(object sender, EventArgs e)
        {
            Console.WriteLine("One row(s) added successfully");
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            IDbProvider provider = new SqlDataProvider();
            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";

            var context = new OrmContext(provider);

            Repository<Customer> customers = context.GetRepository<Customer>();
            Repository<PartDto> parts = context.GetRepository<PartDto>();

            var customer = new Customer(7, "Laura", "Branigan", 20);

            customers.RowAdded += customers_RowAdded;
            //customers.Insert(customer);
            customers.Delete(customer);
            var property = new DataProperty
            {
                Name = "FirstName",
                Value = "Michael"
            };

            DataProperty[] properties = { property };
            //customers.Update(properties, "123");
            //parts.Delete("112");

            context.SaveChanges();
        }

        private static void BatchTest()
        {
            IDbProvider provider = new SqlDataProvider();
            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";
            var repository = new Repository<Customer>(provider);
            var saleRepository = new Repository<Sale>(provider);
            var property = new DataProperty
            {
                Name = "FirstName",
                Value = "Michael"
            };
            //repository.Update("23", property);

            repository.Delete("23");
            repository.Delete("53");
        }

        private static void UpdateTest()
        {
            IDbProvider provider = new SqlDataProvider();
            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";

            var repository = new Repository<Customer>(provider);

            //var name = new StringVariable("FirstName");
            //var value = new StringConstant("Tony");
            //var condition = new ComparativeOperator<string>(ComparativeOperatorType.Equal, name, value);
            //repository.Update(condition, property);

            var property = new DataProperty
            {
                Name = "FirstName",
                Value = "Diego"
            };

            DataProperty[] properties = { property };

            repository.Update(properties, "23");
        }

        private static void SelectTest()
        {
            IDbProvider provider = new SqlDataProvider();

            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";
            var repository = new Repository<Customer>(provider);

            string[] fields = { "Id", "Name" };

            var name = new StringVariable("Name");
            var value = new StringConstant("o");
            var condition = new ComparativeOperator<string>(ComparativeOperatorType.Like, name, value);
            List<DataProperty[]> result = repository.Select(condition, fields);

            //List<Customer2> allCustomers = repository.LoadAll();
            //List<Customer2> customersById = repository.LoadById(1,2,3);
            //List<Customer2> customersByFilter = repository.LoadByFilter(condition);

            if (result.Count != 0)
                Console.WriteLine(result.Select(record => "{" + record.Select(p => p.Name + ": " + p.Value).Aggregate((p1, p2) => p1 + ", " + p2) + "}")
                                        .Aggregate((r1, r2) => r1 + "\n" + r2));
            else
                Console.WriteLine("No record has been found for this condition!");
        }

        private static void DeleteTest()
        {
            IDbProvider provider = new SqlDataProvider();
            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";
            var repository = new Repository<Customer>(provider);
            repository.Delete("832");

            var name = new StringVariable("FirstName");
            var value = new StringConstant("Alex");
            var condition = new ComparativeOperator<string>(ComparativeOperatorType.Equal, name, value);
            repository.Delete(condition);
            var customer = new Customer(4, "Lucy", "Bronze", 0);
            repository.Delete(customer);
        }

        private static void InsertTest()
        {
            var customer = new Customer(17, "Frank", "Lampard", 77);

            IDbProvider provider = new SqlDataProvider();
            provider.ConnectionString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={Settings.Instance.Database};Data Source={Settings.Instance.ServerName}";
            var repository = new Repository<Customer>(provider);
            repository.Insert(customer);
        }

        #endregion
    }
}