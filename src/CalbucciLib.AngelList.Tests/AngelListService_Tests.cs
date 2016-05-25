using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalbucciLib.AngelList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalbucciLib.AngelList.Tests
{
    [TestClass()]
    public class AngelListService_Tests
    {
        protected AngelListService Als;
        [TestInitialize]
        public void Init()
        {
            Als = new AngelListService(ConfigurationManager.AppSettings["AngelListAuthToken"]);
        }

        [TestMethod()]
        public void GetMe_Test()
        {
            var me = Als.GetMe();
            Assert.IsNotNull(me);
            Assert.AreEqual("Marcelo Calbucci", me.Name);
        }

        [TestMethod()]
        public void GetUser_Test()
        {
            var sacca = Als.GetUser(209);
            Assert.IsNotNull(sacca);
            Assert.AreEqual( "Chris Sacca", sacca.Name);
        }

        [TestMethod()]
        public void GetUser_Test1()
        {
            var sacca = Als.GetUser("sacca");
            Assert.IsNotNull(sacca);
            Assert.AreEqual("Chris Sacca", sacca.Name);
        }

        [TestMethod()]
        public void GetUserByEmail_Test()
        {
            var jason = Als.GetUserByEmail("jason@calacanis.com");
            Assert.IsNotNull(jason);
            Assert.AreEqual("Jason Calacanis", jason.Name);
        }

        [TestMethod()]
        public void GetStartup_Test()
        {
            var lp = Als.GetStartup(1094798);
            Assert.IsNotNull(lp);
            Assert.AreEqual("Listpedia", lp.Name);
        }

        [TestMethod()]
        public void GetStartup_Test1()
        {
            var lp = Als.GetStartup("listpedia");
            Assert.IsNotNull(lp);
            Assert.AreEqual("Listpedia", lp.Name);
        }

        [TestMethod()]
        public void FollowUser_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UnfollowUser_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsFollowingUser_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListFollowerUserIds_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LisFollowerUsers_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListFollowingUserIds_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LisFollowingUsers_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FollowStartup_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UnfollowStartup_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsFollowingStartup_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListFollowingStartupIds_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LisFollowingStartups_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListStartupFollowers_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ListStartupFollowerIds_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Search_Test()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchSlug_Test()
        {
            Assert.Fail();
        }
    }
}