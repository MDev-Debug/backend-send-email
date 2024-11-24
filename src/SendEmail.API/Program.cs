using SendEmail.CrossCutting.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddDependencyInjection();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.AddEnvironmentDependencyInjection();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
