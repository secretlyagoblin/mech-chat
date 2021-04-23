using CaveExplorer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CaveExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var data = M
            //
            //

            var res = MechStory.Story.Tags.canopy;

            var story = System.IO.File.ReadAllText(@"../../../ink/selector.ink.json");

            var game = new Storyland(story);

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
