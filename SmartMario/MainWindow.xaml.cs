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
using System.Windows.Threading;

namespace SmartMario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Number of rows (or columns) in the level
        /// </summary>
        public static int m_GridSize = 6;

        /// <summary>
        /// Graphical grid used to contain and display the LevelCells
        /// </summary>
        private Grid m_LevelGridGUI = new Grid();

        /// <summary>
        /// Matrix that contains all the LevelCell of the level
        /// </summary>
        private LevelCell[,] m_LevelCellMatrix = null;

        /// <summary>
        /// Cell on which Mario is currently standing
        /// </summary>
        private LevelCell m_CellWithMario;

        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
        private DispatcherTimer m_DisplayTimer = new DispatcherTimer();

        private DateTime start;

        public MainWindow()
        {
            InitializeComponent();

            CreateLevel();

            PopulateLevel();

            //Pathfinding.ComputeMaxChampPath(m_LevelCellMatrix);

            CreateGUI();

            // Timers setup
            m_DisplayTimer.Tick += new EventHandler(displayTimer_Tick);
            m_DispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            m_DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            m_DisplayTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            start = DateTime.Now;
            m_DisplayTimer.Start();
            m_DispatcherTimer.Start();
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            timerText.Text = Convert.ToString(DateTime.Now - start);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();
            MessageBox.Show("you loose !");
        }

        private void ResetTimer()
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();
            m_DisplayTimer.Interval = new TimeSpan(0, 0, 0, 50);
            m_DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            start = DateTime.Now;
            m_DispatcherTimer.Start();
            m_DisplayTimer.Start();
        }

        #region Getters / Setters

        //public int GridSize
        //{
        //    get
        //    {
        //        return m_GridSize;
        //    }
        //    set
        //    {
        //        m_GridSize = value;
        //    }
        //}

        public Grid LevelGridGUI
        {
            get
            {
                return m_LevelGridGUI;
            }
            set
            {
                m_LevelGridGUI = value;
            }
        }

        public LevelCell[,] LevelCellMatrix
        {
            get
            {
                return m_LevelCellMatrix;
            }
            set
            {
                m_LevelCellMatrix = value;
            }
        }

        public LevelCell CellWithMario
        {
            get
            {
                return m_CellWithMario;
            }
            set
            {
                m_CellWithMario = value;
            }
        }

        #endregion

        /// <summary>
        /// Creates a level matrix with the size "m_GridSize" with a LevelCell in each cell
        /// </summary>
        public void CreateLevel()
        {
            LevelCellMatrix = new LevelCell[m_GridSize, m_GridSize];

            // For every cell we have, we create a new LevelCell
            // and we address it to the LevelCell matrix
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    // Creates a new LevelCell and gives it the correct attributes for row and column
                    LevelCellMatrix[i, j] = new LevelCell();
                    LevelCellMatrix[i, j].LineIndex = i;
                    LevelCellMatrix[i, j].ColumnIndex = j;
                }
            }
        }

        /// <summary>
        /// Populates the matrix of cell that represents a level, and will later be displayed in the GUI
        /// </summary>
        public void PopulateLevel()
        {
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        LevelCellMatrix[i, j].AddMario();
                        CellWithMario = LevelCellMatrix[i, j];
                    }
                    else if ((i == m_GridSize - 1) && (j == m_GridSize - 1))
                    {
                        LevelCellMatrix[i, j].AddPeach();
                    }
                    else if ((i == 0 && (j == 4)) |
                           (i == 1 && (j == 1) | (j == 3)) |
                           (i == 2 && (j == 3) | (j == 5)) |
                           (i == 3 && (j == 2) | (j == 5)) |
                           (i == 5 && (j == 0) | (j == 4)))
                    {
                        LevelCellMatrix[i, j].AddChamp();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a grid with "GridSize" rows and columns, which contains
        /// all the level cells for display purpose
        /// </summary>
        public void CreateGUI()
        {
            LevelGridGUI = new Grid();

            // Creates the desired number of rows and columns for the grid
            for (int i = 0; i < m_GridSize; i++)
            {
                LevelGridGUI.RowDefinitions.Add(new RowDefinition());
                LevelGridGUI.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    // Address the LevelCellMatrix[i,j] to the grid row 'i' and column 'j'
                    Grid.SetRow(m_LevelCellMatrix[i, j], i);
                    Grid.SetColumn(m_LevelCellMatrix[i, j], j);

                    LevelGridGUI.Children.Add(m_LevelCellMatrix[i, j]);
                }
            }

            Grid.SetRow(LevelGridGUI, 0);
            Grid.SetColumn(LevelGridGUI, 0);
            mainGrid.Children.Add(LevelGridGUI);
            Content = mainGrid;
        }

        public void OneRound()
        {
            // Déroulement d'un tour de jeu, avec soit le déplacement par le joueur soit le timer qui finit
            // Update du GUI
        }

        #region Events

        /// <summary>
        /// Events called when the button for going right is clicked
        /// </summary>
        private void On_GoRightButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = CellWithMario.getRightCell(this);
            if (nextCellToGo != null)
            {
                nextCellToGo.AddMario();
                CellWithMario.ClearCell();
                CellWithMario = nextCellToGo;
            }
            else
            {
                MessageBox.Show("Don't fly, you fool !");
            }
        }

        /// <summary>
        /// Events called when the button for going down is clicked
        /// </summary>
        private void On_GoDownButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = CellWithMario.getBottomCell(this);
            if (nextCellToGo != null)
            {
                nextCellToGo.AddMario();
                CellWithMario.ClearCell();
                CellWithMario = nextCellToGo;
            }
            else
            {
                MessageBox.Show("Don't fly, you fool !");
            }
        }

        #endregion
    }
}
