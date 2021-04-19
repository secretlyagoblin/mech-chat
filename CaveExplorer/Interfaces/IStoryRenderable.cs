using System;
using System.Collections.Generic;
using System.Text;

namespace CaveExplorer.Interfaces
{
    interface IStoryRenderable
    {
        void RenderStory(Step step);

        void RenderChoices(List<Choice> choices);

        int WaitForChoice();
    }
}
