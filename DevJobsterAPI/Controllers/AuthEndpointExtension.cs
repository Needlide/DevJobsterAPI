using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.Common;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.DatabaseModels.RequestModels.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace DevJobsterAPI.Controllers;

public static class AuthEndpointExtension
{
    public static WebApplication MapAuthEndpoint(this WebApplication app)
    {
        var authGroup = app.MapGroup("/api/auth").RequireCors("AllowFrontend");
        
        authGroup.MapPost("/login",
            async Task<Results<Ok<ApiResponse<TokenResponse>>, UnauthorizedHttpResult>> (
                LoginRegisterModel loginRegisterModel, 
                IUserManagementService userService,
                IConfiguration configuration) =>
            {
                var result = await userService.ValidateUserAsync(loginRegisterModel);
                if (!result.Success || result.UserId == null || result.UserId == Guid.Empty || result.UserType == null)
                    return TypedResults.Unauthorized();

                var token = GenerateJwtToken(result.UserId.Value, result.UserType.Value, configuration);
                return TypedResults.Ok(ApiResponseFactory.Success(
                    new TokenResponse(token), 
                    "Authentication successful"));
            }).AllowAnonymous();

        return app;
    }

    private static string GenerateJwtToken(Guid userId, UserType userType, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.Role, userType.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}