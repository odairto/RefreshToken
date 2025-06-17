using Microsoft.AspNetCore.Authentication.Cookies;
using RefreshToken.Api.Interfaces;
using RefreshToken.Api.Middlewares;
using RefreshToken.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICookieService, CookieService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
