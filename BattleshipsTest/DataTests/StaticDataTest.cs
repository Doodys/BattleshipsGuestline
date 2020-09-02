using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Battleships.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using FluentAssertions;
using Battleships.Models;
using System.Linq;

namespace BattleshipsTest.StaticDataTests
{

    [TestClass]
    public class StaticDataTest
    {
        StaticData _staticData;
        Grid _grid = new Grid();

        [TestInitialize]
        public void SetUp()
        {
            _staticData = Substitute.For<StaticData>(_grid);
        }

        [TestMethod]
        public void GatheringChildrenFromGrid_OnlyTextBoxes()
        {
            // Arrange
            var lChildren = new List<UIElement>()
            {
                new TextBox(),
                new TextBox(),
                new TextBox(),
                new TextBox(),
                new TextBox()
            };

            foreach (var textBox in lChildren)
                _grid.Children.Add(textBox);

            // Act
            _staticData.BattlefieldTextBoxesList = _staticData.GatherAllTextBoxesFromBattlefield(_grid);

            // Assert
            _staticData.BattlefieldTextBoxesList.Count.Should().Be(5);
        }

        [TestMethod]
        public void GatheringChildrenFromGrid_MixedChildren()
        {
            // Arrange
            var lChildren = new List<UIElement>()
            {
                new TextBox(),
                new TextBox(),
                new TextBox(),
                new TextBox(),
                new TextBlock()
            };

            foreach (var textBox in lChildren)
                _grid.Children.Add(textBox);

            // Act
            _staticData.BattlefieldTextBoxesList = _staticData.GatherAllTextBoxesFromBattlefield(_grid);

            // Assert
            _staticData.BattlefieldTextBoxesList.Count.Should().Be(4);
        }

        [TestMethod]
        public void GatheringChildrenFromGrid_NoTextBoxes()
        {
            // Arrange
            var lTbChildren = new List<UIElement>()
            {
                new TextBlock(),
                new TextBlock(),
                new TextBlock(),
                new TextBlock(),
                new TextBlock()
            };

            foreach (var textBox in lTbChildren)
                _grid.Children.Add(textBox);

            // Act
            _staticData.BattlefieldTextBoxesList = _staticData.GatherAllTextBoxesFromBattlefield(_grid);

            // Assert
            _staticData.BattlefieldTextBoxesList.Count.Should().Be(0);
        }

        [TestMethod]
        public void CreateBatllefield_NoException()
        {
            // Arrange
            _staticData.BattlefieldTextBoxesList = new List<TextBox>()
            {
                new TextBox() { Text = "1" },
                new TextBox() { Text = "2" },
                new TextBox() { Text = "3" },
                new TextBox() { Text = "4" },
                new TextBox() { Text = "5" },
                new TextBox() { Text = "6" },
                new TextBox() { Text = "7" },
                new TextBox() { Text = "8" },
                new TextBox() { Text = "9" },
                new TextBox() { Text = "10" }
            };

            _staticData.Battlefield = new List<Cell>();

            // Act
            _staticData.CreateBattlefield();

            // Assert
            _staticData.Battlefield.Count.Should().Be(10);

            for (int i = 0; i < _staticData.BattlefieldTextBoxesList.Count; i++)
                _staticData.Battlefield[i].Naming.Should().Be($"A{i + 1}");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateBatllefield_NullReferenceException()
        {
            // Arrange
            _staticData.BattlefieldTextBoxesList = null;

            // Act => Assert
            _staticData.CreateBattlefield();
        }

        [TestMethod]
        public void CreateBattleships_IsNull()
        {
            // Arrange
            _staticData.BattleshipsList = null;

            // Act
            _staticData.CreateBattleships();

            // Assert
            _staticData.BattleshipsList.Count.Should().Be(3);
        }

        [TestMethod]
        public void CreateBattleships_IsZeroLength()
        {
            // Arrange
            _staticData.BattleshipsList = new List<Battleships.Models.Battleship>();

            // Act
            _staticData.CreateBattleships();

            // Assert
            _staticData.BattleshipsList.Count.Should().Be(3);
        }

        [TestMethod]
        public void CreateBattleships_IsNotZeroLength()
        {
            // Arrange
            _staticData.BattleshipsList = new List<Battleships.Models.Battleship>() { new Battleships.Models.Battleship() };

            // Act
            _staticData.CreateBattleships();

            // Assert
            _staticData.BattleshipsList.Count.Should().Be(1);
        }
    }
}
