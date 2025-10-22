using MonolithPro.Modules.Users;
using MonolithPro.Modules.Orders;
using MonolithPro.Modules.Inventory;
using MonolithPro.Shared;
using MonolithPro.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CONFIGURACIÓN DE ENTITY FRAMEWORK CORE
builder.Services.AddDbContext<MonolithProDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Servicios compartidos (SIN DatabaseContext)
builder.Services.AddSingleton<LoggerService>();
builder.Services.AddSingleton<EmailService>();

// Servicios de módulos (Scoped por DbContext)
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<InventoryService>();

var app = builder.Build();

// APLICAR MIGRACIONES AUTOMÁTICAMENTE
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MonolithProDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<LoggerService>();

    try
    {
        logger.Log("Checking for pending migrations...");

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            logger.Log("Applying migrations...");
            dbContext.Database.Migrate();
            logger.Log("✅ Migrations applied");
        }
        else
        {
            logger.Log("✅ Database up to date");
        }
    }
    catch (Exception ex)
    {
        logger.LogError($"❌ Migration error: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();