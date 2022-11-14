using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMapper mapper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/account
        // es un get req que solo tiene el token en el header
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // mejor q el de abajo
            //var email = User.FindFirstValue(ClaimTypes.Email);                 **
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x =>
            //    x.Type == ClaimTypes.Email)?.Value;
            // ---------------------------------> CAMBIADOS XEL EXTENSION METHOD EN UserManagerExtensions
            //var user = await _userManager.FindByEmailAsync(email);

            var user = await _userManager.FindByEmailFromClaimsPrincipal(User); // mejor con **

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
            };
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/account/emailexist
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/account/address
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            // mejor q el de abajo
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x =>
            //    x.Type == ClaimTypes.Email)?.Value;
            // ---------------------------------> CAMBIADOS XEL EXTENSION METHOD EN UserManagerExtensions
            //var user = await _userManager.FindByEmailAsync(email);

            var user = await _userManager.FindByUserByClaimsPrincipalWithAddressAsync(User);

            return _mapper.Map<AddressDto>(user.Address);
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// PUT: api/account/address
        // la actualizo actualizando el user
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByUserByClaimsPrincipalWithAddressAsync(User);
            // usando automapper p' update las properties
            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

            return BadRequest("Problem updating the address :( ");
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
            };
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// POST: api/account/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {
                        "Email address is in use."
                    }
                });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}
