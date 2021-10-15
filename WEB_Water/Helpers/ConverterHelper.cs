using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public EditUserWithImageViewModel ToChangeUserViewModel(User user)
        {
            return new EditUserWithImageViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                PhoneNumber = user.PhoneNumber,
                Nif = user.Nif,
                IsCustomer = user.IsCustomer,
                ImageUrl = user.ImageUrl
            };
        }

        public User ToUser(EditUserWithImageViewModel model, string path)
        {
            return new User
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                Nif = model.Nif,
                IsCustomer = model.IsCustomer,
                ImageUrl = path
            };
        }
    }
}
