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
    endpoints.Map("products/details/{id:decimal?}", async context => // ? = optional value / :int => int is a constraint
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            decimal id = Convert.ToDecimal(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });

    // Eg: daily-digest-report/{reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}",
        async context => {
            DateTime reportDate = Convert.ToDateTime
                (context.Request.RouteValues["reportdate"]);
            await context.Response.WriteAsync($"in daily-digest-report {reportDate.ToShortDateString()}");
        });

    // Eg: cities/cityid
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!); // ! = cannot be null
        await context.Response.WriteAsync($"City information - {cityId}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
