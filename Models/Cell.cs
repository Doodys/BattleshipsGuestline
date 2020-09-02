using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Battleships.Models
{
    public class Cell
    {
        public TextBox Battlecell { get; set; }
        public string Naming { get; set; }
        public bool IsTaken { get; set; }
        public bool IsDestroyed { get; set; }
    }
}
