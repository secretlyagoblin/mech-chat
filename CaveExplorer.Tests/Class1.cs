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

            var regex = new Regex(@"#[^#^\r^)]+", RegexOptions.Multiline);
            var results = regex.Matches(c).Select(x => x.Value[1..]).ToList();
        }

        [Test]
        public void StartLineFirst()
        {
            var c = Corpi.CoprusA;

            var regex = new Regex(@"^=+");
            var results = regex.Matches(c);//.Select(x => x.Value[1..]).ToList();
        }

        [Test]
        public void GetFunctionsTest()
        {
            var c = Corpi.CoprusA;

            var regex = new Regex(@"^={2,}( )*\w*\W*$", RegexOptions.Multiline);
            var variable = new Regex(@"\w+");

            var results = regex.Matches(c).Cast<Match>();

            var obj = new List<KeyValuePair<string, List<string>>>();

            KeyValuePair<string, List<string>> current = new KeyValuePair<string, List<string>>();

            foreach (var item in results)
            {
                if (item.Value.StartsWith("\n==="))
                {
                    if (!(string.IsNullOrEmpty(current.Key))) obj.Add(current);
                    current = new KeyValuePair<string, List<string>>(variable.Match(item.Value).Value, new List<string>());
                }
                else
                {
                    current.Value.Add(variable.Match(item.Value).Value);
                }
            }

            obj.Add(current);



        }

        [Test]
        public void TotalFind()
        {
            var c = Corpi.CoprusA;

            var rogoc = new Regex(@"^={2,}[ ]*\w*\W*$|#[^#\n\r]*[\n\r]*", RegexOptions.Multiline);

            var res = rogoc.Matches(c).Cast<Match>();

            var currentIndex = res.ToList()[1].Index;

            var topTags = new List<string>();

            foreach (var match in res) 
            {
                if (currentIndex == match.Index )
                {
                    topTags.Add(match.Value);
                    currentIndex = match.Index+match.Length;
                }

                
            }
        }

        [Test]
        public void Steppin()
        {
            var c = Corpi.CoprusA;

            var lines = Regex.Split(c, "\r\n|\r|\n");

            var totalChunks = new List<Chunk>();

            Chunk chunk = null;

            var categoryMatch = new Regex(@"^={2,}( )*\w*\W*$", RegexOptions.Multiline);
            var tagMatch = new Regex(@"(#)\w+");

            var ranOutofTags = true;

            foreach (var line in lines)
            {
                var match = categoryMatch.Match(line);

                if (match.Success)
                {
                    ranOutofTags = false;
                    if (chunk is Chunk) totalChunks.Add(chunk);

                    chunk = new Chunk() { Title = match.Value.Split(" ")[1] };

                } else if(!ranOutofTags)
                {
                    var tags = tagMatch.Matches(line);
                    ranOutofTags = tags.Count == 0;

                    if (ranOutofTags) continue;

                    chunk.Tags.AddRange(tags.Select(x => x.Value));
                }
            }

            if (chunk is Chunk) totalChunks.Add(chunk);
        }

        public class Chunk{

            public string Title { get; set; }

            public List<string> Tags { get; } = new List<string>();

        }
    }
}
