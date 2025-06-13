using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Dapper;
using DevJobsterAPI.ApiModels;
using DevJobsterAPI.Common;
using DevJobsterAPI.Common.Exceptions.DatabaseExceptions;
using DevJobsterAPI.Controllers;
using DevJobsterAPI.Database;
using DevJobsterAPI.Database.Abstract;
using DevJobsterAPI.Database.Services;
using DevJobsterAPI.DatabaseModels.RequestModels.User.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateSlimBuilder(args);

// Telling Dapper to like our naming
DefaultTypeMap.MatchNamesWithUnderscores = true;
// and to correctly map our UserType enum to string
SqlMapper.AddTypeHandler(new UserTypeHandler());

// JSON setup
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Database setup
var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
const string secretName = "DevJobsterDB";

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
KeyVaultSecret secret = client.GetSecret(secretName);

builder.Configuration["ConnectionStrings:DevJobsterDB"] = secret.Value;

builder.Services.AddScoped<IDbContext, DbContext>(_ =>
    new DbContext(builder.Configuration.GetConnectionString("DevJobsterDB")));

builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserSpaceService, UserSpaceService>();

// FluentValidation setup
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(UserRegistrationValidator).Assembly);

// Authorization/authentication setup
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                
                var response = ApiResponseFactory.Fail<object>("Unauthorized", "UNAUTHORIZED");
                
                return context.Response.WriteAsJsonAsync(response);
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorizationBuilder();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RecruiterOnly", policy => policy.RequireRole("Recruiter"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("RecruiterAndAdminOnly", policy => policy.RequireRole("Recruiter", "Admin"));
    options.AddPolicy("UserAndAdminOnly", policy => policy.RequireRole("Admin", "User"));
    options.AddPolicy("UserAndRecruiterOnly", policy => policy.RequireRole("Recruiter", "User"));

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://needlide.github.io")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
    
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        
        var response = new ApiResponse<object>
        {
            Success = false,
            Data = null
        };

        switch (exception)
        {
            case UniqueConstraintViolationException ex:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response.ErrorCode = "DUPLICATE_ENTITY";
                response.Message = ex.Message;
                break;

            case ForeignKeyViolationException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.ErrorCode = "INVALID_REFERENCE";
                response.Message = ex.Message;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorCode = "SERVER_ERROR";

#if DEBUG
                response.Message = exception?.Message ?? string.Empty;
#else
                    response.Message = "An unexpected error occurred";
#endif

                break;
        }

        context.Response.ContentType = "application/json";

        await JsonSerializer.SerializeAsync(context.Response.Body, response,
            AppJsonSerializerContext.Default.ApiResponseObject);
    }));

app.UseRouting();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAdminEndpoint();
app.MapApplicationEndpoint();
app.MapAuthEndpoint();
app.MapChatsEndpoint();
app.MapMessagesEndpoint();
app.MapRecruiterEndpoint();
app.MapUserEndpoint();
app.MapVacancyEndpoint();

await app.RunAsync();