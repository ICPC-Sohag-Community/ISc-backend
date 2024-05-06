using Hangfire;
using ISc.Api;
using ISc.Application.Extension;
using ISc.Infrastructure.Extension;
using ISc.Presistance.Extension;
using ISc.Presistance.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresistance(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.DependencyInjectionService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(cores => cores.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangFireDashboard");
DataSeeding.Initialize(app.Services.CreateScope().ServiceProvider);
app.MapControllers();

app.Run();
