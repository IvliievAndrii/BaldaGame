using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldaGameWFA
{
    public enum Direction { FromZeroToCountList, FromCountListToZero }

    public struct Edge
        // Edge main matrix consist of.
    {
        public int beg;
        public int end;
    }

    public struct EdgeFromTo
        // Connection between two vertexes.
    {
        public int from { get; set; }
        public int to { get; set; }
    }

    public struct VertexNode
        // Vertex main matrix consist of.
    {
        public char letter;
        public bool visit;

        public VertexNode(char letter, bool visit)
        {
            this.letter = letter;
            this.visit = visit;
        }
    }

    public struct DictionaryKey
        // A pair key/path for every word, picked for the turn.
    {
        List<int> path;
        string word;

        public DictionaryKey(List<int> path, string word)
        {
            this.path = path;
            this.word = word;
        }

        public List<int> GetListKey()
        {
            return this.path;
        }

        public string GetWord()
        {
            return this.word;
        }
    }

    public class PCTurn
        // Class with methods, including algorithms for creating pathes/searching words/etc within PC's turn.
    {
        Vertex vert;
        VertexMatrix vertexMatrix;

        public PCTurn(string startWord)
        {
            vert = new Vertex(startWord);
            vertexMatrix = new VertexMatrix(startWord.Length * startWord.Length);
        }

        public Vertex GetVert()
        {
            return vert;
        }

        public List<List<int>> PCMakesPathes()
            // PC creates all the pathes for turn, starting from '+' vertexes.
        {
            List<List<int>> pathesALL = new List<List<int>>();
            List<List<int>> pathesTemp = new List<List<int>>();
            int transit = 0;
            int start = 0;

            for (int i = 0; i < vert.NumberVertex; i++)
            {                
                if (vert.GetVertexLetter(i) == '+')
                    // Searching start vertexes with '+'.
                {
                    transit = i;
                    vert.SetVertexVisit(transit, true);

                    for(int i1 = 0; i1 < vert.NumberVertex; i1++)
                    {
                        if (Char.IsLetter(vert.GetVertexLetter(i1)))
                        {
                            start = i1;
                            pathesTemp = CreatePassALL(start);
                            pathesALL.AddRange(pathesTemp);
                        }                        
                    }                  
                }
                vert.SetVertexVisit(i, false);
            }           
            return pathesALL;
        }

        public List<List<int>> CreatePassALL(int start)
            // PC creates all the pathes for every vertex, marked as start.
        {
            List<List<int>> pathsFromZero = null;
            List<List<int>> pathsFromCountList = null;
            List<List<int>> pathsALL = new List<List<int>>();

            for (int i = (int)Direction.FromZeroToCountList; i <= (int)Direction.FromCountListToZero; i++)
                // Passing through all the vertexes including start.
            {
                List<bool> visited = new List<bool>();

                for (int j = 0; j < vert.NumberVertex; j++)
                {
                    visited.Add(false);
                }

                List<EdgeFromTo> edFromTo = new List<EdgeFromTo>();

                if (i == (int)Direction.FromZeroToCountList)
                    // Adding passes to List<List<int>> after calling recursive method for.
                    // searching all the edges can "participate" in this turn (from the first to the last vertex).
                {
                    CreatePassDFS(start, visited, edFromTo, Direction.FromZeroToCountList);
                    pathsFromZero = GetPathInListEdgeFromTo(start, visited, edFromTo);
                }
                else if (i == (int)Direction.FromCountListToZero)
                    // Adding passes to List<List<int>> after calling recursive method for 
                    // searching all the edges can "participate" in this turn (reverse direction).
                {
                    CreatePassDFS(start, visited, edFromTo, Direction.FromCountListToZero);
                    pathsFromCountList = GetPathInListEdgeFromTo(start, visited, edFromTo);
                }
            }
            foreach (List<int> patch in pathsFromZero)
            {
                pathsALL.Add(patch);
                List<int> p = new List<int>(patch);
                p.Reverse();
                pathsALL.Add(p);
            }
            foreach (List<int> patch in pathsFromCountList)
            {
                pathsALL.Add(patch);
                List<int> p = new List<int>(patch);
                p.Reverse();
                pathsALL.Add(p);
            }
            return pathsALL;
        }

        public void CreatePassDFS(int start, List<bool> visited, List<EdgeFromTo> edFromTo, Direction direct)
            // Saving edges between vertexes with letters.
        {
            int beg = 0;
            switch (direct)
            {
                case Direction.FromZeroToCountList:
                    beg = 0;
                    break;

                case Direction.FromCountListToZero:
                    beg = vert.NumberVertex - 1;
                    break;
            }

            visited[start] = true;

            for (int i = beg; i >= 0 && i < vert.NumberVertex; i = (beg == 0) ? ++i : --i)
            {
                if (vertexMatrix[start, i] == true && visited[i] == false && (Char.IsLetter(vert.GetVertexLetter(i)) || vert.GetVertexVisit(i)))
                {
                    EdgeFromTo e = new EdgeFromTo { from = i, to = start };
                    edFromTo.Add(e);
                    CreatePassDFS(i, visited, edFromTo, direct);
                    // Starting searching for neighbor letter-including-vertexes for next vertex.
                }
            }
        }

        public List<List<int>> GetPathInListEdgeFromTo(int start, List<bool> visited, List<EdgeFromTo> edFromTo)
        {
            List<List<int>> paths = new List<List<int>>();
            for (int i = visited.Count - 1; i >= 0; i--)
                // Doing passes in reverse way.
            {
                if (visited[i] == true)
                {
                    int to = -1;
                    List<int> path1 = new List<int>();
                    path1.Add(i);
                    for (int j = edFromTo.Count - 1; j >= 0; j--)
                    {
                        if (edFromTo[j].from == i || edFromTo[j].from == to)
                        {
                            to = edFromTo[j].to;
                            path1.Add(to);
                        }
                    }
                    paths.Add(path1);
                }
            }
            return paths;
        }

        public List<DictionaryKey> PCSetsPathesToWords(List<List<int>> pathesALL, SortedDictionary<string, int> dictionary)
        {
            List<DictionaryKey> dkeyMass = new List<DictionaryKey>();

            for (int i = 0; i < pathesALL.Count; i++)
                // Checking new word by pc -making a substitution all the letters.
            {
                for (int j = 0; j < pathesALL[i].Count; j++) 
                    // Getting into the new list w/path.
                {
                    int index = pathesALL[i][j];
                    if (vert.GetVertexLetter(index) == '+')
                        // Substitution all the letter to '+' place.
                    {
                        for (char k = 'а'; k <= 'я'; k++)
                        {
                            char local = vert.GetVertexLetter(index);
                            vert.SetVertexLetter(index, k);
                            string word = FormingWordFromPath(pathesALL[i]);
                            if (dictionary.ContainsKey(word))
                            {
                                dkeyMass.Add(new DictionaryKey(pathesALL[i], word));
                            }
                            vert.SetVertexLetter(index, local);
                        }
                    }
                }
            }
            return dkeyMass;
        }

        public bool KeyListIsEmpty(List<DictionaryKey> dkeyMass)
        {
            return dkeyMass.Count == 0;
        }

        public object PCChooseWordForTurn(int pcLevel, List<DictionaryKey> dkeyMass, SortedDictionary<string, int> dictionary)
            // PC chooses word fo turn (just in the beta-version) 
        {
            int first = 0;
            object result;

            SortDictKeys(dkeyMass, first, dkeyMass.Count - 1);

            switch (pcLevel)
            {
                case 1:
                    foreach (DictionaryKey dk in dkeyMass.ToArray())
                    {
                        if (dk.GetWord().Length > 4)
                        {
                            dkeyMass.Remove(dk);
                        }
                    }
                    break;
                case 2:
                    foreach(DictionaryKey dk in dkeyMass.ToArray())
                    {
                        if (dk.GetWord().Length > 5)
                        {
                            dkeyMass.Remove(dk);
                        }
                    }
                    break;
                case 3:
                    // Just to show that pc on hard level is able to use anyw word - 
                    // starting from the longest and finishing the shortest one.
                    // That's why no removing needed - just sorted dkeyMass.
                    break;
            }
            return result = (dkeyMass.Count != 0)? (object)dkeyMass[dkeyMass.Count - 1] : null;
        }

        public static void SortDictKeys(List<DictionaryKey> dkeyMass, int first, int last)
        {
            DictionaryKey p = dkeyMass[(last - first) / 2 + first];
            DictionaryKey temp;
            int i = first, j = last;
            while (i <= j)
            {
                while (dkeyMass[i].GetWord().Length < p.GetWord().Length && i <= last) ++i;
                while (dkeyMass[j].GetWord().Length > p.GetWord().Length && j >= first) --j;
                if (i <= j)
                {
                    temp = dkeyMass[i];
                    dkeyMass[i] = dkeyMass[j];
                    dkeyMass[j] = temp;
                    ++i; --j;
                }
            }
            if (j > first) SortDictKeys(dkeyMass, first, j);
            if (i < last) SortDictKeys(dkeyMass, i, last);
        }

        public string FormingWordFromPath(List<int> path)
        {
            string word = "";

            for (int i = 0; i < path.Count; i++)
            {
                word += Char.ToLower(vert.GetVertexLetter(path[i]));
            }
            return word;
        }
    }
}

