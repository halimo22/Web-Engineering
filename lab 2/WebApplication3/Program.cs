using WebApplication3.Interfaces;
using WebApplication3.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IBookService, BookService>();


var app = builder.Build();
app.MapControllers();


app.Run();
