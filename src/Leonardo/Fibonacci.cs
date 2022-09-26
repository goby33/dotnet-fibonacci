using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public record FiboData {
    public int Input { get; set; }
    public int Output { get; set; }
    public bool IsFromCache { get; set; }
}
public static class Fibonacci
{
    public static int Run(int i)
    {
        if (i <= 2) return 1;
        return Run(i - 1) + Run(i - 2);
    }    
    
    public static async Task<List<int>> RunAsync(string[] args)    {
        using (var fibonacciDataContext = new  FibonacciDataContext())
        {
            var tasks = new List<Task<FiboData>>();
            foreach (var arg in args)
            {
                var resultBdd = await fibonacciDataContext.TFibonaccis.Where(f => f.FibInput == Convert.ToInt32(arg))
                    .Select(f => f.FibOutput).FirstOrDefaultAsync();
                if (resultBdd != default)
                {
                    tasks.Add( Task.FromResult(new  FiboData()
                    {
                        Input = Convert.ToInt32(arg),
                        Output = (int)resultBdd, 
                    }));
                }
                else
                {
                    var result = Task.Run(() =>
                    {
                        return new FiboData()
                        {
                            Input = Convert.ToInt32(arg),
                            Output = Fibonacci.Run(Convert.ToInt32(arg)),
                        };
                    });
                    tasks.Add(result);
                }
               
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var listOfResult in tasks) {
                if (!listOfResult.Result.IsFromCache)
                {
                    fibonacciDataContext.TFibonaccis.Add(new TFibonacci()
                    {
                        FibInput = listOfResult.Result.Input,
                        FibOutput = listOfResult.Result.Output,
                    });
                }  
            }
            await fibonacciDataContext.SaveChangesAsync();
            return tasks.Select(t=>t.Result.Output).ToList();
        }
    }     
}