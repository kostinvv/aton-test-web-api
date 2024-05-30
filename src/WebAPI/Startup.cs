using Microsoft.EntityFrameworkCore;
using WebAPI.Abstractions.Context;
using WebAPI.Abstractions.Services;
using WebAPI.ApiExtensions;
using WebAPI.Data;
using WebAPI.ExceptionHandlers;
using WebAPI.Services;

namespace WebAPI;

public static class Startup
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<BusinessExceptionHandler>();
        
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerConfiguration();

        builder.Services.AddProblemDetails();
        
        builder.Services.AddAuthConfiguration(builder.Configuration);
        
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder 
            => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IUserDbContext>(provider 
            => provider.GetService<ApplicationDbContext>()!);
        
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        
        builder.Services.AddScoped<IUserService, UserService>();
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        return app;
    }
}