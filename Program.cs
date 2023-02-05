using System;
using System.Collections;
using System.Threading; //using Superpowers;

namespace diceThroneAR
{
    class Program
    {
        static void Main(string[] args)
        {
            int charSelect; charSelect = GameFlow.characterSelect();
            //Player 1 chooses their character upon launching the game.
            Character player1 = new Character("1", CharacterInfo.names[charSelect],
                CharacterInfo.introQuips[charSelect]);
            player1.statusEffects.AddRange(CharacterInfo.generateStatusEffects(charSelect));
            player1.cards.AddRange(CharacterInfo.generateDeck(charSelect));
            player1.moves.AddRange(CharacterInfo.generateMoves(charSelect));
            player1.dice.AddRange(CharacterInfo.generateDice(charSelect));
            player1.IntroCharacter();
            //Player 1 is built and introduced to the user.
            GameFlow.gameModeSelect();
            //Player 1 chooses from KotH, a heads up battle, or a team battle.
            GameFlow.gameMatchMaking(player1, GameFlow.player2);
            //Player 2 is generated and the game begins.

            /*Extras
             *Battleground.jaggedLesson(); 
             *Battleground.testGetAverage();*/
        }
    }
}