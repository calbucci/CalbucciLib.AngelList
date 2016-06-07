using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalbucciLib.AngelList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.AngelList.Tests
{
    [TestClass()]
    public class AngelListUtils_Tests
    {
        [TestMethod()]
        public void GetSlug_Test()
        {
            string[] tests = new[]
            {
                "http://notangel.com/abc", null,
                "http://angel.co/terms", null,
                "http://angel.co/abc", "abc",
                "https://angel.co/abc", "abc",
                "https://www.angel.co/abc", "abc",
                "http://angel.co/another", "another",

            };

            for (int i = 0; i < tests.Length; i += 2)
            {
                var test = tests[i];
                var expected = tests[i + 1];
                var actual = AngelListUtils.GetSlug(test);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}