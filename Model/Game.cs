using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicTacToeWPF_MVVM.Model
{
    public class Game
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public string CurrentMark { get; set; }
        private List<string> MarksToAssign { get; set; } = new List<string>();
        public string Winner { get; set; }
        public bool IsFinished { get; set; }

        public Game(Player p1, Player p2)
        {
            // setting defaults
            IsFinished = false;

            // adding players into list
            Players.Add(p1);
            Players.Add(p2);

            // assigning marks to list and then losowaning them across players
            MarksToAssign.Add("X");
            MarksToAssign.Add("O");
            AssignMark();

            // print players info
            foreach (var player in Players)
            {
                Console.WriteLine($"PlayerInfo: {player.Name}, {player.Mark}, {player.IsTurn}");
            }

            // assigining turn to player with 'X'
            AssignTurn();

            foreach (var player in Players)
            {
                Console.WriteLine($"PlayerInfo after turn assignment: {player.Name}, {player.Mark}, {player.IsTurn}");
            }
        }

        // display whose round is it
        public string WhoseRound()
        {
            // beautiful LINQ expression
            // retrieve player from players, whose IsTurn == true
            var player = Players.First(x => x.IsTurn == false);

            return $"Tura gracza: {player.Name}, gra on znakiem {player.Mark}";
        }

        // change turn to opposite state when invoked
        private void ChangeTurn(Player player)
        {
            player.IsTurn = !player.IsTurn;      
        }

        // method for controlling the game, switch player, checking if won etc
        public void ChangeTurns(string[] field)
        {
            foreach (var player in Players)
            {
                if (CheckWin(field) == true)
                {
                    var winner = Players.FirstOrDefault(x => x.IsWinner == true);
                    MessageBox.Show($"Wygrał gracz {winner.Name}, grał on znakiem {winner.Mark}");
                    IsFinished = true;
                    break;
                }
                // czy remis?
                else if (IsTie(field))
                {
                    MessageBox.Show("Remis!");
                    IsFinished = true;
                    break;
                }

                if (player.IsTurn)
                {
                    CurrentMark = player.Mark;
                }

                // change turn at the end
                ChangeTurn(player);
            }
        }

        // assign first round to the guy with "X" mark
        private void AssignTurn()
        {
            var player = Players.FirstOrDefault(x => x.Mark == "X");
            player.IsTurn = true;
        }

        private bool CheckWin(string[] table)
        {
            var conditionToWin1 = CheckWinHorizontal(table);
            var conditionToWin2 = CheckWinVertical(table);
            var conditionToWin3 = CheckWinDiagonal(table);

            return conditionToWin1 || conditionToWin2 || conditionToWin3;
        }

        // check win diagonally
        private bool CheckWinDiagonal(string[] table)
        {
            // logic here is: if 3 marks are the same, then the winner is player with same mark
            // thanks LINQ
            if ((table[0] + table[4] + table[8]) == "XXX")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "X");
                winner.IsWinner = true;

                return true;
            }

            if ((table[0] + table[4] + table[8]) == "OOO")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "O");
                winner.IsWinner = true;

                return true;
            }

            if ((table[2] + table[4] + table[6]) == "XXX")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "X");
                winner.IsWinner = true;

                return true;
            }

            if ((table[2] + table[4] + table[6]) == "OOO")
            {
                var winner = Players.FirstOrDefault(x => x.Mark == "O");
                winner.IsWinner = true;

                return true;
            }

            return false;
        }

        // check win verticaly
        private bool CheckWinVertical(string[] table)
        {
            for (int i = 0; i < 3; i++)
            {
                string txt = table[i] + table[i + 3] + table[i + 6];
                if (txt == "XXX")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "X");
                    winner.IsWinner = true;
                    
                    return true;
                }

                if (txt == "OOO")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "O");
                    winner.IsWinner = true;

                    return true;
                }
                    
            }

            return false;
        }

        // check win horizontally
        private bool CheckWinHorizontal(string[] table)
        {
            for (int i = 0; i < 9; i += 3)
            {
                string txt = table[i] + table[i + 1] + table[i + 2];
                if (txt == "XXX")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "X");
                    winner.IsWinner = true;

                    return true;
                }

                if (txt == "OOO")
                {
                    var winner = Players.FirstOrDefault(x => x.Mark == "O");
                    winner.IsWinner = true;

                    return true;
                }
            }

            return false;
        }

        private bool IsTie(string[] table)
        {
            var txt = "";

            foreach(var element in table)
            {
                txt += element;
            }

            if (txt.Length == 9 && !String.IsNullOrWhiteSpace(txt))
                return true;
            
            return false;
        }

        // assign one of two marks randomly to players
        // method called right at beggining
        private void AssignMark()
        {
            var random = new Random();

            foreach (var player in Players)
            {
                int index = random.Next(MarksToAssign.Count);
                player.Mark = MarksToAssign.ElementAt(index);
                MarksToAssign.RemoveAt(index);
            }
        }
    }
}
