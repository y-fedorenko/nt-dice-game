using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    class GameParams
    {
        public List<Dice> Dices { get;}
        public List<Dice> RemainingDices { get; set; }
        public bool ComputerMovesFirst { get; set; }
        public Dice ComputerDice { get; set; }
        public Dice PlayerDice { get; set; }
        public int ComputerResult {  get; set; }
        public int PlayerResult { get; set; }  

        public GameParams (List<Dice> Dices)
        {
            this.Dices = Dices;
            RemainingDices = new List<Dice>(Dices.Count);
            Dices.ForEach(d => 
                RemainingDices.Add(d));
        }

        public void ListAvailableDices()
        {
            for(int i = 0; i < RemainingDices.Count; i++)
            {
                Console.WriteLine($"{i} - {RemainingDices[i]}");
            }
        }
   
        public Dice GetDice(int index)
        {
            Dice d = RemainingDices[index];
            RemainingDices.Remove(RemainingDices[index]);
            return d;
        }
        public void ShowWinChancesTable()
        {
            //...
        }
    }
}
