using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldaGameWFA
{
    public class Vertex
    {
        VertexNode[] vertex;
        // Graph's vertexes.

        int numberVertex;

        public int NumberVertex
        {
            get { return numberVertex; }
            set { numberVertex = value; }
        }

        public char GetVertexLetter(int index)
        {
            return vertex[index].letter;
        }

        public bool GetVertexVisit(int index)
        {
            return vertex[index].visit;
        }

        public void SetVertexLetter(int index, char let)
        {
            vertex[index].letter = let;
        }

        public void SetVertexVisit(int index, bool vis)
        {
            vertex[index].visit = vis;
        }

        public Vertex(string startWord)
        {
            NumberVertex = startWord.Length * startWord.Length;

            vertex = new VertexNode[NumberVertex];

            int indexBegStartWord = startWord.Length * 2;
            int indexEndStartWord = indexBegStartWord + startWord.Length;
            
            for (int i = 0, j = 0; i < NumberVertex; i++)
                // set vertex 10-11-12-13-14 by startWord letters
            {
                vertex[i] = (i >= indexBegStartWord && i < indexEndStartWord) ?
                             new VertexNode(startWord[j++], false) :
                             new VertexNode('-', false);
            }
            
            for (int i = indexBegStartWord - startWord.Length; i < indexEndStartWord - startWord.Length; i++)
                // Set vertex 5-6-7-8-9 by "+".
            {
                vertex[i] = new VertexNode('+', false);
            }
            
            for (int i = indexBegStartWord + startWord.Length; i < indexEndStartWord + startWord.Length; i++)
                // Set vertex 15-16-17-18-19 by "+".
            {
                vertex[i] = new VertexNode('+', false);
            }
        }
    }
}
