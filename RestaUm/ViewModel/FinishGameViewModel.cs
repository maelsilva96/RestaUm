using System.ComponentModel;
using System.Linq;
using RestaUm.Model;
using Xamarin.Forms;

namespace RestaUm.ViewModel
{
    public class FinishGameViewModel : INotifyPropertyChanged
    {
        public int NumPieces { get; private set; }
        public int NumPlays { get; private set; }
        public string Msg { get; private set; }
        public Command commandToHome { get; set; }

        public FinishGameViewModel() {
            Record record = Stock.StockContext.Records.LastOrDefault();
            this.NumPieces = record.NumPieces;
            this.NumPlays = record.NumPlays;
            this.Msg = (this.NumPieces == 1) ? "Você ganhou!" : "Você Perdeu!";
            this.commandToHome = new Command(GoToHome);
        }

        private void GoToHome ()
        {
            Application.Current.MainPage = new View.Home();
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
