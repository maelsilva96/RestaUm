using System;
using System.ComponentModel;
using Xamarin.Forms;
using RestaUm.Business;

namespace RestaUm.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public Command StartGame { get; set; }
        public HomeViewModel()
        {
            this.StartGame = new Command(StartNewGame);
        }

        private void StartNewGame() 
        {
            Stock.StockContext.Board = (new BoardBusiness(new Model.Board())).CreateNew();
            App.Current.MainPage = new View.Board();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string NameProperty)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(NameProperty));
            }
        }
    }
}
