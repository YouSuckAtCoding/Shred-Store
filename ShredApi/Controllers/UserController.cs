using DataLibrary.User;
using Microsoft.AspNetCore.Mvc;
using Models;


namespace ShredApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }
        
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            var users = await userRepository.GetUsuarios();
            return Ok(users);
        }

        // GET: api/v1/Controller/Login
        [HttpGet("Login")]
        public async Task<ActionResult<UserModel>> Get([FromBody] UserLoginModel user)
        {

            var users = await userRepository.Login(user.Name, user.Password);
            if(users != null)
            {
                return Ok(users);
            }
            return BadRequest();
            
        }


        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> Get(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }
            var user = await userRepository.GetUser(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);


        }

        // POST api/<UserController>/Create
        [HttpPost]
        public async Task<ActionResult<UserRegisterModel>> Post([FromBody] UserRegisterModel user)
        {
            if (ModelState.IsValid)
            {
                await userRepository.InsertUser(user);
                return Ok(user);
            }
            return BadRequest();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> Put(int id, [FromBody] UserModel user)
        {
            if(id != user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await userRepository.UpdateUser(user);
                    return Ok(user);
                }
                catch
                {
                    return StatusCode(409);
                }
            }
            return BadRequest();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await userRepository.DeleteUser(id);
            return Ok();
        }

        [HttpGet("CheckEmail")]
        public async Task<ActionResult> Get([FromBody]string Email)
        {
            var users = await userRepository.CheckUserEmail(Email);
            if(users == "No")
            {
                return Ok("No");
            }
            return Ok(users);
        }
        [HttpPost("PasswordReset")]
        public async Task<ActionResult> Post([FromBody] UserResetPasswordModel user)
        {          
           try
           {
               var res = await userRepository.ResetUserPassword(user.Password, user.Email);
                if (res)
                {
                    return Ok();
                }
                return BadRequest();
           }
           catch
           {
               return StatusCode(409);
           }
            
            
        }

    }
}
