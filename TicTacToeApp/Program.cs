using System;
using System.Threading;
using System.Linq;
namespace TicTacToeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int aSecond = 1000; //(miliseconds)
            /***
            Before the game
            ***/

            Console.WriteLine("\n^v^ Welcome to Tic-Tac-Toe!!! ^v^\n\n");
            Console.WriteLine(TicTacToe.GameInstruction);
            Console.WriteLine("\n\n");
            Thread.Sleep(6*aSecond);

            Console.Write("Now, please choose your icon \"X\" or \"O\": ");
            char user = Convert.ToChar(Console.ReadLine()[0]);
            while (user!='X' && user!='O')
            {
                Console.WriteLine("Your icon is invalid!");
                Console.Write("Only receive \"X\" or \"O\": ");
                user = Convert.ToChar(Console.ReadLine()[0]);
            }
            Console.WriteLine("\n");

            TicTacToe game = new TicTacToe(user);
            
            Console.WriteLine("Thank you for choosing!");
            Console.WriteLine($"You will be {game.UserSymbol}");
            Console.WriteLine($"AI will be {game.AiSymbol}\n");
            Console.WriteLine();

            Console.WriteLine("Below is the layout of the grid: ");
            Console.WriteLine(TicTacToe.PrintGridLayout());
            Console.WriteLine();
            Thread.Sleep(4*aSecond);
            
            Console.WriteLine("For each of your turn, your task is to enter the square's number to denote that the spot is yours.");
            Thread.Sleep(4*aSecond);
            Console.WriteLine("You and AI will take turn to play until one of you win or when the whole grid has been occupied.\n");
            Thread.Sleep(4*aSecond);
            Console.WriteLine("The rule is simple and I believe that you know how to play it. Let's get started!!!@v@\n");
            Thread.Sleep(4*aSecond);

            /***
            During the game
            ***/
            Console.Write($"Who go first? You or AI? Enter {game.UserSymbol} if you want to go first and enter {game.AiSymbol} if you want AI to go first: ");
            char response = Console.ReadLine()[0];
            while (response!='X' && response!='O')
            {
                Console.WriteLine("Your icon is invalid!");
                Console.Write("Only receive \"X\" or \"O\": ");
                response= Convert.ToChar(Console.ReadLine()[0]);
            }

            bool userTurn = true;
            if (response==game.AiSymbol) userTurn = !userTurn;

            bool validIn;
            char winner;
            do 
            {
                if (userTurn)
                {
                    Console.WriteLine(String.Concat(Enumerable.Repeat("*", 100)));
                    Console.Write("Your turn! Enter a square's number: ");
                    do{
                        try{
                            game.UserPlay(Convert.ToInt32(Console.ReadLine()));
                            validIn=true;
                        }
                        catch(InvalidInputGameException e)
                        {
                            Console.WriteLine(e.Message);
                            validIn=false;
                            Console.Write("No worry! Enter another square's number: ");
                        }
                    } while (!validIn);
                    Console.WriteLine(game);
                    Console.WriteLine(String.Concat(Enumerable.Repeat("*", 100)));
                    Console.WriteLine();
                    Thread.Sleep(2*aSecond);
                    userTurn = !userTurn;
                }
                else
                {
                    Console.WriteLine(String.Concat(Enumerable.Repeat("*", 100)));
                    Console.WriteLine("AI's turn @<>@");
                    game.AiPlay();
                    Console.WriteLine(game);
                    Console.WriteLine(String.Concat(Enumerable.Repeat("*", 100)));
                    Console.WriteLine();
                    Thread.Sleep(2*aSecond);
                    userTurn = !userTurn;
                }

            }while (!game.GameOver(out winner));
            /***
            End of the game
            ***/
            Console.WriteLine("<>               GAME OVER               <>");
            if (winner==game.UserSymbol) Console.WriteLine("        Congratulation! You win!");
            else if (winner==game.AiSymbol) Console.WriteLine("     Sorry! AI wins, give it another try!");
            else Console.WriteLine("                A tie!");

        }
    }
}
