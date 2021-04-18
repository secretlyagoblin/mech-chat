using CaveExplorer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaveExplorer
{
    class GameContext : IContext
    {
        public Dictionary<string,Dictionary<string,object>> Deets { get; set; }

        public object Get(string top, string bottom)
        {
            return Deets[top][bottom];
        }
    }
}
