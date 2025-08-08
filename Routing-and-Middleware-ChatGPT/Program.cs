var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
 
/* The app runs this two one more time, after it has run it's endpoint, if it's called from a browser.
 This is because browsers can be "sneaky" and make extra requests that postman doesn't. This extra
 request is often for favicon. */
// Middleware 1: Before Routing
app.Use(async (context, next) => 
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    }
    await next(context);/* if some code comes after this next function, it will run after the
    "next" endpoint is finished */
});
 
// Enable Routing Middleware
app.UseRouting();
 
// Middleware 2: After Routing
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endPoint = context.GetEndpoint();
    if (endPoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endPoint.DisplayName}\n");
    }
    await next(context);/* if some code comes after this next function, it will run after the
    "next" endpoint is finished */
});
/**** The two app.Use is often run again, when it's called from a browser. This is often a favicon request. */

// Creating Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("map1", async (context) => 
    {
        await context.Response.WriteAsync("In Map 1");
    });
 
    endpoints.MapPost("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
});
 
// Fallback Middleware
app.Run(async context => 
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
 
app.Run();