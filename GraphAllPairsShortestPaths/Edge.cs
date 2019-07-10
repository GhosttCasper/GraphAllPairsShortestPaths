using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAllPairsShortestPaths
{
    public class Edge
    {
        public int Weight;
        public Vertex First; // IncidentFrom выходит (начало)
        public Vertex Second; // IncidentTo входит (конец)
        public bool InTree;

        public Edge(Vertex incidentFrom, Vertex incidentTo, int weight)
        {
            First = incidentFrom;
            Second = incidentTo;
            Weight = weight;
            InTree = false;
        }

    }

    public class IncidentEdge
    {
        public int Weight;
        public Vertex IncidentTo; // входит (конец)

        public IncidentEdge(Vertex incidentTo, int weight)
        {
            IncidentTo = incidentTo;
            Weight = weight;
        }
    }
}
