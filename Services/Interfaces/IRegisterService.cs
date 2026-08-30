using CSharpCollective.Services.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IRegisterService
    {
        public UserDto registerUser(UserDto Datarecieved);
        public bool userValidation(UserDto userData);
    }
}
