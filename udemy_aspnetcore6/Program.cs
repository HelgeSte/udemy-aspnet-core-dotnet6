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

    // endpoints.Map("employee/PROFILE/{employeename:minlength(4):maxlength(7)=helge}", async context =>
    endpoints.Map("employee/PROFILE/{employeename:length(4,7):alpha=helge}", async context =>
    {
        string? employeename = Convert.ToString(context.Request.RouteValues["employeename"]); // case insensitive
        await context.Response.WriteAsync($"In employees - {employeename}");
    });

    //Eg: products/details/1
    //endpoints.Map("products/details/{id:int:min(1):max(1000)?}", async context => // ? = optional value / :int => int is a constraint
    endpoints.Map("products/details/{id:int:range(1,1000)?}", async context => 
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
    
    // Sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan|dec|may|feb|jun)$)}/", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if (month == "jan" || month == "apr")
        {
            await context.Response.WriteAsync($"First half of the year sales report - {year} - {month}");
        } else if (month == "jul" || month == "oct")  {
            await context.Response.WriteAsync($"Second half of the year sales report - {year} - {month}");
        } else if(month == "may")  {
            await context.Response.WriteAsync($"May is an awesome month");
        }
        else
        {
            // Easier to give a more meaningful error message, but doing it here instead of a
            // app.Run function
            await context.Response.WriteAsync($"This is an awful month and is not allowed");
        }
    });
    {
        
    }
    
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});
app.Run();
