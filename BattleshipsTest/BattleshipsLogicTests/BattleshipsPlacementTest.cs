using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Battleships.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;
using Battleships.Models;
using Battleships.BattleshipsLogic;
using System.Linq;

namespace BattleshipsTest.BattleshipsLogicTests
{
    [TestClass]
    public class BattleshipsPlacementTest
    {
        private StaticData _staticData;
        private BattleshipsPlacement _battleshipsPlacement;

        private List<TextBox> _gridElements = new List<TextBox>();
        private Grid _grid = new Grid();

        private const string VerticalPlacement = "Vertical";
        private const string HorizontalPlacement = "Horizontal";
        private const string BackTurn = "Back";
        private const string FrontTurn = "Front";

        private readonly List<string> Directions = new List<string>() { VerticalPlacement, HorizontalPlacement };
        private readonly List<string> Turns = new List<string>() { BackTurn, FrontTurn };

        [TestInitialize]
        public void SetUp()
        {
            GridPreparation();

            foreach (var textBox in _gridElements)
                _grid.Children.Add(textBox);

            _staticData = Substitute.For<StaticData>(_grid);
            _battleshipsPlacement = Substitute.For<BattleshipsPlacement>(_staticData);
        }

        [TestMethod]
        public void TestInitializationGrid_Check()
        {
            // Arrange => Act => Assert
            _gridElements.Count.Should().Be(100);
        }

        [TestMethod]
        public void HorizontalOrVertical_Test()
        {
            // Arrange => Act
            string result = _battleshipsPlacement.HorizontalOrVertical();

            // Assert
            Directions.Should().Contain(result);
        }

        [TestMethod]
        public void FrontOrBack_Test()
        {
            // Arrange => Act
            string result = _battleshipsPlacement.FrontOrBack();

            // Assert
            Turns.Should().Contain(result);
        }

        [TestMethod]
        public void ValidateCellAvailability_ReturnTrue()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = false,
                IsTaken = false
            };

            // Act
            bool result = _battleshipsPlacement.ValidateCellAvailability(cell);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Chosen cell is already taken.")]
        public void ValidateCellAvailability_CellIsTaken()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = false,
                IsTaken = true
            };

            // Act => Assert
            _battleshipsPlacement.ValidateCellAvailability(cell);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Chosen cell is already destroyed.")]
        public void ValidateCellAvailability_CellIsDestroyed()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = true,
                IsTaken = false
            };

            // Act => Assert
            _battleshipsPlacement.ValidateCellAvailability(cell);
        }

        [TestMethod]
        public void ValidateNextCellsAvailability_ReturnTrue()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = false,
                IsTaken = false
            };

            // Act
            bool result = _battleshipsPlacement.ValidateNextCellsAvailability(cell);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Chosen cell is already taken.")]
        public void ValidateNextCellsAvailability_CellIsTaken()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = false,
                IsTaken = true
            };

            // Act => Assert
            _battleshipsPlacement.ValidateNextCellsAvailability(cell);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Chosen cell is already destroyed.")]
        public void ValidateNextCellsAvailability_CellIsDestroyed()
        {
            // Arrange
            var cell = new Cell()
            {
                IsDestroyed = true,
                IsTaken = false
            };

            // Act => Assert
            _battleshipsPlacement.ValidateNextCellsAvailability(cell);
        }

        [TestMethod]
        public void ChooseStartingCell_Test()
        {
            // Arrange => Act
            var result = _battleshipsPlacement.ChooseStartingCell();

            // Assert
            result.Should().BeOfType<Cell>();
        }

        [TestMethod]
        public void CheckCellsForFullWidth_HorizontalFront()
        {
            // Arrange
            int counter = 1;

            var ship = new Battleship()
            {
                Width = 4,
                Settings = new BattleshipPlacementSettings()
                {
                    Direction = HorizontalPlacement,
                    Turn = FrontTurn
                }
            };

            var cell = _staticData.Battlefield.Where(c => c.Naming == "A1").FirstOrDefault();           
            var index = _staticData.Battlefield.IndexOf(cell);

            // Act
            bool result = _battleshipsPlacement.CheckCellsForFullWidth(ship, cell, index);

            // Assert
            Assert.IsTrue(result);
            
            foreach(var place in _battleshipsPlacement.chosenPlacement)
            {
                place.Naming.Should().Be($"A{counter}");
                Assert.IsTrue(place.IsTaken);

                counter++;
            }
        }

        [TestMethod]
        public void CheckCellsForFullWidth_HorizontalBack()
        {
            // Arrange
            int counter = 4;

            var ship = new Battleship()
            {
                Width = 4,
                Settings = new BattleshipPlacementSettings()
                {
                    Direction = HorizontalPlacement,
                    Turn = BackTurn
                }
            };

            var cell = _staticData.Battlefield.Where(c => c.Naming == "A4").FirstOrDefault();
            var index = _staticData.Battlefield.IndexOf(cell);

            // Act
            bool result = _battleshipsPlacement.CheckCellsForFullWidth(ship, cell, index);

            // Assert
            Assert.IsTrue(result);

            foreach (var place in _battleshipsPlacement.chosenPlacement)
            {
                place.Naming.Should().Be($"A{counter}");
                Assert.IsTrue(place.IsTaken);

                counter--;
            }
        }

        [TestMethod]
        public void CheckCellsForFullWidth_VerticalFront()
        {
            // Arrange
            int charCounter = 0;

            var ship = new Battleship()
            {
                Width = 4,
                Settings = new BattleshipPlacementSettings()
                {
                    Direction = VerticalPlacement,
                    Turn = FrontTurn
                }
            };

            var cell = _staticData.Battlefield.Where(c => c.Naming == "D4").FirstOrDefault();
            var index = _staticData.Battlefield.IndexOf(cell);

            // Act
            bool result = _battleshipsPlacement.CheckCellsForFullWidth(ship, cell, index);

            // Assert
            Assert.IsTrue(result);

            foreach (var place in _battleshipsPlacement.chosenPlacement)
            {
                string charField = Convert.ToChar(68 + charCounter).ToString();

                place.Naming.Should().Be($"{charField}4");
                Assert.IsTrue(place.IsTaken);

                charCounter--;
            }
        }

        [TestMethod]
        public void CheckCellsForFullWidth_VerticalBack()
        {
            // Arrange
            int charCounter = 0;

            var ship = new Battleship()
            {
                Width = 4,
                Settings = new BattleshipPlacementSettings()
                {
                    Direction = VerticalPlacement,
                    Turn = BackTurn
                }
            };

            var cell = _staticData.Battlefield.Where(c => c.Naming == "A4").FirstOrDefault();
            var index = _staticData.Battlefield.IndexOf(cell);

            // Act
            bool result = _battleshipsPlacement.CheckCellsForFullWidth(ship, cell, index);

            // Assert
            Assert.IsTrue(result);

            foreach (var place in _battleshipsPlacement.chosenPlacement)
            {
                string charField = Convert.ToChar(65 + charCounter).ToString();

                place.Naming.Should().Be($"{charField}4");
                Assert.IsTrue(place.IsTaken);

                charCounter++;
            }
        }

        private void GridPreparation()
        {
            int counter = 1, charCounter = 0;

            for(int i = 0; i < 100; i++)
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
    }
}
