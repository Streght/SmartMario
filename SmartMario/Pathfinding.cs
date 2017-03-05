using System.Collections.Generic;

namespace SmartMario
{
    /// <summary>
    /// Class which contains the algorithm to compute the path to pick up the most mushrooms
    /// </summary>
    public static class Pathfinding
    {
        /// <summary>
        /// The list used to store the path to pick up the most mushrooms
        /// </summary>
        private static List<LevelCell> m_Path = new List<LevelCell>();
        /// <summary>
        /// The level grid (i.e. the LevelCellGrid)
        /// </summary>
        private static LevelCell[,] m_LevelGrid = null;
        /// <summary>
        /// The size of the level
        /// </summary>
        private static int m_LevelGridSize = -1;
        /// <summary>
        /// How many mushrooms can be picked up at most
        /// </summary>
        private static int m_GridMaximumMushroomsNumber = -1;
        /// <summary>
        /// Worthiness index used in recurrence to keep track of the current worthiness
        /// </summary>
        private static int WorthinessIndex = -1;

        #region Getters

        /// <summary>
        /// Getter for the path
        /// </summary>
        public static List<LevelCell> Path
        {
            get
            {
                return m_Path;
            }
        }

        /// <summary>
        /// Getter for the Maximum numbur of mushroom which can be picked up
        /// </summary>
        public static int GridMaximumMushroomsNumber
        {
            get
            {
                return m_GridMaximumMushroomsNumber;
            }
        }

        #endregion

        /// <summary>
        /// Entry point for the class, used to calculate the pathfinding to get the maximum
        /// number of mushrooms in a given matrix of LevelCell
        /// </summary>
        /// <param name="p_LevelCellMatrix"></param>
        public static void ComputeMaxChampPath(LevelCell[,] p_LevelCellMatrix)
        {
            m_LevelGrid = p_LevelCellMatrix;
            m_LevelGridSize = p_LevelCellMatrix.GetLength(0);

            // Compute the worthiness for every mushroom
            ComputeGridWorthiness();

            // Initialise the worthiness index used to find the next cell in the pathfinding algorithm
            WorthinessIndex = m_GridMaximumMushroomsNumber;
            m_Path.Add(m_LevelGrid[0, 0]);

            // Find the path used to pick up the maximum number of mushrooms
            FindPathToPeach(m_LevelGrid[0, 0]);
        }

        /// <summary>
        /// Here, we go through the level grid starting from the bottom right cell and going up diagonally.
        /// For every cell which countains a mushroom, we find the worthiness of the next reachable mushroom
        /// and we add one to have the current cell worthiness
        /// </summary>
        private static void ComputeGridWorthiness()
        {
            // We go though the matrix diagonally
            for (int k = (m_LevelGridSize * 2) - 2; k >= 0; k--)
            {
                for (int j = k; j >= 0; j--)
                {
                    int i = k - j;
                    if (i < m_LevelGridSize && j < m_LevelGridSize)
                    {
                        // If our current cell contains a mushroom
                        if (m_LevelGrid[j, i].HasChamp)
                        {
                            // We find the highest reachable worthiness and we add 1
                            m_LevelGrid[j, i].Worthiness = CalculateMaxWorthinessFromCell(m_LevelGrid[j, i]) + 1;
                            // If the cell worthiness is higher than the current maximum number of mushrooms
                            if (m_LevelGrid[j, i].Worthiness > m_GridMaximumMushroomsNumber)
                            {
                                // We update the global maximum number of mushrooms
                                m_GridMaximumMushroomsNumber = m_LevelGrid[j, i].Worthiness;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// We go through the reachable cells from the current cell p_LevelCell and we find the one
        /// which has the highest worthiness
        /// </summary>
        /// <param name="p_LevelCell"></param>
        /// <returns></returns>
        private static int CalculateMaxWorthinessFromCell(LevelCell p_LevelCell)
        {
            int MaxWorthiness = 0;
            for (int i = p_LevelCell.LineIndex; i < m_LevelGridSize; i++)
            {
                for (int j = p_LevelCell.ColumnIndex; j < m_LevelGridSize; j++)
                {
                    if (m_LevelGrid[i, j].HasChamp)
                    {
                        if (m_LevelGrid[i, j].Worthiness > MaxWorthiness)
                        {
                            MaxWorthiness = m_LevelGrid[i, j].Worthiness;
                        }
                    }
                }
            }
            return MaxWorthiness;
        }

        /// <summary>
        /// Here, we compute the path to get to Peach to get the maximum number of mushrooms
        /// </summary>
        /// <param name="p_LevelCell"> The cell we are currently located in </param>
        private static void FindPathToPeach(LevelCell p_LevelCell)
        {
            // Dictionnary used to store the result from the FindNextMostWorthyMushroom function
            Dictionary<string, object> HashMap = new Dictionary<string, object>();

            // For each line, we run the FindNextMostWorthyMushroom function which figure out if the
            // next most worthy mushroom is there, i.e. if we have a mushroom with a worthy equals to
            // WorthinessIndex - 1
            for (int i = p_LevelCell.LineIndex; i < m_LevelGridSize; i++)
            {
                HashMap = FindNextMostWorthyMushroom(i, p_LevelCell);
                // We break if we found the next mushroom to avoid unecessary searches
                if (!(bool)HashMap["bool"])
                {
                    break;
                }
            }

            // We extract the next LevelCell from the Dictionnary
            LevelCell NextCase = (LevelCell)HashMap["nextCase"];

            // We add the horizontal cells to get to the NextCell in the path List
            for (int i = p_LevelCell.ColumnIndex + 1; i <= NextCase.ColumnIndex; i++)
            {
                m_Path.Add(m_LevelGrid[p_LevelCell.LineIndex, i]);
            }

            // We add the vertical cells to get to the NextCell in the path List
            for (int i = p_LevelCell.LineIndex + 1; i <= NextCase.LineIndex; i++)
            {
                m_Path.Add(m_LevelGrid[i, NextCase.ColumnIndex]);
            }

            // If the next cell contains Peach, we stop the recurrence
            if (NextCase.HasPeach)
            {
                return;
            }
            // Otherwise we recursively execute this algorithm of the remaining cells
            else
            {
                FindPathToPeach(NextCase);
            }
            return;
        }

        /// <summary>
        /// Here, We try to figure out if the next most worthy cell is in the line number p_LineIndex,
        /// i.e. if we have a mushroom with a worthy equals to WorthinessIndex - 1
        /// </summary>
        /// <param name="p_LineIndex"> The line we are considering </param>
        /// <param name="p_LevelCell"> The strating cell </param>
        /// <returns> A Dictionnary containing a boolean indicating if we found the next most worthy cell
        /// and if so a LevelCell which is the next cell to consider </returns>
        private static Dictionary<string, object> FindNextMostWorthyMushroom(int p_LineIndex, LevelCell p_LevelCell)
        {
            // Dictionnary used to store the result of the search
            Dictionary<string, object> HashMap = new Dictionary<string, object>();

            for (int j = p_LevelCell.ColumnIndex; j < m_LevelGridSize; j++)
            {
                // The next worthy cell will be the one which has a worthiness equals to WorthinessIndex - 1
                if (m_LevelGrid[p_LineIndex, j].Worthiness == WorthinessIndex - 1)
                {
                    LevelCell NextCase = null;

                    // If we found it and the new worthiness equals 1, is the last mushroom before finding Peach,
                    // so the next cell is the one of the bottom right with Peach
                    if (m_LevelGrid[p_LineIndex, j].Worthiness == 1)
                    {
                        NextCase = m_LevelGrid[m_LevelGridSize - 1, m_LevelGridSize - 1];
                    }
                    // otherwise, the next case is the one with the worthiness equal to WorthinessIndex - 1
                    else
                    {
                        NextCase = m_LevelGrid[p_LineIndex, j];
                        // We update the WorthinessIndex to its new value (WorthinessIndex - 1)
                        WorthinessIndex = m_LevelGrid[p_LineIndex, j].Worthiness;
                    }

                    // We add the needed information to the Disctionnary
                    HashMap.Add("bool", false);
                    HashMap.Add("nextCase", NextCase);
                    return HashMap;
                }
            }

            // We didin't find the next cell, we try with another line
            HashMap.Add("bool", true);
            return HashMap;
        }
    }
}
