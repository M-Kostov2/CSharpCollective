using CSharpCollective.Services.DtoModels;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILoginService
    {
        public UserDto userExists(UserDto Datarecieved);
        public bool userValidation(UserDto userData);
        public User userExists(User Datarecieved);
    }
}
