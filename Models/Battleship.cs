using System.Collections.Generic;

namespace Battleships.Models
{
    public class Battleship
    {
        public int Width { get; set; }
        public List<Cell> Placement { get; set; }
        public BattleshipPlacementSettings Settings { get; set; }
        public string Name { get; set; }
    }
}
