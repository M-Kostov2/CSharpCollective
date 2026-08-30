
using AutoMapper;
using CSharpCollective.Services.DtoModels;
using DataBase.DataBaseProvider;
using DataBase.DataContext;
using DataBase.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Services.ConfigMap;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class RegisterService : IUserValidation, IRegisterService
    {

        private CollectiveContext context;
        private readonly IMapper mapper;
        private ILoginService loginservice;
      



        public RegisterService(IMapper mapper,CollectiveContext context, ILoginService loginservice)
        {
           this.context = context;
           this.mapper = mapper;
           this.loginservice = loginservice;
       

        }


        public UserDto registerUser(UserDto Datarecieved)
        {


            if (userValidation(Datarecieved) == false)
            {
                return null;
            }

            Datarecieved.Password = ManualPasswordHasher.Hash(Datarecieved.Password);

            User userRegistered = new User(Datarecieved.Email, Datarecieved.Password, Datarecieved.UserName);
            ;
            UserDto userDtoInfo = new UserDto();

            var userExist = loginservice.userExists(userRegistered);
            if (userExist != null)
            {

                //  _mapper.Map(userRegistered, userDtoInfo);
                return null;
            }

            context.Users.AddAsync(userRegistered);
            context.SaveChangesAsync();


            mapper.Map(userRegistered, userDtoInfo);

            return userDtoInfo;

        }




        public bool userValidation(UserDto userData)
        {
            if (String.IsNullOrEmpty(userData.Email) || String.IsNullOrEmpty(userData.UserName) ||String.IsNullOrEmpty(userData.Password))
            {
                return false;
            }


            return true;
        }
    }
}