using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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
        private int m_GridSize = 6;

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

        /// <summary>
        /// Current score of Mario, gains +1 every time a mushroom is collected
        /// </summary>
        private int m_Score = 0;

        /// <summary>
        /// Timer used to keep track of the remaining time for choosing an action
        /// </summary>
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Timer used to diplay the remaining time (refreshes every 50 ms)
        /// </summary>
        private DispatcherTimer m_DisplayTimer = new DispatcherTimer();

        /// <summary>
        /// Datetime used to store the 
        /// </summary>
        private DateTime m_Start;

        /// <summary>
        /// Time to choose an action ()
        /// </summary>
        private TimeSpan m_Delay = new TimeSpan(0, 0, 5);

        public MainWindow()
        {
            InitializeComponent();

            // Timers setup
            m_DisplayTimer.Tick += new EventHandler(displayTimer_Tick);
            m_DispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            initWPF();
        }

        private void initWPF()
        {
            CreateLevel();

            PopulateLevel();

            // Compute the path to get the most mushrooms
            Pathfinding.ComputeMaxChampPath(m_LevelCellMatrix);
            // Display the maximum number of mushrooms possible to get
            nbMushroomsMaxText.Text = Pathfinding.GridMaximumMushroomsNumber.ToString();

            foreach (LevelCell cell in Pathfinding.Path)
            {
                //m_LevelCellMatrix[cell.LineIndex, cell.ColumnIndex].AddMario();
            }

            CreateGUI();

            InitialiseTimers();
        }

        #region Getter / Setter

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

        public int GridSize
        {
            get
            {
                return m_GridSize;
            }
        }

        #endregion

        /// <summary>
        /// Creates a level matrix with the size "m_GridSize" with a LevelCell in each cell
        /// </summary>
        private void CreateLevel()
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
        private void PopulateLevel()
        {
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        LevelCellMatrix[i, j].AddMario();
                        m_CellWithMario = LevelCellMatrix[i, j];
                    }
                    else if ((i == m_GridSize - 1) && (j == m_GridSize - 1))
                    {
                        LevelCellMatrix[i, j].AddPeach();
                    }
                    else if ((i == 0 && (j == 4)) |
                           (i == 1 && (j == 1) | (j == 3)) |
                           (i == 2 && (j == 3) | (j == 5)) |
                           (i == 3 && (j == 2) | (j == 5)) |
                           (i == 4 && (j == 3) | (j == 4)) |
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
        private void CreateGUI()
        {
            m_LevelGridGUI = new Grid();

            // Creates the desired number of rows and columns for the grid
            for (int i = 0; i < m_GridSize; i++)
            {
                m_LevelGridGUI.RowDefinitions.Add(new RowDefinition());
                m_LevelGridGUI.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    // Address the LevelCellMatrix[i,j] to the grid row 'i' and column 'j'
                    Grid.SetRow(m_LevelCellMatrix[i, j], i);
                    Grid.SetColumn(m_LevelCellMatrix[i, j], j);

                    m_LevelGridGUI.Children.Add(m_LevelCellMatrix[i, j]);
                }
            }

            Grid.SetRow(m_LevelGridGUI, 0);
            Grid.SetColumn(m_LevelGridGUI, 0);
            mainGrid.Children.Add(m_LevelGridGUI);
            Content = mainGrid;
        }

        /// <summary>
        /// Initialize the timers with their trigger value
        /// </summary>
        private void InitialiseTimers()
        {
            m_DispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            m_DisplayTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            m_Start = DateTime.Now;
            m_DisplayTimer.Start();
            m_DispatcherTimer.Start();
        }

        /// <summary>
        /// Update the current number of mushrooms
        /// </summary>
        private void EditTextDetails()
        {
            nbMushroomsText.Text = m_Score.ToString();
            Content = mainGrid;
        }

        /// <summary>
        /// Reset the timers (when the player clicks on a button)
        /// </summary>
        private void ResetTimers()
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();

            InitialiseTimers();
        }

        /// <summary>
        /// Update the current position of Mario, the current number of mushrooms and reset the timer
        /// </summary>
        /// <param name="nextCellToGo"> The next cell where Marion will go </param>
        private void RoundAndAroundAndAround(LevelCell nextCellToGo)
        {
            if (nextCellToGo != null)
            {
                if (nextCellToGo.HasChamp == true)
                {
                    m_Score++;
                    EditTextDetails();
                }
                nextCellToGo.AddMario();
                m_CellWithMario.ClearCell();
                m_CellWithMario = nextCellToGo;
            }
            else
            {
                //MessageBox.Show("Don't fly, you fool !");
            }

            EditTextDetails();
            ResetTimers();
        }

        #region Events

        /// <summary>
        /// Event called when the button for going right is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_GoRightButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = m_CellWithMario.getRightCell(this);

            RoundAndAroundAndAround(nextCellToGo);
        }

        /// <summary>
        /// Event called when the button for going down is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_GoDownButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = m_CellWithMario.getBottomCell(this);

            RoundAndAroundAndAround(nextCellToGo);
        }

        /// <summary>
        /// Event triggered when the displayTimer reaches 50 ms to refresh the time display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayTimer_Tick(object sender, EventArgs e)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            DateTime time = Convert.ToDateTime((m_Delay - (DateTime.Now - m_Start)).ToString());

            timerText.Text = "Temps restant : " + time.ToString("s.ff");
        }

        /// <summary>
        /// Event triggered when the 5 seconds timer reaches 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();
            MessageBox.Show("you loose !");

            initWPF();
        }

        #endregion
    }
}
