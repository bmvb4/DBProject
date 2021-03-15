using BDProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BDProject.ModelWrappers
{
    public class UserWrapper
    {
        public UserWrapper(User user)
        {
            id = user.IdUser;
            username = user.Username;
            password = user.Password;
            email = user.Email;
            firstname = user.FirstName;
            lastname = user.LastName;
            description = user.Description;
            base64Photo = user.Photo;
            token = user.token;
        }

        private int id;
        public int ID
        {
            get => id;
        }

        private string username;
        public string Username
        {
            get => username;
        }

        private string password;
        public string Password
        {
            get => password;
        }

        private string email;
        public string Email
        {
            get => email;
        }

        private string firstname;
        public string FirstName
        {
            get => firstname;
        }

        private string lastname;
        public string LastName
        {
            get => lastname;
        }

        private string description;
        public string Description
        {
            get => description;
        }

        private string base64Photo;
        public ImageSource PhotoSource
        {
            get
            {
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(base64Photo)));
            }
        }


        private string token;
        public string Token
        {
            get => token;
        }
    }
}
