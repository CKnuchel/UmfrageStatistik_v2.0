using Common.Models;
using Data.Context;
using Logic.DataLoader;
using Logic.Interfaces;
using Logic.Repository;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

// Hinzufügen der eigenen Services
builder.Services.AddScoped<IRepository<Modul>, ModulRepository>();
builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
builder.Services.AddScoped<IRepository<Answer>, AnswerRepository>();
builder.Services.AddScoped<IRepository<Response>, ResponseRepository>();
builder.Services.AddScoped<IBasicLoader, StandardLoader>();
builder.Services.AddScoped<IFilterLoader, FilteredLoader>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Auslesen der Verbindung aus dem appsettings.json

builder.Services.AddDbContext<UmfrageContext>(options => // Hinzufügen von unserem Context in die Services des Web
                                              {
                                                  if(connectionString == null) return;
                                                  options.UseSqlServer(connectionString); // Definieren der Verbindung fuer zum SQL Server
                                              });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();