using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartMario
{
    /// <summary>
    /// Logique d'interaction pour LevelCell.xaml
    /// </summary>
    public partial class LevelCell : UserControl
    {
        private int m_LineIndex;
        private int m_ColumnIndex;
        private int m_Worthiness = 0;

        private bool m_HasMario = false;
        private bool m_HasPeach = false;
        private bool m_HasChamp = false;

        public LevelCell()
        {
            InitializeComponent();
        }

        #region Getters / Setters

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

        public void AddMario()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MarioPic.jpg", UriKind.Relative));
            cellImage.Source = image;
            m_HasMario = true;
        }

        public void AddPeach()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/PeachPic.png", UriKind.Relative));
            cellImage.Source = image;
            HasPeach = true;
        }

        public void AddChamp()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MushPic.png", UriKind.Relative));
            cellImage.Source = image;
            HasChamp = true;
        }

        public void ClearCell()
        {
            cellImage.Source = null;
            HasChamp = m_HasMario = HasPeach = false;
        }

        /// <summary>
        /// Gets the cell located directly to the right 
        /// </summary>
        /// <returns>The cell on the right, or null if we are located 
        ///          on the right edge of the level</returns>
        public LevelCell getRightCell(MainWindow window)
        {
            // If the cell if on the right edge, there is no neighbor to return
            if (ColumnIndex == window.GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix
            return window.LevelCellMatrix[LineIndex, ColumnIndex + 1];
        }

        /// <summary>
        /// Gets the cell located directly below 
        /// </summary>
        /// <returns>The cell below, or null if we are located 
        ///          on the bottom edge of the level</returns>
        public LevelCell getBottomCell(MainWindow window)
        {
            // If the cell if on the right edge, there is no neighbor to return
            if (LineIndex == window.GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix
            return window.LevelCellMatrix[LineIndex + 1, ColumnIndex];
        }
    }
}
