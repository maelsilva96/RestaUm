using RestaUm.ViewModel;

using Xamarin.Forms;

namespace RestaUm.View
{
    public partial class Board : ContentPage
    {
        private BoardViewModel _boardViewModel;
        public Board()
        {
            InitializeComponent();
            this._boardViewModel = new BoardViewModel(_GridBoard);
            BindingContext = this._boardViewModel;
            this.GerateGrid();
        }

        private void GerateGrid()
        {
            _GridBoard = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            this._boardViewModel.GerateBoard();
        }
    }
}
