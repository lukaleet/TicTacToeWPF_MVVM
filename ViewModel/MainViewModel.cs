using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF_MVVM.Model;

namespace TicTacToeWPF_MVVM.ViewModel
{
    using BaseClass;
    using System.Windows.Controls;
    using System.Windows.Input;
    public class MainViewModel : ViewModel
    {

        /*
         * OKNO SIE ROZJEZDZA GDY PODAJE SIE ZA DLUGI NICK, NIESTETY
         * RESZTA DZIALA :--)
         */

        // EDIT 17.04.2021 11:38
        // DODANO WALIDACJE POL PRZED GRA


        // initializing Game object
        private Game _ngame;

        // helper flag to reset state
        private bool _gameInProgress = false;

        // creating array of empty strings
        // 1x 9
        public string[] _table = new string[] { "", "", "", "", "", "", "", "", "" };

        public string[] Table
        {
            get { return _table; }
            set
            {
                _table = value;
                onPropertyChange(nameof(Table));
            }
        }

        // full prop for displaying info below board
        private string _whoseRound;

        public string WhoseRound
        {
            get
            {
                return _whoseRound;
            }
            set
            {
                _whoseRound = value;
                onPropertyChange(nameof(WhoseRound));
            }
        }


        public MainViewModel()
        {
        }

        // input fields for name of first player
        private string _playerName1;

        public string PlayerName1
        {
            get { return _playerName1; }
            set
            {
                _playerName1 = value;
                onPropertyChange(nameof(PlayerName1));
            }
        }

        // input field for name of second player
        private string _playerName2;

        public string PlayerName2
        {
            get { return _playerName2; }
            set
            {
                _playerName2 = value;
                onPropertyChange(nameof(PlayerName2));
            }
        }

        // if can click, then trigger OnClickFunc
        private ICommand _onClick;

        public ICommand OnClick
        {
            get
            {
                return _onClick ?? (_onClick = new RelayCommand(OnClickFunc, CanOnClick));
            }
        }

        private void OnClickFunc(object obj)
        {
            // parsing Command of Button to int
            int field = int.Parse((string)obj);

            Table[field] = _ngame.CurrentMark;

            // update table, this is kind of magic
            Table = Table;

            // change turn after click
            _ngame.ChangeTurns(Table);
            WhoseRound = _ngame.WhoseRound();

            // reset field if game is finished (somebody won or tied)
            if (_ngame.IsFinished)
            {
                Table = new string[] { "", "", "", "", "", "", "", "", "" };
                PlayerName1 = "";
                PlayerName2 = "";
                _gameInProgress = false;
                WhoseRound = "";
            }
        }

        // if one of small button has something else than "", then prevent from clicking it
        private bool CanOnClick(object param)
        {
            // parse command of a button to int, to act as index
            int field = int.Parse((string)param);

            // block all fields if game is not started
            if (!_gameInProgress)
            {
                return false;
            }

            for (int i = 0; i < Table.Length; i++)
            {
                if (Table[field] != "")
                    return false;
            }

            return true;
        }

        private ICommand _start;

        public ICommand Start
        {
            get
            {
                return _start ?? (_start = new RelayCommand(StartGame, CanStartGame));
            }
        }

        // creating players and game after providing names, and clicking button
        private void StartGame(object param)
        {
            var player1 = new Player(_playerName1);
            var player2 = new Player(_playerName2);
            _ngame = new Game(player1, player2);
            _gameInProgress = true;

            // making turn at start, to make CurrentMark not null
            _ngame.ChangeTurns(Table);

            // display first round from here
            WhoseRound = _ngame.WhoseRound();
        }

        // if names are provided, then game can be started
        private bool CanStartGame(object param)
        {
            // if both TextBoxes are not empty
            var textBoxEmpty = !(String.IsNullOrEmpty(_playerName1) || String.IsNullOrEmpty(_playerName2));

            // if both of them are not empty and game is not started, then you can click button
            return (textBoxEmpty && !_gameInProgress);
        }
    }
}
