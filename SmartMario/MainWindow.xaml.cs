using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SmartMario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Number of rows (or columns) in the level.
        /// </summary>
        private int m_GridSize = -1;

        /// <summary>
        /// Graphical grid used to contain and display the LevelCells.
        /// </summary>
        private Grid m_LevelGridGUI = new Grid();

        /// <summary>
        /// Matrix that contains all the LevelCell of the level.
        /// </summary>
        private LevelCell[,] m_LevelCellMatrix = null;

        /// <summary>
        /// Cell on which Mario is currently standing.
        /// </summary>
        private LevelCell m_CellWithMario;

        /// <summary>
        /// Current score of Mario, gains +1 every time a mushroom is collected.
        /// </summary>
        private int m_Score = 0;

        /// <summary>
        /// Timer used to keep track of the remaining time for choosing an action.
        /// </summary>
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Timer used to diplay the remaining time (refreshes every 5 ms).
        /// </summary>
        private DispatcherTimer m_DisplayTimer = new DispatcherTimer();

        /// <summary>
        /// Datetime used to store the starting time of an action.
        /// </summary>
        private DateTime m_Start;

        /// <summary>
        /// Time to choose an action (5 seconds).
        /// </summary>
        private TimeSpan m_Delay = new TimeSpan(0, 0, 5);

        /// <summary>
        /// A list which contains the line numbers of the random generated mushrooms.
        /// </summary>
        List<int> line = new List<int>();

        /// <summary>
        /// A list which contains the column numbers of the random generated mushrooms.
        /// </summary>
        List<int> column = new List<int>();

        /// <summary>
        /// Create a new MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Read the GridSize from the .ini file.
            IniFile settings = new IniFile("settings.ini");
            m_GridSize = Convert.ToInt32(settings.Read("GridSize"));

            // Timers events setup.
            m_DisplayTimer.Tick += new EventHandler(displayTimer_Tick);
            m_DispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            initWPF();
        }

        /// <summary>
        /// Function with initializes the LevelCellMatrix, the WPF, computes the pathfinding
        /// and initializes the timers.
        /// </summary>
        private void initWPF()
        {
            m_Score = 0;
            EditTextDetails();

            line.Clear();
            column.Clear();

            CreateLevel();

            PopulateLevel();

            // Compute the path to get the most mushrooms.
            Pathfinding.ComputeMaxChampPath(m_LevelCellMatrix);

            // Display the maximum number of mushrooms possible to get.
            nbMushroomsMaxText.Text = Pathfinding.GridMaximumMushroomsNumber.ToString();

            CreateGUI();

            InitialiseTimers();
        }

        #region Getters / Setters

        /// <summary>
        /// Get the matrix of level cells.
        /// </summary>
        public LevelCell[,] LevelCellMatrix
        {
            get
            {
                return m_LevelCellMatrix;
            }
        }

        /// <summary>
        /// Get the size of the grid.
        /// </summary>
        public int GridSize
        {
            get
            {
                return m_GridSize;
            }
        }

        #endregion

        /// <summary>
        /// Creates a level matrix with the size "m_GridSize" with a LevelCell in each cell.
        /// </summary>
        private void CreateLevel()
        {
            m_LevelCellMatrix = new LevelCell[m_GridSize, m_GridSize];

            // For every cell we have, we create a new LevelCell
            // and we address it to the LevelCell matrix.
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    // Creates a new LevelCell and gives it the correct attributes for row and column.
                    LevelCellMatrix[i, j] = new LevelCell();
                    LevelCellMatrix[i, j].LineIndex = i;
                    LevelCellMatrix[i, j].ColumnIndex = j;
                }
            }
        }

        /// <summary>
        /// Populates the matrix of cell that represents a level, and will later be displayed in the GUI.
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
                }
            }

            Random random = new Random();

            // We randomly generate a number of coordinate couples equals to
            // (m_GridSize^2) / sqrt(m_GridSize). This allows us to have a reasonnable number
            // of mushrooms.
            for (int k = 0; k < (int)(Math.Pow(m_GridSize, 2) / Math.Sqrt(m_GridSize)); k++)
            {
                line.Add(random.Next(0, m_GridSize - 1));
                column.Add(random.Next(0, m_GridSize - 1));
            }

            for (int k = 0; k < (int)(Math.Pow(m_GridSize, 2) / Math.Sqrt(m_GridSize)); k++)
            {
                if (!(line[k] == 0 && column[k] == 0) && !(line[k] == m_GridSize - 1 && column[k] == m_GridSize - 1))
                    LevelCellMatrix[line[k], column[k]].AddChamp();
            }
        }

        /// <summary>
        /// Creates a grid with "GridSize" rows and columns, which contains
        /// all the level cells for display purpose.
        /// </summary>
        private void CreateGUI()
        {
            m_LevelGridGUI = new Grid();

            // Creates the desired number of rows and columns for the grid.
            for (int i = 0; i < m_GridSize; i++)
            {
                m_LevelGridGUI.RowDefinitions.Add(new RowDefinition());
                m_LevelGridGUI.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    // Address the LevelCellMatrix[i,j] to the grid row 'i' and column 'j'.
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
        /// Initialize the timers with their trigger value.
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
        /// Update the current number of mushrooms.
        /// </summary>
        private void EditTextDetails()
        {
            nbMushroomsText.Text = m_Score.ToString();
            Content = mainGrid;
        }

        /// <summary>
        /// Reset the timers (when the player clicks on a button).
        /// </summary>
        private void ResetTimers()
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();

            InitialiseTimers();
        }

        /// <summary>
        /// Update the current position of Mario, the current number of mushrooms and reset the timer.
        /// </summary>
        /// <param name="nextCellToGo"> The next cell where Mario will go. </param>
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
                if (nextCellToGo.HasPeach == true)
                {
                    m_DisplayTimer.Stop();
                    m_DispatcherTimer.Stop();

                    DisplayPath();
                    MessageBox.Show("OKIDOKI ! LET'S GO MAMAMIA !\nYou got " +
                        m_Score +
                        " mushrooms out of the "
                        + Pathfinding.GridMaximumMushroomsNumber +
                        " possible");

                    initWPF();
                }

                else
                {
                    ResetTimers();
                }
            }
        }

        /// <summary>
        /// Display the path to get the most mushrooms.
        /// </summary>
        public void DisplayPath()
        {
            // We put Mario back to the start.
            m_CellWithMario.ClearCell();
            LevelCellMatrix[0, 0].AddMario();
            LevelCellMatrix[m_GridSize - 1, m_GridSize - 1].AddPeach();

            // We put the mushrooms back on the grid.
            for (int k = 0; k < (int)(Math.Pow(m_GridSize, 2) / Math.Sqrt(m_GridSize)); k++)
            {
                if (!(line[k] == 0 && column[k] == 0) && !(line[k] == m_GridSize - 1 && column[k] == m_GridSize - 1))
                    LevelCellMatrix[line[k], column[k]].AddChamp();
            }

            // We put a red solid border on each cell.
            foreach (LevelCell cell in Pathfinding.Path)
            {
                m_LevelCellMatrix[cell.LineIndex, cell.ColumnIndex].BorderBrush = new SolidColorBrush(Colors.Red);
                m_LevelCellMatrix[cell.LineIndex, cell.ColumnIndex].BorderThickness = new Thickness(5, 5, 5, 5); ;
            }
        }

        #region Events

        /// <summary>
        /// Event called when the button for going right is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_GoRightButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = m_CellWithMario.getRightCell(this);

            RoundAndAroundAndAround(nextCellToGo);
        }

        /// <summary>
        /// Event called when the button for going down is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_GoDownButtonClick(object sender, RoutedEventArgs e)
        {
            LevelCell nextCellToGo = m_CellWithMario.getBottomCell(this);

            RoundAndAroundAndAround(nextCellToGo);
        }

        /// <summary>
        /// Event triggered when the displayTimer reaches 50 ms to refresh the time display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayTimer_Tick(object sender, EventArgs e)
        {
            DateTime time = Convert.ToDateTime((m_Delay - (DateTime.Now - m_Start)).ToString());

            timerText.Text = "Remaining time : " + time.ToString("s.ff") + " seconds";
        }

        /// <summary>
        /// Event triggered when the 5 seconds timer reaches 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            m_DisplayTimer.Stop();
            m_DispatcherTimer.Stop();

            DisplayPath();

            MessageBox.Show("Game over !\nYou got " +
                m_Score +
                " mushrooms out of the " +
                Pathfinding.GridMaximumMushroomsNumber +
                " possible");

            initWPF();
        }

        #endregion
    }
}
