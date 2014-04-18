using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SQLConsole.BizLogic.Tests
{
    [TestClass]
    public class DevTests
    {
        [TestMethod]
        public void DelimiterTest()
        {
            string sql = @"-- MAIN QUEUE 
select * from sb_core_queue

-- TIMESTAMP QUEUE

SELECT * FROM SB_TIMESTAMP_QUEUE";

            var result = _delimiterRegExp.Split(sql);
            var res2 = _delimiterRegExp.Matches(sql);
        }


        private const string DelimiterPattern = @"^--\s+([A-Za-z0-9\s]+)\s+$";
        private readonly Regex _delimiterRegExp = new Regex(DelimiterPattern, RegexOptions.Multiline | RegexOptions.Compiled);

        private string[] SplitSqlScript(string script)
        {
            return _delimiterRegExp.Split(script);
        }
    }
}
