using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace MechStory.Generators
{
    [Generator]
    public class InkCompileTimeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            // begin creating the source we'll inject into the users compilation
            var sourceBuilder = new StringBuilder(@"
using System;
using System.Collections.Generic;
namespace MechStory.Story
{
");

            sourceBuilder.Append(BuildTagEnum(_tags));
            sourceBuilder.Append(Chapter.ToClassDefinition());
            sourceBuilder.Append(BuildChapterClass(_chapters));
            sourceBuilder.Append("}");

            var str = sourceBuilder.ToString();
            Debug.Write(str);

            

            // inject the created source into the users compilation
            context.AddSource("helloWorldGenerator", SourceText.From(str, Encoding.UTF8));

        }

        private List<string> _tags = new List<string>();

        private List<Chapter> _chapters = new List<Chapter>();

        public static List<KeyValuePair<string, string>> All = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>() };


        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached) Debugger.Launch();            

            var paths = System.IO.Directory.GetFiles(@"C:\Users\chris\source\repos\mech-gen\Ink").Where(x=>System.IO.Path.GetExtension(x) != ".json");
            var getAllTags = new Regex(@"#[^#\n\r]*[\n\r]*", RegexOptions.Multiline);

            var allTags = new List<string>();

            foreach (var path in paths)
            {
                var text = System.IO.File.ReadAllText(path);
                _tags.AddRange(getAllTags.Matches(text).Cast<Match>().Select(x => x.Value));
                var chapters = CreateChapters(text);
                _chapters.AddRange(chapters);
            }

            _tags = _tags.Distinct().ToList();
        }

        internal List<Chapter> CreateChapters(string file)
        {
            var getAllChaptersAndTags = new Regex(@"^={2,}[ ]*\w*\W*$|#[^#\n\r]*[\n\r]*", RegexOptions.Multiline);
            var getName = new Regex(@"\w+", RegexOptions.Multiline);
            var allChaptersAndTags = getAllChaptersAndTags.Matches(file).Cast<Match>();

            var currentIndex = 0;

            var chapters = new List<Chapter>();


            foreach (var match in allChaptersAndTags)
            {
                if (match.Value.StartsWith("=="))
                {
                    currentIndex = match.Index + match.Length;

                    var name = getName.Match(match.Value);
                    chapters.Add(new Chapter(name.Value));
                }else if (match.Value.StartsWith("#") && Math.Abs(currentIndex-match.Index) < 6 ) //magic number
                {
                    currentIndex = match.Index + match.Length;

                    var tag = FormatTag(match.Value);
                    var chapter = chapters.Last();

                    switch (tag.Item2)
                    {
                        case TagType.Only: 
                            chapter.Only.Add(tag.Item1);
                            break;
                        case TagType.Not:
                            chapter.Not.Add(tag.Item1);
                            break;
                        case TagType.Any:
                            chapter.Any.Add(tag.Item1);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                  
                }


            }

            return chapters;
        }

        private System.Globalization.TextInfo _titleCase = System.Globalization.CultureInfo.InvariantCulture.TextInfo;

        internal string BuildTagEnum(List<string> tags)
        {
            var sb = new StringBuilder();

            var strugs = tags
                .Select(x => FormatTag(x).Item1)
                .Select(x=>x.Trim())
                .Distinct();

            sb.AppendLine($@"public enum Tag {{");

            sb.AppendLine(string.Join(",", strugs));
            sb.AppendLine($@"}}");

            return sb.ToString();
        }

        internal string BuildChapterClass(List<Chapter> chapters)
        {
            var sb = new StringBuilder();

            sb.Append($@"public static class Chapters{{");
            sb.Append($@"public static List<Chapter> All = new List<Chapter>(){{");
            sb.Append(string.Join(",",chapters.Select(x => x.ToCode())));
            sb.Append($@"}};");
            sb.Append($@"}}");

            return sb.ToString();
        }

        internal (string,TagType) FormatTag(string tag)
        {
            var lowerTag = tag.ToLowerInvariant();

            if (lowerTag.StartsWith("#not_"))
            {
                var sub = lowerTag.Substring(5);
                sub = _titleCase.ToTitleCase(sub);
                sub = sub.Replace(" ", "");
                return (sub, TagType.Not);
            } else if (lowerTag.StartsWith("#only_"))
            {
                var sub = lowerTag.Substring(6);
                sub = _titleCase.ToTitleCase(sub);
                sub = sub.Replace(" ", "");
                return (sub, TagType.Only);
            }
            else
            {
                var sub = lowerTag.Substring(1);
                sub = _titleCase.ToTitleCase(sub);
                sub = sub.Replace(" ", "");
                return (sub, TagType.Any);
            }            
        }
    }

    internal enum TagType
    {
        Only,Not,Any
    }

    public class Chapter
    {
        public List<string> Only { get; } = new List<string>();
        public List<string> Not { get; } = new List<string>();
        public List<string> Any { get; } = new List<string>();
        public string Title { get; }
        public Chapter(string title)
        {
            Title = title;
        }

        public static string ToClassDefinition()
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
public class Chapter
{
        public List<Tag> Only { get; set; } = new List<Tag>();
        public List<Tag> Not { get; set; } = new List<Tag>();
        public List<Tag> Any { get; set; } = new List<Tag>();
        public string Title { get; set; }
        public Chapter(string title)
        {
            Title = title;
        }
}
");

            return sb.ToString();
        }

        public string ToCode()
        {
            var sb = new StringBuilder();

            sb.AppendLine($@"new Chapter(""{Title}""){{");

            sb.AppendLine($@"Only = new List<Tag>(){{");
            sb.AppendLine(string.Join(",", Only.Select(x => "Tag."+x)));
            sb.AppendLine($@"}},");

            sb.AppendLine($@"Not = new List<Tag>(){{");
            sb.AppendLine(string.Join(",", Not.Select(x => "Tag." + x)));
            sb.AppendLine($@"}},");

            sb.AppendLine($@"Any = new List<Tag>(){{");
            sb.AppendLine(string.Join(",", Any.Select(x => "Tag." + x)));
            sb.AppendLine($@"}}");

            sb.AppendLine($@"}}");

            return sb.ToString();
        }
    }

}

