using System;
using System.Collections.Generic;
using System.Text;

namespace CaveExplorer.Interfaces
{
    interface IContext
    {
        public object Get(string top, string bottom);
    }
}
