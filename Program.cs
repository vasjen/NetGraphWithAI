using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NetGraphWithAI.AIHelper;
using NetGraphWithAI.Graph;
using OpenAI_API;

var builder = WebApplication.CreateBuilder(args);
var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');
builder.Services.AddTransient<IConfiguration>(sp =>
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json")
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddUserSecrets<Program>();
            return configurationBuilder.Build();
        });



builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)

            
                .AddMicrosoftIdentityWebApp(builder.Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
                .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

    builder.Services
        .AddRazorPages()
        .AddMicrosoftIdentityUI();
    builder.Services.AddScoped<HttpClient>( (p) => 
     {
        var apiToken = p.GetService<IConfiguration>().GetValue<string>("OpenAIAPI");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("authorization", "Bearer "+apiToken);
        return client;
    });
    builder.Services.AddControllers();
    builder.Services.AddScoped<GraphEmailClient>();
    builder.Services.AddScoped<GraphProfileClient>();
    builder.Services.AddScoped<OpenAIAPI>( (p) =>
    {
         var apiToken = p.GetService<IConfiguration>().GetValue<string>("OpenAIAPI");
        var client = new OpenAIAPI(apiToken);
        return client;
    });
    builder.Services.AddScoped<IAIHelperService, AIHelperService>();


;
var app = builder.Build();
 app.UseHttpsRedirection();
 app.UseStaticFiles();
 app.UseRouting();
 app.UseAuthentication();
 app.UseAuthorization();
 app.MapRazorPages();
 app.UseEndpoints(c =>
     
     {c.MapControllerRoute(
         name: "default",
         pattern: "{controller=API}/{action=Index}/{id?}");  
     });           

app.Run();
