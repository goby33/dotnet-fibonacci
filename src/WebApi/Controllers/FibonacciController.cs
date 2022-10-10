using Leonardo;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FibonacciController : ControllerBase
{
    
    
    [HttpPost(Name = "PostFibonacci")]
    public async Task<IList<long>> Run(string[] args,[FromServices] ILogger<WeatherForecastController> logger, [FromServices] Fibonacci fibonacci)
    {
        logger.LogInformation("youyou");
        return await fibonacci.RunAsync(args);
    }
}