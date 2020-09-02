using Battleships.Data;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using System.Windows.Controls;
using Battleships.FormsLogic;
using Battleships.BattleshipsLogic;
using Battleships.Models;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleshipsBddTest
{
    public abstract class BaseTestSteps
    {
        protected StaticData _staticData;

        protected MainWindowLogic mainWindowLogic;
        protected BattleshipsPlacement battleshipsPlacement;
        protected ListView lvShip = new ListView();

        protected List<TextBox> gridElements = new List<TextBox>();
        protected Grid grid = new Grid();

        public BaseTestSteps(StaticData staticData)
        {
            _staticData = staticData;
        }

        [Given(@"Actions are executed")]
        public void ActionsAreExecuted()
        {
            GridPreparation();

            foreach (var textBox in gridElements)
            {
                grid.Children.Add(textBox);
            }

            //Load static data
            _staticData = new StaticData(grid);

            //Inject static data into BattleshipsPlacement and MainWindowLogic
            battleshipsPlacement = new BattleshipsPlacement(_staticData);
            mainWindowLogic = new MainWindowLogic(_staticData);

            //Choose placement for battleships on generated grid
            battleshipsPlacement.ChooseBattleshipPlacement();
        }

        [Given(@"Actions are executed without exceptions")]
        public void ActionsAreExecutedWithoutExceptions()
        {
            GridPreparation();

            foreach (var textBox in gridElements)
            {
                grid.Children.Add(textBox);
            }

            try
            {
                //Load static data
                _staticData = new StaticData(grid);

                //Inject static data into BattleshipsPlacement and MainWindowLogic
                battleshipsPlacement = new BattleshipsPlacement(_staticData);
                mainWindowLogic = new MainWindowLogic(_staticData);

                //Choose placement for battleships on generated grid
                battleshipsPlacement.ChooseBattleshipPlacement();
            }
            catch(Exception) { }           
        }

        [Given(@"List of battleships is cleared and battlefield grid is defaulted")]
        public void ClearBattleshipsListAndDefaultGrid()
        {
            _staticData.BattleshipsList.Clear();

            foreach(var field in _staticData.Battlefield)
            {
                field.IsTaken = false;
            }

            _staticData.BattleshipsList = new List<Battleship>();
        }

        [Given(@"Created new ship with width ""(.*)"" and placement on fields ""(.*)""")]
        public void CreateNewShip(string width, string fields)
        {
            var cellList = new List<Cell>();
            Random rnd = new Random();

            foreach (var cell in fields.Split(','))
            {
                cellList.Add(new Cell()
                {
                    Battlecell = new TextBox() { Name = cell },
                    Naming = cell,
                    IsTaken = true,
                    IsDestroyed = false
                });
            }

            _staticData.BattleshipsList.Add(
                new Battleship()
                {
                    Width = Convert.ToInt32(width),
                    Placement = cellList,
                    Name = rnd.Next(0, 100).ToString()
                });
        }

        [Then(@"Aim into cell ""(.*)"" should return ""(.*)""")]
        public void AimIntoCell(string cell, string output)
        {
            try
            {
                var message = mainWindowLogic.bInputLogic(cell);
                message.Should().Be(output);
            }
            catch(Exception ex)
            {
                ex.Message.Should().Be(output);
            }
        }

        [Then(@"Aim into cells ""(.*)"" should return ""(.*)""")]
        public void AimIntoCells(string cell, string output)
        {
            try
            {
                string message = string.Empty;

                foreach (var shot in cell.Split(','))
                {
                    message = mainWindowLogic.bInputLogic(shot);
                }
                
                message.Should().Be(output);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be(output);
            }
        }

        [Then(@"Match should be won")]
        public void CheckIfWon()
        {
            bool result = false;

            foreach (var ship in _staticData.BattleshipsList)
            {
                lvShip.Items.Add(ship.Name);
            }

            if (lvShip.Items.Count < 1)
            {
                result = true;
            }

            Assert.IsTrue(result);
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

                gridElements.Add
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
