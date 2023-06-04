using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Extensions;
using CryptonRemoteBack.Infrastructure;
using System.Text;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

string? dbConnString = builder.Configuration
    .GetConnectionString("DbConnString")
    ?? "Server=localhost;Port=5432;Database=cryptondatabase;User Id=postgres;Password=postgres;";
builder.Services.AddNpgsqlDbContext<CryptonRemoteBackDbContext>(dbConnString);

#region Authorization

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequiredLength = 1;
    opts.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<CryptonRemoteBackDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:Audience"],
        ValidIssuer = configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:Secret"]
                ?? "111111111111111111111111111111111111111111111111")),
    };
});

#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddSingleton<IConfiguration>(configuration); ;

#region quartz

//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();

//    q.ScheduleJob<NetLabParserSyncJob>(trigger => trigger
//        .WithIdentity("NetLabParser trigger")
//        .StartNow()
//        .WithSimpleSchedule(x => x.WithIntervalInSeconds(5)));
//});

//builder.Services.AddQuartzHostedService(options =>
//{
//    options.WaitForJobsToComplete = true;
//});

#endregion

builder.Services.AddCors();
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors(x => x
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)); // allow credentials

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.RunDatabaseMigrations();
app.Run();
