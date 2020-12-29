using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinBot2.Algorithms;
using PinBot2.Common;

namespace UnitTestProject1
{
    [TestClass]
    public class AutopilotTests
    {
        [TestMethod]
        public void Test_TimeRangeStart_1()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(11, 30, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_2()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(1, 11, 0, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(true, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_3()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(1, 9, 0, 1));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_4()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 9, 0, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_5()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 13, 0, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_6()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(11, 0, 0)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 11, 0, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_7()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(10, 59, 59)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 11, 0, 0));//time now
            bool maxReached = true;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_8()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(10, 59, 59)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 11, 0, 0));//time now
            bool maxReached = false;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(true, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_9()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(10, 59, 59)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 13, 0, 0));//time now
            bool maxReached = false;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }
        [TestMethod]
        public void Test_TimeRangeStart_10()
        {
            var min = new TimeSpan(10, 0, 0);
            var max = new TimeSpan(12, 0, 0);
            var WhenMaxReached = DateTime.Today.Add(new TimeSpan(10, 59, 59)); //time when max was reached
            var Now = DateTime.Today.Add(new TimeSpan(0, 9, 0, 0));//time now
            bool maxReached = false;

            var ret = Test_TimeRange(min, max, WhenMaxReached, Now, maxReached);
            System.Diagnostics.Debug.WriteLine("Must be active: " + ret);
            Assert.AreEqual(false, ret);
        }


        private bool Test_TimeRange(TimeSpan min, TimeSpan max, DateTime WhenMaxReached, DateTime Now, bool maxReached)
        {
            var target = new PinAlgo(null);
            target.Config.Autopilot = true;
            target.Config.AutoStart = new Range<DateTime>(DateTime.Today + min, DateTime.Today + max);
            var ret = target.MustBeActive(maxReached, Now, WhenMaxReached);
            return ret;
        }

    }
}
