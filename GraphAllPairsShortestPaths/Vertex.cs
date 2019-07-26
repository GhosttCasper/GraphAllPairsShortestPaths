using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAllPairsShortestPaths
{
    public class Vertex
    {
        public int Index { get; }
        public int Distance { get; set; }
        public Vertex Parent { get; set; }
        public int ParentWeight { get; set; }

        public bool Discovered { get; set; }
        public bool Color { get; set; }

        public Vertex(int index)
        {
            Index = index;
            Parent = null;
            Distance = int.MaxValue;
        }
    }
}
