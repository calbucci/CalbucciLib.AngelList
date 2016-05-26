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
            Assert.IsNotNull(me.Email);
        }

        [TestMethod()]
        public void GetUser_Test()
        {
            var sacca = Als.GetUser(209);
            Assert.IsNotNull(sacca);
            Assert.AreEqual("Chris Sacca", sacca.Name);
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
        public void ListUsers_Test()
        {
            List<int> userIds = new List<int>
            {
                209, // sacca
                44979, // calbucci
            };

            var users = Als.ListUsers(userIds);
            Assert.AreEqual(userIds.Count, users.Count);
            foreach (var user in users)
            {
                Assert.IsNotNull(user);
                Assert.IsTrue(userIds.Contains(user.Id));
            }
        }


        [TestMethod()]
        public void FollowUser_Test()
        {
            int randomUserId = 123020; // https://angel.co/peterlthomson
            var ret = Als.FollowUser(randomUserId);
            Als.UnfollowUser(randomUserId); // unfollow even before the asserts to clear up for next test

            Assert.IsNotNull(ret);
            Assert.AreEqual(44979, ret.Follower.Id);
            Assert.AreEqual(randomUserId, ret.Followed.Id);
        }


        [TestMethod()]
        public void IsFollowingUser_Test()
        {
            var ret = Als.IsFollowingUser(44979, 209);
            Assert.IsTrue(ret);
        }

        [TestMethod()]
        public void ListFollowerUserIds_Test()
        {
            var ret = Als.ListFollowerUserIds(44979);
            Assert.IsTrue(ret.Count > 1000);

        }

        [TestMethod()]
        public void ListFollowerUsers_Test()
        {
            var ret = Als.ListFollowerUsers(44979);
            Assert.IsTrue(ret.Count > 1000);
        }

        [TestMethod()]
        public void ListFollowingUserIds_Test()
        {
            var ret = Als.ListFollowingUserIds(44979);
            Assert.IsTrue(ret.Count > 100);
            Assert.IsTrue(ret.Contains(209)); // sacca
        }

        [TestMethod()]
        public void ListFollowingUsers_Test()
        {
            var ret = Als.ListFollowingUsers(44979);
            Assert.IsTrue(ret.Count > 100);
            Assert.IsTrue(ret.Any(u => u.Id == 209)); // sacca

        }

        [TestMethod()]
        public void FollowStartup_Test()
        {
            int randomStartupId = 563464; // https://angel.co/bananadesk
            var ret = Als.FollowStartup(randomStartupId);
            Als.UnfollowStartup(randomStartupId); // clear up for next test

            Assert.IsNotNull(ret);
            Assert.AreEqual(44979, ret.Follower.Id);
            Assert.AreEqual(randomStartupId, ret.Followed.Id);
        }

        [TestMethod()]
        public void IsFollowingStartup_Test()
        {
            int listpediaId = 1094798;
            var ret = Als.IsFollowingStartup(44979, listpediaId);
            Assert.IsTrue(ret);
        }

        [TestMethod()]
        public void ListFollowingStartupIds_Test()
        {
            var ret = Als.ListFollowingStartupIds(44979);
            Assert.IsTrue(ret.Count > 20);
        }

        [TestMethod()]
        public void ListFollowingStartups_Test()
        {
            var ret = Als.ListFollowingStartups(44979);
            Assert.IsTrue(ret.Count > 20);
        }

        [TestMethod()]
        public void ListStartupFollowers_Test()
        {
            int listpediaId = 1094798;
            var ret = Als.ListStartupFollowers(listpediaId);
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod()]
        public void ListStartupFollowerIds_Test()
        {
            int listpediaId = 1094798;
            var ret = Als.ListStartupFollowerIds(listpediaId);
            Assert.IsTrue(ret.Count > 0);
        }

        [TestMethod()]
        public void Search_Test()
        {
            int listpediaId = 1094798;
            var ret = Als.Search("listpedia", Model.AngelListEntityType.Startup);
            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Count > 0);
            Assert.IsTrue(ret.Any(r => r.Id == listpediaId));
        }

        [TestMethod()]
        public void SearchSlug_Test()
        {
            int listpediaId = 1094798;
            var ret = Als.SearchSlug("listpedia", Model.AngelListEntityType.Startup);
            Assert.IsNotNull(ret);
            Assert.IsTrue(ret.Id == listpediaId);
        }

    }
}