using CaveExplorer.Interfaces;
using Ink.Runtime;
using Ink;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaveExplorer
{
    class Storyland
    {
        private Ink.Runtime.Story _story;

        private IContext _context;
        public Storyland(string story, IContext context)
        {
            
            _story = new Ink.Runtime.Story(story);
            _context = context;

            //_story.BindExternalFunction("Get", (string a, string b) => _context.Get(a, b));
            //_story.BindExternalFunction("Add", (int a, int b) => a + b);
            
        }

        public bool CanContinue => _story.canContinue;

        public string Step()
        {
            var str = _story.Continue();


            switch (str)
            {
                case "INIT-VARIABLES\n":
                    return Step();

                default: return str;
            }
        }

        public bool ShowChoices(out List<Choice> choices, out List<string> tags)
        {
            //choices = new List<Choice>();
            //tags = new List<string>();

            tags = _story.currentTags;

            if (_story.currentChoices.Count > 0)
            {
                choices = new List<Choice>();

                for (int i = 0; i < _story.currentChoices.Count; ++i)
                {
                    Choice choice = _story.currentChoices[i];                   

                    choices.Add(choice);                    
                }

                

                return true;
            }

            choices = new List<Choice>(0);

            return false;
        } 

        public void Choose(int i)
        {
            _story.ChooseChoiceIndex(i);
        }
    }
}
