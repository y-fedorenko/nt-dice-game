using Org.BouncyCastle.Bcpg;
using System.Reflection.Metadata;
using ConsoleTables;
using System.Reflection.Metadata.Ecma335;
using Org.BouncyCastle.Security;
using System.Text;
namespace DiceGame
{
    internal class Program
    {
        private static List<Dice> Dices = new List<Dice>();

        static void Main(string[] args)
        {
            Dices = GetDices(args); 
            ShowMainMenu(new GameParams(Dices));
        }

        public static List<Dice> GetDices(string[] args)
        {
            List<Dice> Dices = new List<Dice>();
            foreach (string argument in args)
            {
                Dices.Add(Dice.Create(argument));
            }
            if (Dices.Count < 3)
            {
                Console.WriteLine("Error: Please enter at least 3 or more sets of 6 comma-separated numbers");
                Environment.Exit(0);
            }
            return Dices;
        }

        public static void ShowMainMenu(GameParams Parameters)
        {
            Console.WriteLine("Hello, player, welcome to non-transitive dice game!");
            ShowLotMenu(Parameters);
            if (Parameters.ComputerMovesFirst)
            {
                ShowComputerDiceSelect(Parameters);
                ShowPlayerDiceSelect(Parameters);
                ShowComputerMoveMenu(Parameters);
                ShowPlayerMoveMenu(Parameters);

            }
            else
            {
                ShowPlayerDiceSelect(Parameters);
                ShowComputerDiceSelect(Parameters);
                ShowPlayerMoveMenu(Parameters);
                ShowComputerMoveMenu(Parameters);
            }
            ShowWinner(Parameters);
        }

        public static int GetPlayerInputNumber(int range, bool isShowingRequired = true)
        {
            for (int i = 0; i < range && isShowingRequired; i++)
                Console.WriteLine($"{i} - {i}");
            Console.WriteLine("x - Exit");
            Console.WriteLine("? - HELP");
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'X':
                    case 'x':
                        Environment.Exit(0);
                        break;
                    case '/':
                    case '?':
                        ShowHelpInfo();
                        break;
                    default:
                        if (char.IsDigit(key.KeyChar))
                        {
                            int index = key.KeyChar - '0';
                            if (index >= 0 && index < range)
                            {
                                return index;
                            }
                        }
                        break;
                }
            }
        }

        public static void ShowLotMenu(GameParams Parameters)
        {
            Console.WriteLine("Let's determine who makes the first move.\nI selected a random value in the range 0..1");
            SecretNumber LotDraw = new SecretNumber(new Random().Next(0, 2));
            Console.WriteLine($"(HMAC = {LotDraw.HMAC})");
            Console.WriteLine("Try to Guess My selection");
            int PlayerInput = GetPlayerInputNumber(2);
            Parameters.ComputerMovesFirst = PlayerInput == LotDraw.Number ? false : true;
            Console.WriteLine($"Your selection is - {PlayerInput}\nMy selection - {LotDraw.Number} (KEY - {LotDraw.Key})");
        }

        public static void ShowComputerDiceSelect(GameParams Parameters)
        {
            Parameters.ComputerDice = Parameters.GetDice(new Random().Next(0, Parameters.RemainingDices.Count));
            Console.WriteLine(Parameters.ComputerMovesFirst ? $"I willmake a first move and select [{Parameters.ComputerDice}] dice." : $"In my turn, I selected [{Parameters.ComputerDice}] dice.");

        }

        public static void ShowPlayerDiceSelect(GameParams Parameters)
        {
            Console.WriteLine(Parameters.ComputerMovesFirst ? "Now your turn to select your dice:" : "You select your dice first");
            Parameters.ListAvailableDices();
            int PlayerInput = GetPlayerInputNumber(Parameters.RemainingDices.Count, false);
            Parameters.PlayerDice = Parameters.GetDice(PlayerInput);
            Console.WriteLine($"You have selected [{Parameters.PlayerDice}].");
        }
        public static void ShowPlayerMoveMenu(GameParams Parameters)
        {
            Console.WriteLine("It is time for your throw");
            SecretNumber RandomValue = new SecretNumber(new Random().Next(0, 6));
            Console.WriteLine($"I selected a random value in range of 0...5 (HMAC={RandomValue.HMAC}).\nAdd your number modulo 6.");
            int PlayerInput = GetPlayerInputNumber(6);
            int index = (RandomValue.Number + PlayerInput) % 6;
            Parameters.PlayerResult = Parameters.PlayerDice.FaceValues[index];
            Console.WriteLine($"Your selection: {PlayerInput}\nMy number is {RandomValue.Number} (KEY={RandomValue.Key})");
            Console.WriteLine($"The result is {RandomValue.Number} + {PlayerInput} = {index} (mod 6).");
            Console.WriteLine($"Your trow is {Parameters.PlayerResult}");
        }

        public static void ShowComputerMoveMenu(GameParams Parameters)
        {
            Console.WriteLine("It is time for my throw");
            SecretNumber RandomValue = new SecretNumber(new Random().Next(0, 6));
            Console.WriteLine($"I selected a random value in range of 0...5 (HMAC={RandomValue.HMAC}).\nAdd your number modulo 6.");
            int PlayerInput = GetPlayerInputNumber(6);
            int index = (RandomValue.Number + PlayerInput) % 6;
            Parameters.ComputerResult = Parameters.ComputerDice.FaceValues[index];
            Console.WriteLine($"Your selection: {PlayerInput}\nMy number is {RandomValue.Number} (KEY={RandomValue.Key})");
            Console.WriteLine($"The result is {RandomValue.Number} + {PlayerInput} = {index} (mod 6).");
            Console.WriteLine($"My trow is {Parameters.ComputerResult}");
        }

        public static void ShowWinner(GameParams Parameters)
        {
            if (Parameters.PlayerResult == Parameters.ComputerResult) Console.WriteLine("We have a DRAW!");
            else Console.WriteLine(Parameters.PlayerResult > Parameters.ComputerResult ? $"You win {Parameters.PlayerResult} > {Parameters.ComputerResult}" :
                $"I win {Parameters.ComputerResult} > {Parameters.PlayerResult}");
        }

        public static void ShowHelpInfo()
        {
            var headers = new List<string> { "User dice v" }; 
            headers.AddRange(Dices.Select(dice => dice.ToString()));
            var table = new ConsoleTable(headers.ToArray());
            for (int i = 0; i < Dices.Count; i++)
            {
                var row = new List<object> { Dices[i].ToString() };
                for (int j = 0; j < Dices.Count; j++)
                {
                    decimal winChance = Dice.GetWinChances(Dices[i], Dices[j]);
                    string value = (i == j) ? "-" : winChance.ToString("0.00");
                    row.Add(value);
                }
                table.AddRow(row.ToArray());
            }
            Console.WriteLine("Probability of the win for the user:");
            table.Write(Format.Alternative);
        }
    }
}
