using CaveExplorer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using MechStory.Story;

namespace CaveExplorer
{
    class Program
    {

        static void DebugChapters(List<Chapter> chapters)
        {
            foreach (Chapter chap in chapters)
            {
                Console.WriteLine(chap.Title);

                Console.ForegroundColor = ConsoleColor.Yellow;

                var props = 0;

                if (chap.Only.Count > 0)
                {
                    Console.Write("All of: ");
                    chap.Only.ForEach(x => Console.Write($"[{x}] "));
                    props++;
                }

                if (chap.Not.Count > 0)
                {
                    Console.WriteLine();
                    Console.Write("None of: ");
                    chap.Not.ForEach(x => Console.Write($"[{x}] "));
                    props++;
                }
                if (chap.Any.Count > 0)
                {
                    Console.WriteLine();
                    Console.Write("Any of: ");
                    chap.Any.ForEach(x => Console.Write($"[{x}] "));
                    props++;
                }

                if (props > 0) Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {

            //var subset = Chapters.All.FulfillingTagset(Tag.Swamp, Tag.Twist, Tag.Frog);

            //DebugChapters(subset);





            var story = System.IO.File.ReadAllText(@"../../../../ink/selector.ink.json");

            var game = new Storyland(story);
            game.SpecialTags.Add(Tag.Twist);
            game.SpecialTags.Add(Tag.Complication);

            //Overthinking this!
            IStoryRenderable renderer = new Renderer();

            while (game.CanContinue)
            {
                var step = game.StepForward();

                renderer.RenderStory(step);

                if (game.ShowChoices(out var choices))
                {
                    renderer.RenderChoices(choices);
                    game.Choose(renderer.WaitForChoice());
                }
            }


        }

    }
}
