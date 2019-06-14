using Leo.Data;
using Leo.DI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leo.Tests.NET461
{
    [TestClass]
    public class DataTest : TestBase
    {
        private static IEnumerable<IDML> dmls =  ServiceProvider.GetServices<IDML>();
    private static Query<TestEntity> query= ServiceProvider.GetService<Query<TestEntity>>();

        [TestMethod]
        public void Add()
        {
            ToDo(r =>
            {
                r.Add(GetTestEntity());
                IsTrue(r.SaveChanges() == 1);
            });
        }

        [TestMethod]
        public void Get()
        {
            ToDo(r =>
            {
                var entity = GetTestEntity();
                r.Add(entity);
                IsTrue(r.SaveChanges() == 1);
               IsTrue( query.Get(entity.Guid).CreateDate == entity.CreateDate);
            });
        }

        [TestMethod]
        public void AddRange()
        {
            ToDo((r) =>
            {
                var list = GetTestEntities();
                r.Add(list.ToArray());
                IsTrue(r.SaveChanges() == list.Count);
            });
        }

        [TestMethod]
        public void Query()
        {
            //AddRange();
            ToDo(r =>
            {
                var TestEntitys = query.Where(t=>t.Guid!="").ToArray();
                Console.WriteLine(TestEntitys.Count());
                IsTrue(TestEntitys.Any());
            });
        }

       

        [TestMethod]
        public void Remove()
        {
            ToDo(r =>
            {
                var entity = GetTestEntity();
                r.Add(entity);
                IsTrue(r.SaveChanges() == 1);
                r.Remove(entity);
                IsTrue(r.SaveChanges() == 1);
            });
        }

        [TestMethod]
        public void RemoveRange()
        {
            ToDo(r =>
            {
                var list = GetTestEntities();
                r.Add(list.ToArray());
                IsTrue(r.SaveChanges() == list.Count);
                r.Remove(list.ToArray());
                IsTrue(r.SaveChanges() == list.Count);
            });
        }


        [TestMethod]
        public void Update()
        {
            ToDo(r =>
            {
                var entity = GetTestEntity();
                r.Add(entity);
                IsTrue(r.SaveChanges() == 1);
                entity.NoUpdate = "Changed";
                entity.Num = new Random().Next();
                r.Update(entity);
                IsTrue(r.SaveChanges() == 1);
                var newEntity = query.Get(entity.Guid);
                IsTrue(newEntity.Num == entity.Num);
                Console.WriteLine(newEntity.NoUpdate);
                IsTrue(string.IsNullOrEmpty(newEntity.NoUpdate)); //未实现
            });
        }

        [TestMethod]
        public void UpdateRange()
        {
            ToDo(r =>
            {
                var list = GetTestEntities();
                r.Add(list.ToArray());
                IsTrue(r.SaveChanges() == list.Count);
                Random random = new Random();
                foreach (var item in list)
                {
                    item.Num = random.Next();
                }
                r.Update(list.ToArray());
                IsTrue(r.SaveChanges() == list.Count);
                var newEntity = query.Get(list[8].Guid);
                IsTrue(newEntity.Num == list[8].Num);
            });
        }



        private static List<TestEntity> GetTestEntities(int count = 10)
        {
            var list = new List<TestEntity>();
            for (int i = 0; i < count; i++)
            {
                list.Add(GetTestEntity());
            }
            return list;
        }

        private static TestEntity GetTestEntity()
        {
            var entity = new TestEntity()
            { Guid = Guid.NewGuid().ToString(), CreateDate = DateTime.Now }
            ;
            return entity;
        }

        private static void ToDo(Action<IDML> action)
        {
            foreach (var repository in dmls)
            {
                action.Invoke(repository);
            }
        }
    }
}
