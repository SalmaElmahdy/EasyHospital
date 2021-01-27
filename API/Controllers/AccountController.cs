using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        //variable gwah data bia5odha mn database
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
       //we create a "token service" when the user register or login
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;

        }
        [HttpPost("register")]//bc we get new info about  new user


        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("username is taken");
            //  problem
            /*
            in this method in the first i used two parameters of type string(username,password)
            to be save in the database but it canot take any thing from the body it just take from the place which include the link http as a query
            so it is not an efficient way and it gives me null if i writ the pass and user name in the body */
            //  solution
            /*
            so we using the concept of DTOs or data transfere opjects


            */


            using var hmac = new HMACSHA512();//class wich we will use to hash our password
            //any time we use a var with using it is going to call a method from this class which call dispose
            //any class  use dispose method call something call the disposable interface
            //this class generate a pass key which i should save in password salt after take key from user
            //second thing we create a user
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                //the computHash method just take byte arr so i should convert password string to byte array

                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            //next we should add our user table
            //but it do not save anything in the databse it just put it in Users
            _context.Users.Add(user);
            //so to save it in the table int the databse 
            await _context.SaveChangesAsync();

            /*
            instead of return user we will return UserDto
            */

            return new UserDto{
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //the first thing we want to do is get the user from database
            // we are going to use await bc we make requist to our DB

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            // this fun is return the only element of the sequence or a default valuue if it is empty 
            // and it is threw an exception if there is more than element in the sequence
            if (user == null) return Unauthorized("Invalid username");


            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");

            }
            return new UserDto{
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            //used to make sure  that username is unique
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());

        }




    }
}