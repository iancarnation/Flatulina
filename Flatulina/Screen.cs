using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flatulina
{
    class Screen
    {
        //public EnvironmentSolid[] objs;
        public List<EnvironmentSolid> objs;
        public Screen() 
        {
            objs = new List<EnvironmentSolid>();
        }
    }
}
