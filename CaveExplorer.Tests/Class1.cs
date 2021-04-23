using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaveExplorer.Tests
{
    public class Class1
    {
        [Test]
        public void Testing()
        {
            Assert.True(3 == 3);
        }

        [Test]
        public void GetTagsTest()
        {
            var c = Corpi.CoprusA;

            var regex = new Regex(@"(#)\w+");
            var results = regex.Matches(c).Select(x=>x.Value[1..]).ToList();
        }

        [Test]
        public void GetFunctionsTest()
        {
            var c = Corpi.CoprusA;

            var regex = new Regex(@"={2,}\s\w+\s=*\W");
            var variable = new Regex(@"\w+");

            var results = regex.Matches(c).Cast<Match>();

            var obj = new List<KeyValuePair<string, List<string>>>();

            KeyValuePair<string, List<string>> current = new KeyValuePair<string, List<string>>();

            foreach (var item in results)
            {
                if (item.Value.StartsWith("==="))
                {
                    if (!(string.IsNullOrEmpty(current.Key))) obj.Add(current);
                    current = new KeyValuePair<string, List<string>>(variable.Match(item.Value).Value,new List<string>());
                }
                else
                {
                    current.Value.Add(variable.Match(item.Value).Value);
                }
            }

            obj.Add(current);


        }
    }
}
