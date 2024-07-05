

using cattoapi.Interfaces;
using cattoapi.Interfaces.BlackListTokens;
using cattoapi.Interfaces.Comments;
using cattoapi.Interfaces.EmailServices;
using cattoapi.Interfaces.Likes;
using cattoapi.Interfaces.Posts;
using cattoapi.Models;
using cattoapi.Models.EmailModel;
using cattoapi.Repos;
using cattoapi.Repos.BlackListTokens;
using cattoapi.Repos.Commetns;
using cattoapi.Repos.EmailServices;
using cattoapi.Repos.Likes;
using cattoapi.Repos.Posts;
using cattoapi.utlities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//JWT AUTH
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var authHeader =  context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Fail("Authorization header is missing or invalid.");
                return;
            }

            var tokenAsString = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(tokenAsString))
            {
                context.Fail("Invalid JWT token.");
                return;
            }
            var token = handler.ReadJwtToken(tokenAsString);

            int userId;

            string isVerficationToken = token.Claims.FirstOrDefault(c => c.Type == "Verify")?.Value;
            string isChnagePasswordToken = token.Claims.FirstOrDefault(c => c.Type == "changepassword")?.Value;


            if (isVerficationToken != null || isChnagePasswordToken != null)
            {
                userId = int.Parse(token.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
            }
            else
            {
                userId = int.Parse(token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
            }

           var tokenValidationService = context.HttpContext.RequestServices.GetRequiredService<IBlackListTokensRepo>();
            var iatClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(iatClaim)).UtcDateTime;

            if (await tokenValidationService.IsTokenBlacklisted(userId, issuedAt))
            {
                
                context.Fail("This token has been invalidated.");
            }
            else
            {
                context.Success();
            }
        }
    };



});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<PasswordService>();

builder.Services.AddScoped<IBlackListTokensRepo, BlackListTokensRepo>();
builder.Services.AddScoped<IEmailServicesRepo, EmailServicesRepo>();
builder.Services.AddScoped<ILikesRepo, LikesRepo>();
builder.Services.AddScoped<ICommentsRepo, CommentsRepo>();
builder.Services.AddScoped<IPostsRepo, PostsRepo>();
builder.Services.AddScoped<IUserOperationsRepo, UserOperationsRepo>();
builder.Services.AddScoped<IAdminOperationsRepo, AdminOperationsRepo>();
builder.Services.AddScoped<IAccountRepo, Accountrepo>();
builder.Services.AddScoped<IAuthOperationsRepo, AuthOperationsRepo>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert JWT Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
}));

//add cache to blacklist tokens
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<CattoDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration["ConnectionStrings:default"]);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
