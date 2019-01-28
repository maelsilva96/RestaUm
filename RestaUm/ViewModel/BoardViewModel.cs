using System.ComponentModel;
using RestaUm.Model;
using Xamarin.Forms;
using System.Collections.Generic;
using RestaUm.Ultil;
using System.Linq;

namespace RestaUm.ViewModel
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        private Grid _grid { get; set; }
        private Piece PieceSelect { get; set; }

        private int roundProperty;
        public int Round { get { return roundProperty; } private set { roundProperty = value; OnPropertyChanged("Round"); } }

        private int piecesInGame;
        public int PiecesInGame { get { return piecesInGame; } private set { piecesInGame = value; OnPropertyChanged("PiecesInGame"); } }

        private Board Board { get; set; }
        private bool PossibleMovement { get; set; } = true;

        public Command CommandPiece { get; set; }
        public Command CommandClearPiece { get; set; }
        public Command CommandReturnHome { get; set; }

        public BoardViewModel(Grid grid)
        {
            this._grid = grid;
            this.Round = Stock.StockContext.Round;
            this.Board = Stock.StockContext.Board;
            this.CommandPiece = new Command((list) => this.SelectPiece((List<int>)list));
            this.CommandClearPiece = new Command((list) => this.ClearPiece((List<int>)list));
            this.CommandReturnHome = new Command(this.ReturnHome);
        }

        public void GerateBoard()
        {
            this._grid.Children.Clear();
            for (int x = 0; x < this.Board.Pieces.GetLength(0); x++)
            {
                for (int y = 0; y < this.Board.Pieces.GetLength(1); y++)
                {
                    this.PlayAvailable(x, y);
                    this._grid.Children.Add(
                        this.GetButton(this.Board.Pieces[x, y])
                        , x, y
                    );
                }
            }
            this.FinishGame();
        }

        public void ClearPiece(List<int> list)
        {
            this.PieceSelect = null;
            this.Board.Pieces[list[0], list[1]].StatusPiece = Enum.StatusPiece.Activo;
            this.GerateBoard();
        }

        public void SelectPiece(List<int> list)
        {
            var piece = this.Board.Pieces[list[0], list[1]];
            if (this.PieceSelect != null)
            {
                if (piece.StatusPiece == Enum.StatusPiece.Livre) this.MoveToPieceIsValid(piece);
                else Application.Current.MainPage.DisplayAlert("Ops", "Movimento invalido!", "Ok");
            }
            else
            {
                this.SelectPice(piece);
            }
            this.GerateBoard();
        }

        private void FinishGame () {
            if(!this.PossibleMovement) {
                Stock.StockContext.Records.Add(new Record()
                {
                    NumPieces = this.PiecesInGame,
                    NumPlays = this.Round
                });
                Application.Current.MainPage = new View.FinishGame();
            }
        }

        private void ReturnHome()
        {
            Application.Current.MainPage = new View.Home();
        }

        private void PlayAvailable(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                this.PiecesInGame = 0;
                this.PossibleMovement = false;
            }

            if (
                this.Board.Pieces[x, y].StatusPiece == Enum.StatusPiece.Activo ||
                this.Board.Pieces[x, y].StatusPiece == Enum.StatusPiece.Checked
            )
            {
                this.PiecesInGame++;
                if (x > 1 && !this.PossibleMovement) this.PossibleMovement = this.PieceXValid(x, y, false);
                if (x < 5 && !this.PossibleMovement) this.PossibleMovement = this.PieceXValid(x, y, true);

                if (y > 1 && !this.PossibleMovement) this.PossibleMovement = this.PieceYValid(x, y, false);
                if (y < 5 && !this.PossibleMovement) this.PossibleMovement = this.PieceYValid(x, y, true);
            }
        }

        private bool PieceXValid(int x, int y, bool someValue)
        {
            var p = this.Board.Pieces;
            return someValue
                ? (p[(x + 1), y].StatusPiece == Enum.StatusPiece.Checked) && (p[(x + 2), y].StatusPiece == Enum.StatusPiece.Checked) ||
                    (p[(x + 1), y].StatusPiece == Enum.StatusPiece.Activo) && (p[(x + 2), y].StatusPiece == Enum.StatusPiece.Livre)
                    : 
                (p[(x - 1), y].StatusPiece == Enum.StatusPiece.Checked) && (p[(x - 2), y].StatusPiece == Enum.StatusPiece.Checked) ||
                    (p[(x - 1), y].StatusPiece == Enum.StatusPiece.Activo) && (p[(x - 2), y].StatusPiece == Enum.StatusPiece.Livre);
        }

        private bool PieceYValid(int x, int y, bool someValue)
        {
            var p = this.Board.Pieces;
            return someValue
                ? (p[x, (y + 1)].StatusPiece == Enum.StatusPiece.Checked) && (p[x, (y + 2)].StatusPiece == Enum.StatusPiece.Checked) ||
                    (p[x, (y + 1)].StatusPiece == Enum.StatusPiece.Activo) && (p[x, (y + 2)].StatusPiece == Enum.StatusPiece.Livre)
                    : 
                (p[x, (y - 1)].StatusPiece == Enum.StatusPiece.Checked) && (p[x, (y - 2)].StatusPiece == Enum.StatusPiece.Checked) ||
                    (p[x, (y - 1)].StatusPiece == Enum.StatusPiece.Activo) && (p[x, (y - 2)].StatusPiece == Enum.StatusPiece.Livre);
        }

        private void SelectPice(Piece piece)
        {
            if (piece.StatusPiece == Enum.StatusPiece.Activo)
            {
                piece.StatusPiece = Enum.StatusPiece.Checked;
                this.PieceSelect = piece;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Ops", "Selecione uma peça valida!", "Ok");
            }
        }

        private void MoveToPieceIsValid(Piece piece)
        {
            if (NumberUtil.Diff(piece.LocationX, this.PieceSelect.LocationX) == 2)
            {
                this.MovePieceInX(piece);
                this.Round += 1;
            }
            else if (NumberUtil.Diff(piece.LocationY, this.PieceSelect.LocationY) == 2)
            {
                this.MovePieceInY(piece);
                this.Round += 1;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Ops", "Movimento invalido!", "Ok");
            }
        }

        private void MovePieceInY(Piece piece)
        {
            piece.StatusPiece = Enum.StatusPiece.Activo;
            if (piece.LocationY < this.PieceSelect.LocationY)
            {
                this.Board.Pieces[
                    this.PieceSelect.LocationX, (this.PieceSelect.LocationY - 1)
                ].StatusPiece = Enum.StatusPiece.Livre;
            }
            else
            {
                this.Board.Pieces[
                    this.PieceSelect.LocationX, (this.PieceSelect.LocationY + 1)
                ].StatusPiece = Enum.StatusPiece.Livre;
            }
            this.Board.Pieces[
                this.PieceSelect.LocationX, this.PieceSelect.LocationY
            ].StatusPiece = Enum.StatusPiece.Livre;
            this.PieceSelect = null;
        }

        private void MovePieceInX(Piece piece)
        {
            piece.StatusPiece = Enum.StatusPiece.Activo;
            if (piece.LocationX < this.PieceSelect.LocationX)
            {
                this.Board.Pieces[
                    (this.PieceSelect.LocationX - 1), this.PieceSelect.LocationY
                ].StatusPiece = Enum.StatusPiece.Livre;
            }
            else
            {
                this.Board.Pieces[
                    (this.PieceSelect.LocationX + 1), this.PieceSelect.LocationY
                ].StatusPiece = Enum.StatusPiece.Livre;
            }
            this.Board.Pieces[
                this.PieceSelect.LocationX, this.PieceSelect.LocationY
            ].StatusPiece = Enum.StatusPiece.Livre;
            this.PieceSelect = null;
        }

        private Button GetButton(Piece piece)
        {
            switch (piece.StatusPiece)
            {
                case Enum.StatusPiece.Inactive:
                    return new Button
                    {
                        BackgroundColor = Color.Transparent,
                        BorderWidth = 0,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        WidthRequest = 30,
                        HeightRequest = 30
                    };
                case Enum.StatusPiece.Livre:
                    return new Button
                    {
                        Image = "ball_golf.png",
                        BackgroundColor = Color.Transparent,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        WidthRequest = 30,
                        HeightRequest = 30,
                        Command = this.CommandPiece,
                        CommandParameter = new List<int>() { piece.LocationX, piece.LocationY }
                    };
                case Enum.StatusPiece.Checked:
                    return new Button
                    {
                        Image = "ball.png",
                        BackgroundColor = Color.FromRgba(0, 0, 200, 0.5),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Command = this.CommandClearPiece,
                        CommandParameter = new List<int>() { piece.LocationX, piece.LocationY }
                    };
                default:
                    return new Button
                    {
                        Image = "ball.png",
                        BackgroundColor = Color.Transparent,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Command = this.CommandPiece,
                        CommandParameter = new List<int>() { piece.LocationX, piece.LocationY }
                    };

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string NameProperty)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(NameProperty));
            }
        }
    }
}
