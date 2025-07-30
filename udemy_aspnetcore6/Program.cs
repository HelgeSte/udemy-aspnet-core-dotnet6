var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// cannot use GetEndpoints here
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint 1: {endPoint.DisplayName}\n");
    }
    else
    {
        Console.WriteLine("Endpoint 1 is null");
    }
    await next(context);
});

app.UseRouting(); // just enabled routing
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint(); // use /map1, map2/, etc
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint 2: {endPoint.DisplayName}\n");
    } else
    {
        Console.WriteLine("Endpoint 2 is null");
    }
    
    await next(context);
});

// Executes the appropriate endpoint based on teh endpoint selected 
// by the above UseRouting() method
app.UseEndpoints(endpoints =>
{
    // Create your end points here
    // Can use endpoints.MapGet, MapControllers, MapPost, etc
    //endspoints.Map(...);
    //endspoints.MapGet(...);
    //endspoints.MapPost(...);
    endpoints.Map("/map1", async (context) => // context = request deletegate / middleware
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapPost("/map2", async (context) => // context = request deletegate / middleware
    {
        await context.Response.WriteAsync("In Map 2");
    });

    endpoints.MapGet("/map3", async (context) => // context = request deletegate / middleware
    {
        await context.Response.WriteAsync("In Map 3");
    });
}); 

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
