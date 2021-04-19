using CaveExplorer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CaveExplorer
{
    class Renderer : IStoryRenderable
    {
        public int Timestep { get; set; } = 6;
        public Renderer()
        {
            Console.WriteLine();
        }

        public void RenderChoices(List<Choice> choices)
        {
            
                Console.WriteLine();

                choices.ForEach(x => {
                    var choice = $"{x.Index}: {x.Content}";

                    Indent(4);

                    foreach (var c in choice)
                    {
                        Console.Write(c);
                        Thread.Sleep(Timestep);
                    }

                    Console.WriteLine();
                });

                Console.WriteLine();
            }

        public void RenderStory(Step step)
        {

            Indent(4);

            foreach (var c in step.Text)
            {
                Console.Write(c);
                Thread.Sleep(Timestep);
            }

            if (step.Tags.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;

                foreach (var tag in step.Tags)
                {
                    Indent(4);
                    Console.Write('[');
                    foreach (var c in tag)
                    {
                        Console.Write(c);
                        Thread.Sleep(Timestep);
                    }
                    Console.Write("] ");

                }
                Console.WriteLine();
                
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();



        }

        private static void Indent(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write(' ');
            }
        }

        public int WaitForChoice()
        {
            var i = int.Parse(Console.ReadKey(true).KeyChar.ToString());
            Console.WriteLine();

            return i;
        }
    }
}
