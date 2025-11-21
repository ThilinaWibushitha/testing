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
builder.Services.AddDbContext<PosavanceInventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<FranchiseManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FranchiseConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ShiftCloseNotification>();
builder.Services.AddScoped<ISchema, DynamicSchema>();
builder.Services.AddScoped<ICrudTbl_Items, CrudTbl_Items>();
builder.Services.AddSingleton<SharedStateService>();

builder.Services.AddSingleton<ITransactionUploadService, TransactionUploadService>();

// Register the background service
builder.Services.AddHostedService<TransactionSyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
