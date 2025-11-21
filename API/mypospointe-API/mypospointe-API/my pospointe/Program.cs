using Microsoft.EntityFrameworkCore;
using my_pospointe.Models;
using my_pospointe.Services;
using my_pospointe.Services.DynamicColumn;
using my_pospointe.Services.DynamicColumn.CRUD;
using my_pospointe_api.Services;
using my_pospointe_api.Services.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContexts
var defaultConnectionFromEnv = Environment.GetEnvironmentVariable("DefaultConnection");
var defaultConnectionFromConfig = builder.Configuration.GetConnectionString("DefaultConnection");
var defaultConnection = (!string.IsNullOrWhiteSpace(defaultConnectionFromEnv)) ? defaultConnectionFromEnv
    : (!string.IsNullOrWhiteSpace(defaultConnectionFromConfig)) ? defaultConnectionFromConfig
    : null;

var franchiseConnectionFromEnv = Environment.GetEnvironmentVariable("FranchiseConnection");
var franchiseConnectionFromConfig = builder.Configuration.GetConnectionString("FranchiseConnection");
var franchiseConnection = (!string.IsNullOrWhiteSpace(franchiseConnectionFromEnv)) ? franchiseConnectionFromEnv
    : (!string.IsNullOrWhiteSpace(franchiseConnectionFromConfig)) ? franchiseConnectionFromConfig
    : null;

var hasValidDefaultConnection = !string.IsNullOrWhiteSpace(defaultConnection);
var hasValidFranchiseConnection = !string.IsNullOrWhiteSpace(franchiseConnection);

if (hasValidDefaultConnection)
{
    builder.Services.AddDbContext<PosavanceInventoryContext>(options =>
        options.UseSqlServer(defaultConnection!));
}
else
{
    builder.Services.AddDbContext<PosavanceInventoryContext>(options =>
        options.UseInMemoryDatabase("POSAvanceInventory"));
}

if (hasValidFranchiseConnection)
{
    builder.Services.AddDbContext<FranchiseManagementContext>(options =>
        options.UseSqlServer(franchiseConnection!));
}
else
{
    builder.Services.AddDbContext<FranchiseManagementContext>(options =>
        options.UseInMemoryDatabase("FranchiseManagement"));
}

if (!hasValidDefaultConnection || !hasValidFranchiseConnection)
{
    Console.WriteLine("WARNING: Database connection strings not configured. Using in-memory database.");
    Console.WriteLine("Set DefaultConnection and FranchiseConnection environment variables to use SQL Server.");
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ShiftCloseNotification>();
builder.Services.AddScoped<ISchema, DynamicSchema>();
builder.Services.AddScoped<ICrudTbl_Items, CrudTbl_Items>();
builder.Services.AddSingleton<SharedStateService>();

builder.Services.AddSingleton<ITransactionUploadService, TransactionUploadService>();
builder.Services.AddScoped<IShiftSyncService, ShiftSyncService>();

// Register the background service
builder.Services.AddHostedService<TransactionSyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
