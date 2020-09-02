using Battleships.Data;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BattleshipsLogic
{
    public class BattleshipsPlacement
    {
        public List<Cell> chosenPlacement;

        private StaticData _staticData;
        private Random rand = new Random();

        private const string VerticalPlacement = "Vertical";
        private const string HorizontalPlacement = "Horizontal";
        private const string BackTurn = "Back";
        private const string FrontTurn = "Front";

        public BattleshipsPlacement(StaticData staticData)
        {
            _staticData = staticData;
        }

        public void ChooseBattleshipPlacement()
        {
            foreach (var ship in _staticData.BattleshipsList)
            {
                bool repeatOnWrongCell = true;

                //why did I choose "while" approach instead of changing Direction/Turn of the ship if some cells are taken
                //and then, after checking all 4 ways, eventually choose new starting cell?
                //it's faster and less code-consumming (spaghetti only on my plate, please)
                while (repeatOnWrongCell)
                {
                    var chosenCell = ChooseStartingCell();
                    var chosenIndex = _staticData.Battlefield.IndexOf(chosenCell);

                    ship.Settings = new BattleshipPlacementSettings()
                    {
                        Direction = HorizontalOrVertical(),
                        Turn = FrontOrBack()
                    };

                    if (CheckCellsForFullWidth(ship, chosenCell, chosenIndex))
                    {
                        ship.Placement = chosenPlacement;

                        foreach (var staticField in _staticData.Battlefield
                            .SelectMany(staticField => chosenPlacement
                            .Where(chosenField => chosenField.Naming.Equals(staticField.Naming))
                            .Select(chosenField => staticField)))
                        {
                            staticField.IsTaken = true;
                        }

                        repeatOnWrongCell = false;
                    }
                }
            }
        }

        public bool CheckCellsForFullWidth(Battleship ship, Cell chosenCell, int chosenIndex)
        {
            try
            {
                var shipDir = ship.Settings.Direction;
                var shipTurn = ship.Settings.Turn;

                Cell nextCell = new Cell();

                chosenCell.IsTaken = true;
                chosenPlacement = new List<Cell>() { chosenCell };

                for (int i = 1; i < ship.Width; i++)
                {
                    var currCell = _staticData.Battlefield[chosenIndex];

                    if (shipDir.Equals(HorizontalPlacement))
                    {
                        if (shipTurn.Equals(FrontTurn))
                        {
                            nextCell = _staticData.Battlefield[chosenIndex + 1];
                            chosenIndex++;
                        }
                        else
                        {
                            nextCell = _staticData.Battlefield[chosenIndex - 1];
                            chosenIndex--;
                        }

                        if(!currCell.Naming[0].Equals(nextCell.Naming[0]))
                            throw new Exception();
                    }
                    else
                    {
                        if (shipTurn.Equals(FrontTurn))
                        {
                            nextCell = _staticData.Battlefield[chosenIndex - 10];
                            chosenIndex -= 10;
                        }
                        else
                        {
                            nextCell = _staticData.Battlefield[chosenIndex + 10];
                            chosenIndex += 10;
                        }

                        if (currCell.Naming[0].Equals(nextCell.Naming[0]))
                            throw new Exception();
                    }

                    if (chosenPlacement.Contains(nextCell))
                        throw new Exception();

                    if (!ValidateNextCellsAvailability(nextCell))
                        throw new Exception();

                    nextCell.IsTaken = true;
                    chosenPlacement.Add(nextCell);
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Cell ChooseStartingCell()
        {
            var chosenIndex = rand.Next(0, 100);
            var chosenCell = _staticData.Battlefield[chosenIndex];

            if (!ValidateCellAvailability(chosenCell))
                ChooseStartingCell();

            return chosenCell;
        }

        public bool ValidateCellAvailability(Cell cell)
        {
            if (cell.IsDestroyed)
                throw new Exception("Chosen cell is already destroyed.");

            if (cell.IsTaken)
                throw new Exception("Chosen cell is already taken.");

            return true;
        }

        public bool ValidateNextCellsAvailability(Cell cell)
        {
            if (ValidateCellAvailability(cell))
                return true;

            return false;
        }

        public string FrontOrBack()
        {
            return rand.Next(0, 2).Equals(0) ? FrontTurn : BackTurn;
        }

        public string HorizontalOrVertical()
        {
            return rand.Next(0, 2).Equals(0) ? HorizontalPlacement : VerticalPlacement;
        }
    }
}
