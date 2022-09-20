using System; using System.Collections; using System.Threading; //using Superpowers;

namespace diceThroneAR
{
    class Program
    {
        static void Main(string[] args)
        {
            int charSelect; charSelect = GameFlow.characterSelect();
            Character player1 = new Character("1", CharacterInfo.names[charSelect], CharacterInfo.introQuips[charSelect]);
            player1.statusEffects.AddRange(CharacterInfo.generateStatusEffects(charSelect));
            player1.cards.AddRange(CharacterInfo.generateDeck(charSelect));
            player1.IntroCharacter();
            GameFlow.gameModeSelect();
            GameFlow.gameMatchMaking(player1, GameFlow.player2);
            //Battleground.jaggedLesson(); Battleground.testGetAverage();
        }
    }
}