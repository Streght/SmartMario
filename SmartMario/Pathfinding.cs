using System.Collections.Generic;

namespace SmartMario
{
    public static class Pathfinding
    {
        private static List<LevelCell> m_Path = new List<LevelCell>();
        private static LevelCell[,] m_Grid = null;
        private static int m_GridSize = -1;

        public static List<LevelCell> Path
        {
            get
            {
                return m_Path;
            }
        }

        private static void ComputeGridWorthiness()
        {
            for (int k = (m_GridSize * 2) - 2; k >= 0; k--)
            {
                for (int j = k; j >= 0; j--)
                {
                    int i = k - j;
                    if (i < m_GridSize && j < m_GridSize)
                    {
                        if (m_Grid[i, j].HasChamp)
                        {
                            m_Grid[i, j].Worthiness = CalculateMaxWorthinessFromCell(m_Grid[i, j]) + 1;
                        }
                    }
                }
            }
        }

        private static int CalculateMaxWorthinessFromCell(LevelCell cell)
        {
            int MaxWorthiness = 0;
            for (int i = cell.ColumnIndex; i < m_Grid.Length; i++)
            {
                for (int j = cell.ColumnIndex; j < m_Grid.Length; j++)
                {
                    if (m_Grid[i, j].HasChamp)
                    {
                        if (m_Grid[i, j].Worthiness > MaxWorthiness)
                        {
                            MaxWorthiness = m_Grid[i, j].Worthiness;
                        }
                    }
                }
            }
            return MaxWorthiness;
        }

        private static void FindPathToPeach(LevelCell cell)
        {
            int MaxWorthiness = 0;
            LevelCell NextCase = null;

            for (int i = cell.ColumnIndex; i < m_GridSize; i++)
            {
                for (int j = cell.ColumnIndex; j < m_GridSize; j++)
                {
                    if (m_Grid[i, j].Worthiness > MaxWorthiness)
                    {
                        NextCase = m_Grid[i, j];
                    }
                }
            }

            for (int i = cell.ColumnIndex; i < NextCase.ColumnIndex; i++)
            {
                Path.Add(m_Grid[cell.LineIndex, i]);
            }

            for (int i = cell.LineIndex; i < NextCase.LineIndex; i++)
            {
                Path.Add(m_Grid[i, cell.LineIndex]);
            }

            if (NextCase.HasPeach)
            {
                return;
            }
            else
            {
                FindPathToPeach(NextCase);
            }
            return;
        }

        public static void ComputeMaxChampPath(LevelCell[,] p_LevelCellMatrix)
        {
            m_Grid = p_LevelCellMatrix;
            m_GridSize = p_LevelCellMatrix.Length;

            ComputeGridWorthiness();
            FindPathToPeach(m_Grid[0, 0]);
        }
    }

}
