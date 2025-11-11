using ExpenseTracker.Data;
using ExpenseTracker.Extensions;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using ExpenseTracker.Repos;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(op =>
op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(
  op =>
    {
        op.Password.RequireDigit = false;
        op.Password.RequireLowercase = false;
        op.Password.RequireUppercase = false;
        op.Password.RequireNonAlphanumeric = false;
        op.Password.RequiredLength = 6;
    }
)
.AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddCustomJwtAuth(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
// builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddSwaggerGenJwtAuth();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

