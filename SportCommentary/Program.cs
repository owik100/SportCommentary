using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using SportCommentary.Areas.Identity;
using SportCommentary.Data;
using SportCommentary.Repository;
using SportCommentary.Repository.Interfaces;
using SportCommentary.Service;
using SportCommentary.Service.Interfaces;
using SportCommentaryDataAccess.DTO.SportType;
using SportCommentaryDataAccess.Entities;
using System;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

}
    ).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddScoped<ISportTypeRepository, SportTypeRepository>();
builder.Services.AddScoped<ISportTypeService, SportTypeService>();

builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Logging.AddLog4Net();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PolicyAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();