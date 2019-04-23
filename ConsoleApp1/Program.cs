using System;
using System.Collections.Generic;
using System.Data;
using Leo.Data;
using Leo.Fac;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Leo.Data.TestModel> models = new List<Leo.Data.TestModel>();
            for (int i = 0; i < 10; i++)
            {
                models.Add(new Leo.Data.TestModel() { ID = i, Name = Guid.NewGuid().ToString() });
            }
            DataTable dataTable = models.ToDataTable();
            var repositoryBuilder = FacManager.GetServcie<IRepositoryBuilder>();
          var repository=  repositoryBuilder.Create<TestModel>();
          Console.WriteLine(  repository.SaveChanges());
            Console.ReadLine();

        }
    }
}
