using Battleships.BattleshipsLogic;
using Battleships.Data;
using Battleships.FormsLogic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Battleships
{
    public partial class MainWindow : Window
    {
        private StaticData _staticData;
        private MainWindowLogic mainWindowLogic;
        private BattleshipsPlacement battleshipsPlacement;

        private const string DeadCell = "#FF5F0B0B";
        private const string EmptyCell = "#FF818B93";

        public MainWindow()
        {
            InitializeComponent();

            _staticData = new StaticData(gBattlefield);

            battleshipsPlacement = new BattleshipsPlacement(_staticData);
            mainWindowLogic = new MainWindowLogic(_staticData);

            battleshipsPlacement.ChooseBattleshipPlacement();

            UpdateListView();
        }

        #region Events

        private void bInput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = mainWindowLogic.bInputLogic(tbInput.Text);
                MessageBox.Show(message);

                var colorOfDestruction = new BrushConverter();

                var attackedTextBox = _staticData.BattlefieldTextBoxesList
                    .Where(bf => bf.Name.Equals(tbInput.Text.ToUpper()))
                    .FirstOrDefault();

                attackedTextBox.Background = (Brush)colorOfDestruction.ConvertFrom(DeadCell);
                attackedTextBox.Text = string.Empty;

                UpdateListView();

                var theEndCheck = IsThisTheEnd();

                if (!string.IsNullOrEmpty(theEndCheck))
                {
                    MessageBox.Show(theEndCheck);
                }
            }
            catch (Exception ex)
            {
                var colorOfDestruction = new BrushConverter();
                var attackedTextBox = _staticData.BattlefieldTextBoxesList
                    .Where(bf => bf.Name.Equals(tbInput.Text.ToUpper()))
                    .FirstOrDefault();

                if (ex.Message.Equals("Miss!") && attackedTextBox.Background != (Brush)colorOfDestruction.ConvertFrom(DeadCell))
                {
                    attackedTextBox.Background = (Brush)colorOfDestruction.ConvertFrom(EmptyCell);
                }

                tbInput.Text = string.Empty;
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region State Updaters

        private void UpdateListView()
        {
            lvShips.Items.Clear();

            foreach (var ship in _staticData.BattleshipsList)
            {
                lvShips.Items.Add(ship.Name);
            }
        }

        private string IsThisTheEnd()
        {
            if(lvShips.Items.Count < 1)
            {
                bInput.IsEnabled = false;
                tbInput.IsEnabled = false;
                lvShips.IsEnabled = false;

                return "You won the game!";
            }

            return string.Empty;
        }

        #endregion
    }
}
