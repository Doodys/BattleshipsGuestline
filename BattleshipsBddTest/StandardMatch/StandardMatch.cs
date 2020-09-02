using Battleships.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BattleshipsBddTest.StandardMatch
{
    [Binding]
    [Scope(Feature = "Standard match")]
    public class StandardMatch : BaseTestSteps
    {
        public StandardMatch(StaticData staticData) : base(staticData) { }
    }
}
