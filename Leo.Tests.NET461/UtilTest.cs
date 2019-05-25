using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leo.Util.Planning;

namespace Leo.Tests.NET461
{
    [TestClass]
  public  class UtilTest
    {
        [TestMethod]
        public void PlanTest()
        {
            IPlan plan = new TimesPlan(10, new TimeSpan(0,0,5));
            plan.BeforeProgressRun += (s, e) => { Console.WriteLine(DateTime.Now.ToString()); };
            plan.Start();
        }

      
    }
}
