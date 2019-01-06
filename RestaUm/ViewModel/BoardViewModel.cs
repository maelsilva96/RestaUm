using System;
using System.ComponentModel;
using RestaUm.Model;
using RestaUm.Business;

namespace RestaUm.ViewModel
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        private int roundProperty;
        public int Round { get { return roundProperty; } private set { roundProperty = value; OnPropertyChanged("Round"); } }
        private Board boardProperty;
        public Board Board { get { return boardProperty; } private set { boardProperty = value; OnPropertyChanged("Board"); } }
        private BoardBusiness _boardBusiness;
       
        public BoardViewModel()
        {
            this.Round = Stock.StockContext.Round;
            this.Board = Stock.StockContext.Board;
            this._boardBusiness = new BoardBusiness(this.Board);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string NameProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(NameProperty));
            }
        }
    }
}
