using Battleships.Data;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace Battleships.FormsLogic
{
    public class MainWindowLogic
    {
        private readonly Regex regexPattern = new Regex(@"^[A-J]{1}(10|[1-9]{1})$");
        private StaticData _staticData;
        private StringBuilder sbMessage;

        public MainWindowLogic(StaticData staticData)
        {
            _staticData = staticData;
        }

        public string bInputLogic(string tbInputText)
        {
            if (!string.IsNullOrEmpty(tbInputText))
            {
                tbInputText = tbInputText.ToUpper();

                if (regexPattern.IsMatch(tbInputText))
                {
                    string message = CheckIfDestroyed(tbInputText);

                    if (!string.IsNullOrEmpty(message))
                        return message;
                    else
                        throw new Exception("Miss!");
                }
                else
                    throw new Exception("Wrong input format!"); //HGFE
            }
            else
                throw new Exception("Input cannot be empty!");
        }

        public string CheckIfDestroyed(string field)
        {
            sbMessage = new StringBuilder();

            if (_staticData.BattleshipsList.Any(bl => bl.Placement.Any(p => p.Naming.Equals(field) && p.IsDestroyed.Equals(false))))
            {
                var attackedShip = _staticData.BattleshipsList
                    .Where(a => a.Placement
                    .Any(p => p.Naming.Equals(field)))
                    .FirstOrDefault();

                var attackedField = attackedShip.Placement
                    .Where(p => p.Naming.Equals(field))
                    .FirstOrDefault();

                var battlefieldCell = _staticData.Battlefield
                    .Where(b => b.Naming.Equals(field))
                    .FirstOrDefault();

                attackedField.IsDestroyed = true;
                battlefieldCell.IsDestroyed = true;

                sbMessage.Append($"You attacked battleship {attackedShip.Width} cells long");
            }

            if (_staticData.BattleshipsList.Any(bl => bl.Placement.All(p => p.IsDestroyed.Equals(true))))
            {
                sbMessage.Append($" and destroyed it!");

                _staticData.BattleshipsList.Remove
                    (
                    _staticData.BattleshipsList
                    .Where(a => a.Placement
                    .Any(p => p.Naming.Equals(field)))
                    .FirstOrDefault()
                    );
            }

            if (!string.IsNullOrEmpty(sbMessage.ToString()))
                return sbMessage.ToString();
            else
                return string.Empty;
        }
    }
}

