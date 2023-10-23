using Minesweeper.Models;
using Minesweeper.Service;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddSingleton<GameInfoResponse>();
builder.Services.AddSingleton<FieldRepository>();
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
					  policy =>
					  {
						  policy.WithOrigins("https://localhost:44376/",
											  "https://minesweeper-test.studiotg.ru/")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowAnyOrigin();
					  });
});
// Add services to the container.
builder.Services.AddControllers()
	.AddNewtonsoftJson(opt =>
	{
		opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
	});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
