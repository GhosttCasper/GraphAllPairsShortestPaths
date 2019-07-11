﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAllPairsShortestPaths
{
    public class Vertex
    {
        public int Index;
        public int Distance;
        public Vertex Parent;
        public int ParentWeight;

        public bool Discovered;
        public bool Color;

        public Vertex(int index)
        {
            Index = index;
            Parent = null;
            Distance = int.MaxValue;
        }
    }
}
