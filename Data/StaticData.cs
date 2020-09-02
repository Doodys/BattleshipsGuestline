using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Battleships.Data
{
    public class StaticData : IStaticData
    {
        public List<TextBox> BattlefieldTextBoxesList { get; set; }
        public List<Cell> Battlefield { get; set; }
        public List<Battleship> BattleshipsList { get; set; }

        public StaticData(Grid battlefield)
        {
            BattlefieldTextBoxesList = GatherAllTextBoxesFromBattlefield(battlefield);

            CreateBattlefield();
            CreateBattleships();
        }

        public void CreateBattlefield()
        {
            if (BattlefieldTextBoxesList != null)
            {
                int listCounter = 0, charCounter = 0;
                Battlefield = new List<Cell>();

                while (Battlefield.Count < BattlefieldTextBoxesList.Count)
                {
                    string charField = Convert.ToChar(65 + charCounter).ToString();

                    for (int i = 0; i < 10; i++, listCounter++)
                    {
                        Battlefield.Add
                            (
                                new Cell
                                {
                                    Battlecell = BattlefieldTextBoxesList[listCounter],
                                    Naming = $"{charField}{(i + 1)}",
                                    IsTaken = false,
                                    IsDestroyed = false
                                }
                            );
                    }
                    charCounter++;
                }
            }
            else
                throw new NullReferenceException(nameof(BattlefieldTextBoxesList));
        }


        public void CreateBattleships()
        {
            if (BattleshipsList == null || BattleshipsList.Count.Equals(0))
                BattleshipsList = new List<Battleship>()
                {
                    new Battleship
                    {
                        Width = 5,
                        Name = "5 masts"
                    },
                    new Battleship
                    {
                        Width = 4,
                        Name = "4 masts"
                    },
                    new Battleship
                    {
                        Width = 4,
                        Name = "4 masts"
                    }
                };
            else return;
        }

        public List<TextBox> GatherAllTextBoxesFromBattlefield(Grid battlefield)
        {
            return battlefield.Children.OfType<TextBox>().ToList();
        }
    }
}
