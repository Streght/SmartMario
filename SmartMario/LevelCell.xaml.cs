using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartMario
{
    /// <summary>
    /// Logique d'interaction pour LevelCell.xaml
    /// </summary>
    public partial class LevelCell : UserControl
    {
        private int m_LineNumber;
        private int m_ColumnNumber;
        private int m_Worthiness = 0;

        private bool m_HasMario = false;
        private bool m_HasPeach = false;
        private bool m_HasChamp = false;

        public LevelCell()
        {
            InitializeComponent();
        }

        #region Getters / Setters

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

        public int LineIndex
        {
            get
            {
                return m_LineNumber;
            }
            set
            {
                m_LineNumber = value;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return m_ColumnNumber;
            }
            set
            {
                m_ColumnNumber = value;
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

        #endregion 

        public void AddMario()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MarioPic.jpg", UriKind.Relative));
            cellImage.Source = image;
            HasMario = true;
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
            HasChamp = HasMario = HasPeach = false;
        }

        /// <summary>
        /// Gets the cell located directly to the right 
        /// </summary>
        /// <returns>The cell on the right, or null if we are located 
        ///          on the right edge of the level</returns>
        public LevelCell getRightCell(MainWindow window)
        {
            // If the cell if on the right edge, there is no neighbor to return
            if (ColumnIndex == MainWindow.m_GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix
            return window.LevelCellMatrix[LineIndex,ColumnIndex + 1];
        }

        /// <summary>
        /// Gets the cell located directly below 
        /// </summary>
        /// <returns>The cell below, or null if we are located 
        ///          on the bottom edge of the level</returns>
        public LevelCell getBottomCell(MainWindow window)
        {
            // If the cell if on the right edge, there is no neighbor to return
            if (LineIndex == MainWindow.m_GridSize - 1)
            {
                return null;
            }
            // Otherwise the right neighbor is next on the line index matrix
            return window.LevelCellMatrix[LineIndex + 1, ColumnIndex];
        }
    }
}
