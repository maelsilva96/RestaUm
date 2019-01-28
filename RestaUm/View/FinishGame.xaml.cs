using Xamarin.Forms;
using RestaUm.ViewModel;

namespace RestaUm.View
{
    public partial class FinishGame : ContentPage
    {
        public FinishGame()
        {
            InitializeComponent();
            this.BindingContext = new FinishGameViewModel();
        }
    }
}
