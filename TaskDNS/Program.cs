using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using TaskDNS.Application.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using TaskDNS.Database.Repository;
using TaskDNS.Network.SignalRHub;
using TaskDNS.Domain.Interface;
using TaskDNS.Domain.Model;
using TaskDNS.Database;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "Policy";
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var JwtProperty = builder.Configuration.GetSection("Property-Jwt");

builder.Services.Configure<PropertyJWT>(JwtProperty);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        });
});

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddSignalR();
builder.Services.AddHostedService<MessageSender>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JwtProperty["Issuer"],
        ValidAudience = JwtProperty["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtProperty["Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => 
{
    options.Cookie.HttpOnly = false;
    options.Cookie.Name = "Cookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.LoginPath = new PathString("/auth/login");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme,CookieAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
});

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(connection),ServiceLifetime.Singleton);

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenRepository,TokenRepository>();
builder.Services.AddSingleton<ICommandRepository,CommandRepository>();

builder.Services.AddSingleton<ChatHub>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.UseWebSockets();
app.UseSession();
app.UseMvc();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints => endpoints.MapHub<ChatHub>("/chat"));

app.Run();
