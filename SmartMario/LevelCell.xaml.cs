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
        private int m_iLineNumber;
        private int m_iColumnNumber;
        private int m_iWorthiness = 0;

        private bool m_bHasMario = false;
        private bool m_bHasPeach = false;
        private bool m_bHasChamp = false;

        public LevelCell()
        {
            InitializeComponent();
        }

        public bool HasMario
        {
            get
            {
                return m_bHasMario;
            }
            set
            {
                m_bHasMario = value;
            }
        }

        public bool HasPeach
        {
            get
            {
                return m_bHasPeach;
            }
            set
            {
                m_bHasPeach = value;
            }
        }

        public bool HasChamp
        {
            get
            {
                return m_bHasChamp;
            }
            set
            {
                m_bHasChamp = value;
            }
        }

        public int LineIndex
        {
            get
            {
                return m_iLineNumber;
            }
            set
            {
                m_iLineNumber = value;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return m_iColumnNumber;
            }
            set
            {
                m_iColumnNumber = value;
            }
        }

        public int Worthiness
        {
            get
            {
                return m_iWorthiness;
            }
            set
            {
                m_iWorthiness = value;
            }
        }

        public void AddMario()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MarioPic.jpg", UriKind.Relative));
            cellImage.Source = image;
        }

        public void AddPeach()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/PeachPic.png", UriKind.Relative));
            cellImage.Source = image;
        }

        public void AddChamp()
        {
            BitmapImage image = new BitmapImage(new Uri("Ressources/MushPic.png", UriKind.Relative));
            cellImage.Source = image;
        }


    }
}
