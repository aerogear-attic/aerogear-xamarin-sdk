using Example.Models;
using Example.Services;
using Example.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ListDiff;

namespace Example.ViewModels
{
    class PlaceholderUsersVM : BaseVM
    {
        public ObservableCollection<PlaceholderUser> Users { get; private set; }
        public bool Loading = false;
        
        private UsersService usersService = new UsersService();

        public PlaceholderUsersVM()
        {
            Users = new ObservableCollection<PlaceholderUser>();
        }

        public async Task Fetch()
        {
            Loading = true;
            var data = await usersService.GetUsers();
            Loading = false;
            Users.MergeInto(data, (user1, user2) => user1.id == user2.id);
        }
    }
}
