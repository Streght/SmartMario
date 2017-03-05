using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartMario
{
    /// <summary>
    /// Logique d'interaction pour LevelCell.xaml.
    /// </summary>
    public partial class LevelCell : UserControl
    {
        /// <summary>
        /// Line index of the cell.
        /// </summary>
        private int m_LineIndex;

        /// <summary>
        /// Column index of the cell.
        /// </summary>
        private int m_ColumnIndex;

        /// <summary>
        /// Worthiness of the cell.
        /// </summary>
        private int m_Worthiness = 0;

        /// <summary>
        /// Boolean to indicate if the cell contains Mario.
        /// </summary>
        private bool m_HasMario = false;

        /// <summary>
        /// Boolean to indicate if the cell contains Peach.
        /// </summary>
        private bool m_HasPeach = false;

        /// <summary>
        /// Boolean to indicate if the cell contains a mushroom.
        /// </summary>
        private bool m_HasChamp = false;

        /// <summary>
        /// Create a new LevelCell.
        /// </summary>
        public LevelCell()
        {
            InitializeComponent();
        }

        #region Getters / Setters

        /// <summary>
        /// Get / set the line index of the cell.
        /// </summary>
        public int LineIndex
        {
            get
            {
                return m_LineIndex;
            }

            set
            {
                m_LineIndex = value;
            }
        }

        /// <summary>
        /// Get / set the column index of the cell.
        /// </summary>
        public int ColumnIndex
        {
            get
            {
                return m_ColumnIndex;
            }

            set
            {
                m_ColumnIndex = value;
            }
        }

        /// <summary>
        /// Get / set the worthiness of the cell.
        /// </summary>
        public int Worthiness
        {
            get
            {
                return m_Worthiness;
            }

            set
            {
                m_Worthiness = value;
            }
        }

        /// <summary>
        /// Get / set if the cell has a mushroom.
        /// </summary>
        public bool HasChamp
        {
            get
            {
                return m_HasChamp;
            }

            set
            {
                m_HasChamp = value;
            }
        }

        /// <summary>
        /// Get / set if the cell has Peach.
        /// </summary>
        public bool HasPeach
        {
            get
            {
                return m_HasPeach;
            }

            set
            {
                m_HasPeach = value;
            }
        }

        /// <summary>
        /// Get / set if the cell has Mario.
        /// </summary>
        public bool HasMario
        {
            get
            {
                return m_HasMario;
            }

            set
            {
                m_HasMario = value;
            }
        }

        #endregion

        /// <summary>
        /// Add the picture of Mario in the cell.
        /// </summary>
        public void AddMario()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MarioPic.jpg", UriKind.Relative));
            cellImage.Source = image;
            m_HasMario = true;
        }

        /// <summary>
        /// Add the picture of Peach in the cell.
        /// </summary>
        public void AddPeach()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/PeachPic.png", UriKind.Relative));
            cellImage.Source = image;
            HasPeach = true;
        }

        /// <summary>
        /// Add the picture of a mushroom in the cell.
        /// </summary>
        public void AddChamp()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MushPic.png", UriKind.Relative));
            cellImage.Source = image;
            HasChamp = true;
        }

        /// <summary>
        /// Clear the cell of its content.
        /// </summary>
        public void ClearCell()
        {
            cellImage.Source = null;
            HasChamp = m_HasMario = HasPeach = false;
        }

        /// <summary>
        /// Gets the cell located directly to the right.
        /// </summary>
        /// <returns> The cell on the right, or null if we are located 
        /// on the right edge of the level. </returns>
        public LevelCell getRightCell(MainWindow p_Window)
        {
            // If the cell if on the right edge, there is no neighbor to return.
            if (ColumnIndex == p_Window.GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix.
            return p_Window.LevelCellMatrix[LineIndex, ColumnIndex + 1];
        }

        /// <summary>
        /// Gets the cell located directly below.
        /// </summary>
        /// <returns> The cell below, or null if we are located 
        /// on the bottom edge of the level. </returns>
        public LevelCell getBottomCell(MainWindow p_Window)
        {
            // If the cell if on the right edge, there is no neighbor to return.
            if (LineIndex == p_Window.GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix.
            return p_Window.LevelCellMatrix[LineIndex + 1, ColumnIndex];
        }
    }
}
