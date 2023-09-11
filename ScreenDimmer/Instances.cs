using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDimmer
{
    internal class Instances
    {
        public static MainWindow Instance { get; set; }
        public static List<DimmerWindow> ScreenDimmers { get; set; } = new List<DimmerWindow>();
        public static bool isFirstRun { get; set; } = true;
        public Instances() { }
    }
}
