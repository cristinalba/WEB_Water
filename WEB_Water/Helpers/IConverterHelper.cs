using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Helpers
{
    public interface IConverterHelper
    {
        User ToUser(EditUserWithImageViewModel model, string path);

        EditUserWithImageViewModel ToChangeUserViewModel(User user);
    }
}
