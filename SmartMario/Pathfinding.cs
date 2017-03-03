using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMario
{
    public static class Pathfinding
    {

        private static List<LevelCell> m_Path = new List<LevelCell>();

        public static List<LevelCell> path
        {
            get
            {
                return m_Path;
            }
        }

        public static void ComputeGridWorthiness(LevelCell[,] p_Grid)
        {
            int size = p_Grid.Length;
            for (int k = (size * 2) - 2; k >= 0; k--)
            {
                for (int j = k; j >= 0; j--)
                {
                    int i = k - j;
                    if (i < size && j < size)
                    {
                        if (p_Grid[i, j].HasChamp)
                        {
                            p_Grid[i, j] = CalculateMaxWorthinessFromCell(p_Grid[i, j]) + 1;
                        }
                    }
                }
            }
        }

        private static int CalculateMaxWorthinessFromCell(LevelCell cell)
        {
            int MaxWorthiness = 0;
            //for(int i=cell.)


            return 0;
        }

    }
}
