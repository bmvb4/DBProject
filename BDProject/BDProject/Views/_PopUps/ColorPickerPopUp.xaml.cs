using BDProject.ModelWrappers;
using BDProject.ViewModels.SettingsViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views._PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerPopUp : PopupPage
    {
		ColorsPageViewModel vm = new ColorsPageViewModel();
		int choice = 0;
		List<ColorPick> ColorPicks = new List<ColorPick>();
		public ColorPickerPopUp(ColorsPageViewModel vmodel, int n)
		{
			InitializeComponent();

			vm = vmodel;
			choice = n;


			ColorPicks = new List<ColorPick> {

				new ColorPick("#25c5db"),
				new ColorPick("#0098a6"),
				new ColorPick("#0e47a1"),
				new ColorPick("#1665c1"),
				new ColorPick("#039be6"),

				new ColorPick("#64b5f6"),
				new ColorPick("#ff7000"),
				new ColorPick("#ff9f00"),
				new ColorPick("#ffb200"),
				new ColorPick("#cf9702"),

				new ColorPick("#8c6e63"),
				new ColorPick("#6e4c42"),
				new ColorPick("#d52f31"),
				new ColorPick("#ff1643"),
				new ColorPick("#f44236"),

				new ColorPick("#ec407a"),
				new ColorPick("#ad1457"),
				new ColorPick("#6a1b9a"),
				new ColorPick("#ab48bf"),
				new ColorPick("#b968c7"),

				new ColorPick("#00695b"),
				new ColorPick("#00887a"),
				new ColorPick("#4cb6ac"),
				new ColorPick("#307c32"),
				new ColorPick("#43a047"),

				new ColorPick("#64dd16"),
				new ColorPick("#222222"),
				new ColorPick("#5f7c8c"),
				new ColorPick("#b1bec6"),
				new ColorPick("#465a65"),

				new ColorPick("#607d8d"),
				new ColorPick("#91a5ae"),
			};

			Colors.ItemsSource = ColorPicks;
		}

		private async void OnClose(object sender, EventArgs e)
		{
			await Navigation.PopPopupAsync();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
			Button b = (Button)sender;

            switch (choice)
            {
				case 1:
					vm.IconColor = b.BackgroundColor;
					break;

				case 2:
					vm.TextColor = b.BackgroundColor;
					break;

				case 3:
					vm.BackgroundColor = b.BackgroundColor;
					break;

				default: break;
            }

			await Navigation.PopPopupAsync();
		}
    }
}