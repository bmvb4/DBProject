using BDProject.Helpers;
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
            if (e.NewTextValue.EndsWith("\n") && !string.IsNullOrEmpty(e.NewTextValue) && !string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                string temp = tagsEditor.Text;
                temp = temp.Replace("\n", string.Empty);

                var vm = (MakePostPageViewModel)this.BindingContext;
                vm.AllTags.Add(new Tag(temp));
                vm.AllTagsHeight += 45;
                tagsEditor.Text = "";
            }
        }
    }
}