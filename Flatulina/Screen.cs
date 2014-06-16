using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flatulina
{
    class Screen
    {
        public List<EnvironmentSolid> objs;
        public List<Enemy> enemies;
        public Screen() 
        {
            objs = new List<EnvironmentSolid>();
            enemies = new List<Enemy>();
        }
    }
}
