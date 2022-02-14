using AutoMapper;
using Isopoh.Cryptography.Argon2;
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

    public AccountController(ApplicationDbContext context, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _mapper = mapper;
        _jwtSettings = jwtSettings;
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

        // TODO: Generate Jwt token

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
        await _context.SaveChangesAsync();

        Console.WriteLine("HELLLOOOOOOO!!!!!");

        return ResponseBase.Succeed();
    }
}
