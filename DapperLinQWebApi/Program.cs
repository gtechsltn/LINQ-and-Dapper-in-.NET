using DapperLinQWebApi.Data;
using DapperLinQWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// add DI
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<OrderRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

