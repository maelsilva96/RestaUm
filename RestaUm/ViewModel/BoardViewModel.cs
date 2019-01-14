using System.ComponentModel;
using RestaUm.Model;
using RestaUm.Business;
using Xamarin.Forms;
using System.Collections.Generic;
using RestaUm.Ultil;

namespace RestaUm.ViewModel
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        private int roundProperty;
        private Grid _grid { get; set; }
        private Piece PieceSelect { get; set; }

        public int Round { get { return roundProperty; } private set { roundProperty = value; OnPropertyChanged("Round"); } }
        private Board Board { get; set; }
        private readonly BoardBusiness _boardBusiness;

        public Command CommandPiece;
        public Command CommandClearPiece;

        public BoardViewModel(Grid grid)
        {
            this._grid = grid;
            this.Round = Stock.StockContext.Round;
            this.Board = Stock.StockContext.Board;
            this._boardBusiness = new BoardBusiness(this.Board);
            this.CommandPiece = new Command((list) => this.SelectPiece((List<int>)list));
            this.CommandClearPiece = new Command((list) => this.ClearPiece((List<int>)list));

        }

        public void GerateBoard()
        {
            this._grid.Children.Clear();
            for (int x = 0; x < this.Board.SizeX; x++)
            {
                for (int y = 0; y < this.Board.SizeY; y++)
                {
                    this._grid.Children.Add(
                        this.GetButton(this.Board.Pieces[x, y])
                        , x, y
                    );
                }
            }
        }

        public void ClearPiece(List<int> list) {
            this.PieceSelect = null;
            this.Board.Pieces[list[0], list[1]].StatusPiece = Enum.StatusPiece.Activo;
            this.GerateBoard();
        }

        public void SelectPiece(List<int> list)
        {
            var piece = this.Board.Pieces[list[0], list[1]];
            if (this.PieceSelect != null) {
                if(piece.StatusPiece == Enum.StatusPiece.Livre) {
                    this.MoveToPieceIsValid(piece);
                } else {
                    Application.Current.MainPage.DisplayAlert("Ops", "Movimento invalido!", "Ok");
                }
            } else {
                this.SelectPice(piece);
            }
            this.GerateBoard();
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

        private void MoveToPieceIsValid (Piece piece) {
            if(NumberUtil.Diff(piece.LocationX, this.PieceSelect.LocationX) == 2)
            {
                this.MovePieceInX(piece);
            } else if(NumberUtil.Diff(piece.LocationY, this.PieceSelect.LocationY) == 2)
            {
                this.MovePieceInY(piece);
            } else
            {
                Application.Current.MainPage.DisplayAlert("Ops", "Movimento invalido!", "Ok");
            }
        }

        private void MovePieceInY (Piece piece)
        {
            piece.StatusPiece = Enum.StatusPiece.Activo;
            if (piece.LocationY < this.PieceSelect.LocationY)
            {
                this.Board.Pieces[
                    this.PieceSelect.LocationX, (this.PieceSelect.LocationY - 1)
                ].StatusPiece = Enum.StatusPiece.Livre;
            } else
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

        private void MovePieceInX (Piece piece) {
            piece.StatusPiece = Enum.StatusPiece.Activo;
            if(piece.LocationX < this.PieceSelect.LocationX)
            {
                this.Board.Pieces[
                    (this.PieceSelect.LocationX - 1), this.PieceSelect.LocationY
                ].StatusPiece = Enum.StatusPiece.Livre;
            } else
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
