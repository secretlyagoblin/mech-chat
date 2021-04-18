using System;
using System.Collections.Generic;
using System.Threading;

namespace CaveExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new GameContext
            {
                Deets = new Dictionary<string, Dictionary<string, object>>()
            {
                {"Cave",new Dictionary<string, object>(){ { "Size", "Huge" } } }
            }
            };

            var story = System.IO.File.ReadAllText(@"../../../ink/selector.js");

            var game = new Storyland(story, context);

            Console.WriteLine();

            var timestep = 6;

            while (game.CanContinue)
            {
                var str = game.Step();

                Step(4);

                foreach (var c in str)
                {
                    Console.Write(c);
                    Thread.Sleep(timestep);
                }

                //Console.WriteLine(game.Step());

                if (game.ShowChoices(out var choices))
                {
                    Console.WriteLine();
                    choices.ForEach(x => { 
                        var choice = $"{x.index}: {x.text.Replace("\n","")}";

                        Step(4);

                        foreach (var c in choice)
                        {
                            Console.Write(c);
                            Thread.Sleep(timestep);
                        }

                        Console.WriteLine();

                    });

                    game.Choose(int.Parse(Console.ReadKey(true).KeyChar.ToString()));
                    Console.WriteLine();
                }
            }


        }

        private static void Step(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write(' ');
            }
        }
    }
}
