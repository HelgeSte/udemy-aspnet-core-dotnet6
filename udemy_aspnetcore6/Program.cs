var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// cannot use GetEndpoints here
app.UseRouting(); // just enabled routing

// Executes the appropriate endpoint based on teh endpoint selected 
// by the above UseRouting() method
app.UseEndpoints(endpoints =>
{
    /* Create your end points here
    // Can use endpoints.MapGet, MapControllers, MapPost, etc
    //endspoints.Map(...);
    //endspoints.MapGet(...);
    //endspoints.MapPost(...);*/
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files - {filename} - {extension}");
    });

    endpoints.Map("employee/PROFILE/{employeename=helge}", async context =>
    {
        string? employeename = Convert.ToString(context.Request.RouteValues["EMPLOYEEname"]); // case insensitive
        await context.Response.WriteAsync($"In employees - {employeename}");
    });

    //Eg: products/details/1
    endpoints.Map("products/details/{id?}", async context => // ? = optional value
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });
}); 

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
