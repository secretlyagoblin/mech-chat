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
namespace MechStory.Story
{
    public static class Pages
    {

    }
");

              

            //sourceBuilder.Append(@"
//Comment
//");

            // finish creating the source to inject
            sourceBuilder.Append(@"
        
    

    public enum Tags{
");
            var regex = new Regex(@"(#)\w+");
            var results = regex.Matches(System.IO.File.ReadAllText(_inkFile.FullName))
                .Cast<Match>()
                .Select(x => x.Value.Substring(1))
                .ToList();

            foreach (var item in results)
            {
                sourceBuilder.Append($@"
        {item},");
            }

            sourceBuilder.Append(@"
    }
}");

            var str = sourceBuilder.ToString();
            Debug.Write(str);

            

            // inject the created source into the users compilation
            context.AddSource("helloWorldGenerator", SourceText.From(str, Encoding.UTF8));

        }

        public enum Zoot
        {
            
        }

        private Ink.Runtime.Story _story;

        private System.IO.FileInfo _inkFile;

        public void Initialize(GeneratorInitializationContext context)
        {
            var path = System.IO.Path.GetFullPath(@"C:\Users\chris\source\repos\mech-gen\MechStory\generatorSource\selector.ink");

            _inkFile = new System.IO.FileInfo(path);

            //_story = new Ink.Runtime.Story(path);

            //if (!Debugger.IsAttached)
            //{
               // Debugger.Launch();
            //}
            
        }
    }
}

