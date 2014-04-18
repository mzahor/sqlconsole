using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLConsole.BizLogic.Text;

namespace SQLConsole.BizLogic.Tests
{
    [TestClass]
    public class TextUtilsTests
    {
        private const string TestCode1 = @"
-- MAIN QUEUE
SELECT COUNT(*) FROM SB_DATA_TRACKER_QUEUE

-- TIMESTAMP QUEUE
SELECT COUNT(*) FROM SB_TIMESTAMP_QUEUE 

";

        private const string TestCode2 = @"
-- MAIN QUEUE
SELECT COUNT(*) FROM SB_DATA_TRACKER_QUEUE

-- TIMESTAMP QUEUE
SELECT COUNT(*) FROM SB_TIMESTAMP_QUEUE";

        private TextUtils GetTextUtils()
        {
            return new TextUtils();
        }

        [TestMethod]
        public void GetSqlScripts_Test1()
        {
            TextUtils textUtils = GetTextUtils();

            IList<SqlScript> code = textUtils.GetScripts(TestCode1);

            Assert.AreEqual(2, code.Count);
            
            Assert.AreEqual(code[0].Code, "SELECT COUNT(*) FROM SB_DATA_TRACKER_QUEUE");
            Assert.AreEqual(code[0].Name, "MAIN QUEUE");

            Assert.AreEqual(code[1].Code, "SELECT COUNT(*) FROM SB_TIMESTAMP_QUEUE");
            Assert.AreEqual(code[1].Name, "TIMESTAMP QUEUE");
        }

        [TestMethod]
        public void GetSqlScripts_Test2()
        {
            TextUtils textUtils = GetTextUtils();

            IList<SqlScript> code = textUtils.GetScripts(TestCode2);

            Assert.AreEqual(2, code.Count);

            Assert.AreEqual(code[0].Code, "SELECT COUNT(*) FROM SB_DATA_TRACKER_QUEUE");
            Assert.AreEqual(code[0].Name, "MAIN QUEUE");

            Assert.AreEqual(code[1].Code, "SELECT COUNT(*) FROM SB_TIMESTAMP_QUEUE");
            Assert.AreEqual(code[1].Name, "TIMESTAMP QUEUE");
        }
    }
}