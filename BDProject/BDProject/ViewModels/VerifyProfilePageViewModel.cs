using BDProject.DatabaseModels;
using BDProject.Helpers;
using BDProject.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDProject.ViewModels
{
    public class VerifyProfilePageViewModel: BaseViewModel
    {

        public VerifyProfilePageViewModel()
        {

            BackCommand = new Command(async () => await BackFunction());
            ResendCommand = new Command(ResendFunction);
            VerifyCommand = new Command(VerifyFunction);
        }

        // Parameters
        // Code parameter
        private string code = "";
        public string Code
        {
            get => code;
            set
            {
                if (value == code) { return; }
                code = value;
                OnPropertyChanged();
            }
        }

        // Code Alert parameter
        private string codeAlert = "";
        public string CodeAlert
        {
            get => codeAlert;
            set
            {
                if (value == codeAlert) { return; }
                codeAlert = value;
                OnPropertyChanged();
            }
        }

        // Commands
        // Back to post command
        public ICommand BackCommand { get; set; }
        private async Task BackFunction()
        {
            await Shell.Current.GoToAsync("//SignUpPage");
            ClearEverything();
        }

        // Resend code command
        public ICommand ResendCommand { get; set; }
        private async void ResendFunction()
        {
            
        }

        // Open Posts command
        public ICommand VerifyCommand { get; set; }
        private async void VerifyFunction()
        {
            var success = await ServerServices.SendPostRequestAsync($"email/confirmEmail?Username={_Globals.UsernameTemp}&Code={Code}", new JObject());

            if (success.IsSuccessStatusCode)
            {

                JObject oJsonObject = new JObject();
                oJsonObject.Add("Username", _Globals.UsernameTemp);
                oJsonObject.Add("Password", _Globals.PasswordTemp);

                success = await ServerServices.SendPostRequestAsync("login", oJsonObject);

                if (success.IsSuccessStatusCode)
                {
                    Preferences.Set("UsernameKey", _Globals.UsernameTemp);
                    Preferences.Set("PasswordKey", _Globals.PasswordTemp);

                    var earthquakesJson = success.Content.ReadAsStringAsync().Result;
                    var rootobject = JsonConvert.DeserializeObject<UserDB>(earthquakesJson);

                    _Globals.GlobalMainUser = new User(rootobject);

                    success = await ServerServices.SendGetRequestAsync($"posts/getAll/{rootobject.Username}", oJsonObject);

                    if (success.IsSuccessStatusCode)
                    {
                        earthquakesJson = success.Content.ReadAsStringAsync().Result;
                        var postList = JsonConvert.DeserializeObject<List<BigPostDB>>(earthquakesJson);
                        _Globals.GlobalMainUser.AddPostsFromDB(postList);
                        _Globals.AddPostsFromDB(postList);

                        await Shell.Current.GoToAsync("//HomePage");
                    }
                }
            }
            else
            {
                CodeAlert = "Invalid Code";
            }

            // Reset password
            await Shell.Current.GoToAsync("//HomePage");

            ClearEverything();
        }

        // Functions
        // Clear everything
        private void ClearEverything()
        {
            Code = "";
            CodeAlert = "";
        }
    }
}
