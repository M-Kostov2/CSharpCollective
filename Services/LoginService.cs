

using AutoMapper;


using CSharpCollective.Services.DtoModels;
using DataBase.DataContext;
using DataBase.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Services.ConfigMap;
using Services.Interfaces;


namespace Services
{
    public class LoginService : IUserValidation, ILoginService
    {

        private CollectiveContext context;
        private readonly IMapper mapper;




        public LoginService(IMapper mapper, CollectiveContext context)
        {
            this.context = context;
            this.mapper = mapper;

        }



        public UserDto userExists(UserDto Datarecieved)
        {


            if (userValidation(Datarecieved) == false)
            {
                return null;
            }


            User userInfo = new User();
            
            mapper.Map(Datarecieved, userInfo);


            var userExists = context.Users.SingleOrDefault(u => u.UserName == userInfo.UserName);
            ;
            UserDto userDtoInfo = new UserDto();
            if (userExists != null)
            {
                string role = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => u.Role).Single().ToString();
                string password = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => u.Password).Single().ToString();
                if (!ManualPasswordHasher.Verify(userInfo.Password, password))
                {
                    userDtoInfo.Password = "Wrong Password";
                    return userDtoInfo;
                }
                if (role == "Admin")
                {
                    userInfo = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => new User
                    {

                        UserName = u.UserName,
                        Email = u.Email,
                        Role = u.Role,
                        Posts = u.Posts.ToArray()

                    }).Single();



                }
                else if (role == "User")
                {
                    userInfo = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => new User
                    {

                        UserName = u.UserName,
                        Email = u.Email,
                        Role = u.Role,
                        LastOnline = DateTime.Now

                    }).Single();

                }
                // _context.Users.Update(userInfo);
                userInfo = context.Users.Single(u => u.UserName == userInfo.UserName);
                userInfo.LastOnline = DateTime.Now;
                context.SaveChanges();
                mapper.Map(userInfo, userDtoInfo);
            }
            else if (userExists == null)
            {
                userDtoInfo = null;
                return userDtoInfo;
            }

            return userDtoInfo;

        }

        public User userExists(User Datarecieved)
        {


            User userInfo = new User();
            userInfo = Datarecieved;




            var userExists = context.Users.SingleOrDefault(u => u.UserName == userInfo.UserName);
            if (userExists != null)
            {
                var role = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => u.Role).ToString();

                if (role == "Admin")
                {
                    userInfo = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => new User
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                        Role = u.Role,
                        Posts = u.Posts.ToArray()

                    }).Single();



                }
                else if (role == "User")
                {
                    userInfo = context.Users.Where(u => u.UserName == userInfo.UserName).Select(u => new User
                    {
                        UserName = u.UserName,
                        Email = u.Email,
                        Role = u.Role

                    }).Single();

                }

            }
            else if (userExists == null)
            {
                userInfo = null;
                return userInfo;
            }

            return userInfo;

        }

        public bool userValidation(UserDto userData)
        {
            if (String.IsNullOrEmpty(userData.Password) || String.IsNullOrEmpty(userData.UserName))
            {
                return false;
            }


            return true;
        }


    }
}

