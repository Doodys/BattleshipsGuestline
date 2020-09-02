using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Battleships.Data
{
    public interface IStaticData
    {
        List<TextBox> BattlefieldTextBoxesList { get; set; }
        List<Cell> Battlefield { get; set; }
        List<Battleship> BattleshipsList { get; set; }
        void CreateBattlefield();
        void CreateBattleships();
        List<TextBox> GatherAllTextBoxesFromBattlefield(Grid battlefield);
    }
}
