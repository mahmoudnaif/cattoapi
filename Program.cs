

using cattoapi.Interfaces;
using cattoapi.Interfaces.Comments;
using cattoapi.Interfaces.Likes;
using cattoapi.Interfaces.Posts;
using cattoapi.Models;
using cattoapi.Repos;
using cattoapi.Repos.Commetns;
using cattoapi.Repos.Likes;
using cattoapi.Repos.Posts;
using cattoapi.utlities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
});


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<PasswordService>();
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
