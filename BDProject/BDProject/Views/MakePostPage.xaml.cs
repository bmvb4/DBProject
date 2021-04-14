using BDProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MakePostPage : ContentPage
    {
        public MakePostPage()
        {
            InitializeComponent();

            descriptionEditor.Unfocus();
            tagsEditor.Unfocus();

            BindingContext = new MakePostPageViewModel();
        }

        private void tagsEditor_TextChangesd(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.EndsWith("\n") && e.NewTextValue.Length >= 2)
            {
                var vm = (MakePostPageViewModel)this.BindingContext;
                vm.AllTags.Add(tagsEditor.Text);
                vm.AllTagsHeight += 45;
                tagsEditor.Text = "";
            }
        }
    }
}