var env = Environment.GetEnvironmentVariable("ENVIRONMENT");
if (env == "Development")
{
    DotNetEnv.Env.Load(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env"));
}
;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://" + EnvSettings.LoadTokenApiURL());

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
