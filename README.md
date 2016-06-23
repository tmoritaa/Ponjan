# Ponjan
A simplified Mahjong game

## Controls
* Click on tiles in your hand to discard them on your turn.
* You can steal tiles and complete hands by pressing the button on the bottom right when they come up.

## Tile Types and Number
* 18 Red tiles: 9 Red0, 9 Red1
* 18 Blue tiles: 9 Blue0, 9 Blue1
* 18 Yellow tiles: 9 Yellow0, 9 Yellow1
* 16 White tiles: 4 White0, 4 White1, 4 White2, 4 White3
* 8 Dragon Tiles, 4 Dragon0, 4 Dragon1

## Basic Rules
### Setup
* Players shuffle deck and deal 8 tiles to each player.

### Turn Structure
* Player draws a tile.
* If hand of 9 tiles has 3 sets (3 of the same tile), and satisfies at least one hand combination, then the player may complete their hand.
* Otherwise, the player may discard a tile from their hand, or declare a Reach if the conditions are met (explained in separate section).
* When a tile is discarded, other players may steal that tile if they can make a set using that tile. If stolen, the set made with the stolen tile must be displayed to the other players.

### Game End
* Game ends when either a player completes their hand, or there are no more tiles to draw from the deck.

### Scoring
* When a player completes their hand, a score is calculated based on the hand combinations that the hand accomplishes.
* If the final tile that completed the hand was drawn, then the other players get a score penalty based on the score of the completed hand divided by the number of players - 1.
* If the final tile was stolen, then the player that got stolen gets a score penalty based on the score of the completed hand.
* The actual score added and subtracted from a player is a bit confusing, but follows the following formula:
  * If total points of hand is below 4, score is 2^(points-1).
  * Otherwise, score is 4 * ((points / 2) + 1) + 4.

### Reaching
* Reaching is an action that can be performed if the player's hand is one tile away from completion. 
* Once a player performs a reach, they may not stop reaching.
* The benefits of reaching are that it adds a point to the hand, and also counts as satisfying a hand combination.
* The problem with reaching is that the player may no longer discard any tiles except the one that is drawn. The player can also not steal a tile unless it completes their hand.

### Notes
* Normally, multiple games are played and the total score at the end of all the games are used to determine the winner.
* When playing multiple games, the person that starts the game should be rotated in CW order.

## Hand Combinations
### Reach (1 point)
* The player has reached before completing their hand.

### Ippatsu (1 point)
* The hand was completed before discarding an additional tile after performing a reach and discarding a tile.
* This hand combination is also cancelled if a player steals a tile.

### All Same Id (1 point)
* All tiles in hand have the same id

### Dragon (1 point) [AI not yet implemented]
* The hand has a set of dragon tiles

### Two Identical (1 point) [AI not yet implemented]
* There are two sets in hand that are comprised of the same tile.

### No Steals (1 point)
* The hand was completed without stealing any tiles.

### Complete With Draw (1 point)
* The hand was completed by drawing a tile

### Three Color (2 point)
* The 3 sets in hand are comprised of one set each, discluding white/dragon tiles.

### One Color and Others (2 points)
* The tiles in hand are either comprised of one color, or white/dragon tiles.

### All Same Color (3 points)
* All tiles in hand have the same color

### All Same (12 points)
* All tiles in hand are the same tile
