using BDProject.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BDProject.API
{
    interface APIInterface
    {
        [Post("/login")]
        Task<string> LogInUser([Body] User user);

        [Post("/register")]
        Task<string> RegisterUser([Body] User user);
    }
}
