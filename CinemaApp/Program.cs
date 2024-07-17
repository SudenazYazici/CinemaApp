using CinemaApp.Data;
using CinemaApp.Interface;
using CinemaApp.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
builder.Services.AddScoped<ICinemaHallRepository, CinemaHallRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ICinemaMovieRepository, CinemaMovieRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(p => { p.AllowAnyOrigin(); p.AllowAnyMethod(); p.AllowAnyHeader(); });

app.UseAuthorization();

app.MapControllers();

app.Run();
