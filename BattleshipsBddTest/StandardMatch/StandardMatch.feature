Feature: Standard match
	Let's play a game

Scenario: Missed shot
	Given Actions are executed without exceptions
	And List of battleships is cleared and battlefield grid is defaulted
	And Created new ship with width "2" and placement on fields "A1,A2"

	Then Aim into cell "A3" should return "Miss!"

Scenario: Aimed shot
	Given Actions are executed without exceptions
	And List of battleships is cleared and battlefield grid is defaulted
	And Created new ship with width "2" and placement on fields "A1,A2"

	Then Aim into cell "A1" should return "You attacked battleship 2 cells long"

Scenario: Sunked ship
	Given Actions are executed without exceptions
	And List of battleships is cleared and battlefield grid is defaulted
	And Created new ship with width "2" and placement on fields "A1,A2"

	Then Aim into cells "A1,A2" should return "You attacked battleship 2 cells long and destroyed it!"

Scenario: Match won
	Given Actions are executed without exceptions
	And List of battleships is cleared and battlefield grid is defaulted
	And Created new ship with width "2" and placement on fields "A1,A2"

	Then Aim into cells "A1,A2" should return "You attacked battleship 2 cells long and destroyed it!"
	And Match should be won
