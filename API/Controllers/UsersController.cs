using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
        public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        //because we are getting data from database
        [HttpGet]
        [AllowAnonymous]//doesnot require authrization
        public async Task< ActionResult<IEnumerable<AppUser>> >GetUsers()
        {
            
            return await _context.Users.ToListAsync();
        }


         /*
         to ensure that our endpoints protected with authentication
          is at an authorized attribute
        */
        [Authorize]
        [HttpGet("{id}")]
        public async Task< ActionResult<AppUser>> GetUser(int id)
        {
            
            return await _context.Users.FindAsync(id);
        }
    }
}