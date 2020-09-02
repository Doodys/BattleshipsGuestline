using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Battleships.FormsLogic;
using Battleships.Data;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Controls;
using Battleships.Models;
using System.Linq;
using FluentAssertions;

namespace BattleshipsTest.FormsLogic
{
    [TestClass]
    public class MainWindowLogicTest
    {
        private string? testInput;
        private List<TextBox> _gridElements = new List<TextBox>();
        private Grid _grid = new Grid();

        private MainWindowLogic _mainWindowLogic;
        private StaticData _staticData;

        [TestInitialize]
        public void SetUp()
        {
            GridPreparation();

            foreach (var textBox in _gridElements)
                _grid.Children.Add(textBox);

            _staticData = Substitute.For<StaticData>(_grid);
            _mainWindowLogic = Substitute.For<MainWindowLogic>(_staticData);

            BattlefieldPreparation(_staticData);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Input cannot be empty!")]
        public void InputButton_NullString()
        {
            // Arrange
            testInput = null;

            // Act => Assert
            _mainWindowLogic.bInputLogic(testInput);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Input cannot be empty!")]
        public void InputButton_EmptyString()
        {
            // Arrange
            testInput = string.Empty;

            // Act => Assert
            _mainWindowLogic.bInputLogic(testInput);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Wrong input format!")]
        public void InputButton_WrongInputFormat_RandomString()
        {
            // Arrange
            testInput = "not_valid";

            // Act => Assert
            _mainWindowLogic.bInputLogic(testInput);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Wrong input format!")]
        public void InputButton_WrongInputFormat_CellDoesNotExist()
        {
            // Arrange
            testInput = "A11";

            // Act => Assert
            _mainWindowLogic.bInputLogic(testInput);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Miss!")]
        public void InputButton_MissedShot()
        {
            // Arrange
            testInput = "B1";

            // Act => Assert
            _mainWindowLogic.bInputLogic(testInput);
        }

        [TestMethod]
        public void CheckIfDestroyed_Miss()
        {
            // Arrange
            testInput = "B1";

            // Act
            var result = _mainWindowLogic.CheckIfDestroyed(testInput);

            //Assert
            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void CheckIfDestroyed_GoodShot()
        {            
            // Arrange
            testInput = "A1";

            // Act
            var result = _mainWindowLogic.CheckIfDestroyed(testInput);

            //Assert
            result.Should().Be("You attacked battleship 4 cells long");
        }

        [TestMethod]
        public void CheckIfDestroyed_LastShot()
        {
            // Arrange
            testInput = "A1";
            foreach (var cell in _staticData.BattleshipsList.SelectMany(ship => ship.Placement.Where(cell => cell.Naming != testInput)))
            {
                cell.IsDestroyed = true;
            }

            // Act
            var result = _mainWindowLogic.CheckIfDestroyed(testInput);

            //Assert
            result.Should().Be("You attacked battleship 4 cells long and destroyed it!");
        }

        private void GridPreparation()
        {
            int counter = 1, charCounter = 0;

            for (int i = 0; i < 100; i++)
            {
                if (counter > 10)
                {
                    counter = 1;
                    charCounter = 0;
                }

                string charField = Convert.ToChar(65 + charCounter).ToString();

                _gridElements.Add
                (
                    new TextBox()
                    {
                        Name = $"{charField}{(counter)}"
                    }
                );

                counter++;
                charCounter++;
            }
        }

        private void BattlefieldPreparation(StaticData staticData)
        {
            staticData.BattleshipsList = new List<Battleship>()
            {
                new Battleship()
                {
                    Width = 4,
                    Placement = new List<Cell>()
                    {
                        new Cell()
                        {
                            Battlecell = new TextBox() { Name = "A1" },
                            Naming = "A1",
                            IsTaken = true,
                            IsDestroyed = false
                        },
                        new Cell()
                        {
                            Battlecell = new TextBox() { Name = "A2" },
                            Naming = "A2",
                            IsTaken = true,
                            IsDestroyed = false
                        },
                        new Cell()
                        {
                            Battlecell = new TextBox() { Name = "A3" },
                            Naming = "A3",
                            IsTaken = true,
                            IsDestroyed = false
                        },
                        new Cell()
                        {
                            Battlecell = new TextBox() { Name = "A4" },
                            Naming = "A4",
                            IsTaken = true,
                            IsDestroyed = false
                        }
                    }
                }
            };

            foreach (var cell in staticData.Battlefield)
            {
                if (!staticData.BattleshipsList.Any(bl => bl.Placement.Any(p => p.Naming.Equals(cell.Naming))))
                {
                    cell.IsTaken = false;
                }
            }
        }
    }
}
