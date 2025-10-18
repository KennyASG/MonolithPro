using MonolithPro.Modules.Users;
using MonolithPro.Modules.Orders;
using MonolithPro.Modules.Inventory;
using MonolithPro.Shared;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar servicios compartidos
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddSingleton<LoggerService>();
builder.Services.AddSingleton<EmailService>();

// Registrar servicios de módulos
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<InventoryService>();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();