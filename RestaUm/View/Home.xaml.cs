using RestaUm.ViewModel;

using Xamarin.Forms;

namespace RestaUm.View
{
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}
