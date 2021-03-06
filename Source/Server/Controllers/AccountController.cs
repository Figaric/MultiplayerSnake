using AutoMapper;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultiplayerSnake.Shared;

namespace MultiplayerSnake.Server;

[ApiController]
[Route(ApiEndpoints.AccountRoute)]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly IOptions<JwtSettings> _jwtSettings;

    private readonly RedisService _redisService;

    public AccountController(
        ApplicationDbContext context, 
        IMapper mapper, 
        IOptions<JwtSettings> jwtSettings,
        RedisService redisService)
    {
        _context = context;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
        _redisService = redisService;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet(ApiEndpoints.MeRoute)]
    public async Task<IResponse> Me()
    {
        return ResponseData<string>.Succeed(HttpContext.User.Identity.Name);
    }

    [HttpPost(ApiEndpoints.LoginRoute)]
    public async Task<IResponse> Login(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

        if(user is null)
        {
            return ResponseFail<FieldError>.Failed(
                new FieldError
                {
                    Field = nameof(dto.UserName),
                    Message = "Such user does not exist"
                });
        }

        var passwordVarificationResult = Argon2.Verify(user.Password, dto.Password);

        if(!passwordVarificationResult)
        {
            return ResponseFail<FieldError>.Failed(
                new FieldError
                {
                    Field = nameof(dto.Password),
                    Message = "Invalid password"
                });
        }

        return ResponseData<LoginResponseData>.Succeed(new LoginResponseData
        {
            JwtToken = Utillities.GenerateJwtToken(user, _jwtSettings.Value) 
        });
    }

    [HttpPost(ApiEndpoints.RegisterRoute)]
    public async Task<IResponse> RegisterAsync(UserRegisterDto dto)
    {
        string hashedPassword = Argon2.Hash(dto.Password);

        User user = _mapper.Map<User>(dto);
        user.Password = hashedPassword;

        await _context.Users.AddAsync(user);

        try
        {
            await _context.SaveChangesAsync();
        } 
        catch
        {
            if((await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName)) != null)
            {
                return ResponseFail<FieldError>.Failed(new FieldError
                {
                    Field = nameof(dto.UserName),
                    Message = "This UserName is already taken"
                });
            }
        }

        return ResponseBase.Succeed();
    }

    // [HttpPost(ApiEndpoints.ForgotPasswordRoute)]
    // public async Task<IResponse> ForgotPasswordAsync(UserForgotPasswordDto dto)
    // {
    //     var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);

    //     if(user == null)
    //     {
    //         // Do that for security purposes
    //         Thread.Sleep(2000);

    //         return ResponseBase.Succeed();
    //     }

    //     var token = await _redisService.GenerateForgotPasswordTokenAsync(user.Id);

    //     // TODO: send email to the user

    //     return ResponseBase.Succeed();
    // }
}
