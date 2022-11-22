using forum.Models;
using forum.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

//  跨域
 builder.Services.AddCors(options =>
{
options.AddPolicy
    (name: "myCors",
        builde =>
{
builde.WithOrigins("*", "*", "*")
.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod();
}
    );
});

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ForumContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ForumDatabase")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
    {
        //未登入時會自動導到這個網址
        option.LoginPath = new PathString("/api/Login/NoLogin");
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.ConfigureSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "abc",
        Version = "v1"
    });
});
builder.Services.AddSwaggerGen();


//  註冊jwt
builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));

//jwt 校驗

#region jwtCheck HS
{
    JWTTokenOptions tokenOptions = new JWTTokenOptions();
    builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = tokenOptions.Audience,
            ValidIssuer = tokenOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
        };
    });
}
#endregion

var app = builder.Build();


//順序要一樣
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseCors("myCors");
app.UseHttpsRedirection();

//授權
#region
app.UseAuthentication();
app.UseAuthorization();
#endregion
//app.UseAuthorization();
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();
