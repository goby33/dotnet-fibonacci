using Leonardo;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Fibonacci",
     () => Fibonacci.RunAsync(new []{"44", "43"}));

app.Run();
