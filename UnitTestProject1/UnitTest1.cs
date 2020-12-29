using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using PinBot2.Model;
using PinBot2.Model.Configurations;
using PinBot2.Model.PinterestObjects;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void RegexByJSON_pins()
        {
            List<Task> TaskList = new List<Task>();
            int threads = 1;
            for (int i = 0; i < threads; i++)
            {
                var newT = Task.Factory.StartNew(() =>
                {
                    string rs = System.IO.File.ReadAllText("./pins_scraped.txt");
                    dynamic d = JsonConvert.DeserializeObject(rs);
                    var a1 = d["resource_data_cache"];
                    var a2 = a1[0];
                    var a3 = a2["data"];
                    int j = 0;
                    foreach (var a5 in a3)
                    {

                        string a6 = a5["id"];
                        PinterestObject obj = new Pin(a6, "", "", "", "", "", PinterestObjectResources.BoardFeedResource);
                        Debug.WriteLine((++j) + "  " + a6);
                    }
                });
                TaskList.Add(newT);
            }
            Task.WaitAll(TaskList.ToArray());
            Assert.IsTrue(1 == 1);
        }

        [TestMethod]
        public void RegexByJSON_pinners()
        {
            List<Task> TaskList = new List<Task>();
            int threads = 1;
            for (int i = 0; i < threads; i++)
            {
                var newT = Task.Factory.StartNew(() =>
                {
                    string rs = System.IO.File.ReadAllText("./pinners_scraped.txt");
                    /*dynamic d = JsonConvert.DeserializeObject(rs);
                    var a1 = d["resource_data_cache"];
                    var a2 = a1[1];
                    var a3 = a2["data"];*/

                    dynamic d = JsonConvert.DeserializeObject(rs);
                    JArray a1 = d["resource_data_cache"];
                    JToken a2 = a1.FirstOrDefault(o => o["data"].GetType().Equals(typeof(JArray)));
                    JArray a3 = (JArray)a2["data"];

                    int j = 0;
                    foreach (JObject a4 in a3)
                    {
                        string id = (string)a4["id"];
                        string username = (string)a4["username"];
                        int pincount = (int)a4["pin_count"];
                        int followerscount = (int)a4["follower_count"];
                        bool FollowedByMe = (bool)a4["explicitly_followed_by_me"];
                        
                        Pinner p = new Pinner(id, username, "", "",PinterestObjectResources.BoardFeedResource);
                        p.PinsCount = pincount;
                        p.FollowersCount = followerscount;
                        p.FollowedByMe = FollowedByMe;

                        Debug.WriteLine((++j) + "  " + p.Id);
                    }
                });
                TaskList.Add(newT);
            }
            Task.WaitAll(TaskList.ToArray());
            Assert.IsTrue(1 == 1);
        }

    }
}
