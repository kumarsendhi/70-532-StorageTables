using ConsoleApp1.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnection"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("customers");
            table.CreateIfNotExists();
            //CreateCustomer(table, new CustomerUS("Kumar", "kumar.c.k@capgemini.com"));
            //GetCustomer(table, "US", "kumar.c.k@capgemini.com");
            GetAllCustomers(table);

            Console.ReadKey();
        }


        static void CreateCustomer(CloudTable table, CustomerUS customer)
        {
            TableOperation insert = TableOperation.Insert(customer);
            table.Execute(insert);
        }

        static void GetCustomer(CloudTable table, string partitionKey,string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<CustomerUS>(partitionKey, rowKey);
            var result = table.Execute(retrieve);
            Console.WriteLine(((CustomerUS)result.Result).Name);
            Console.WriteLine(((CustomerUS)result.Result).Email);
            Console.WriteLine(((CustomerUS)result.Result).PartitionKey);
            Console.WriteLine(((CustomerUS)result.Result).RowKey);
        }

        static void GetAllCustomers(CloudTable table)
        {
            TableQuery<CustomerUS> query = new TableQuery<CustomerUS>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "US"));

            foreach(CustomerUS customer in table.ExecuteQuery(query))
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine(customer.Email);
            }
        }
    }
}
