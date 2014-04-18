using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLConsole.BizLogic.TaskRunning;
using SqlConsole.BizLogic.TaskRunning;

namespace SQLConsole.BizLogic.Tests
{
    [TestClass]
    public class TaskRunnerTests
    {
        private Mock<IPerformanceCounter> _performanceCounterMock;

        [TestInitialize]
        public void SetUp()
        {
            _performanceCounterMock = new Mock<IPerformanceCounter>(MockBehavior.Loose);
            _performanceCounterMock.Setup(x => x.GetExecutingTime(It.IsAny<Action>())).Callback((Action<Action>)(act => act()));
        }

        private TaskRunner GetTestTaskRunner()
        {
            var testTaskRunner = new TaskRunner(_performanceCounterMock.Object);

            return testTaskRunner;
        }

        [TestMethod]
        public void CtorTest()
        {
            TaskRunner testRunner = GetTestTaskRunner();

            // if we get this far then we are ok
        }

        [TestMethod]
        public void StartRunningTest_EventsTests()
        {
            int iterationsFinished = 0;
            int tasksFinished = 0;
            bool executed = false;

            var results  = new ConcurrentBag<TaskResult>();

            TaskRunner testRunner = GetTestTaskRunner();

            testRunner.ActionToRun = () => iterationsFinished++;

            testRunner.IterationsCount = 1000;
            testRunner.TasksCount = 5;

            testRunner.TaskFinished += (sender, args) =>
                                           {
                                               tasksFinished++;
                                               results.Add(args.Result);
                                           };

            testRunner.ExecutionFinished += (sender, args) =>
            {
                Assert.AreEqual(testRunner.IterationsCount * testRunner.TasksCount, iterationsFinished);
                Assert.AreEqual(5, tasksFinished);
                executed = true;
            };

            testRunner.StartRunning();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            testRunner.Join();

            Assert.IsTrue(executed);
            Assert.AreEqual(testRunner.TasksCount, results.Count);

            // Debug info
            Console.WriteLine("Total time: {0}", results.Select(x => x.TimeTaken).Aggregate((a, b) => a + b));
        }
    }
}