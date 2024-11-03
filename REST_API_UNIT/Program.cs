using REST_API_UNIT.Extension;
using REST_API_UNIT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddScoped<IUnitService, UnitService>(sp
    => new UnitService(
        builder.Configuration.GetConnectionString("CosmosDB"),
        builder.Configuration["CosmosConfig:primaryKey"],
        builder.Configuration["CosmosConfig:databaseName"],
        builder.Configuration["CosmosConfig:containerName"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
