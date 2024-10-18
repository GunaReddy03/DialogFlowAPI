using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using static DialogFlowAPI.Models.Logins;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DialogFlowAPI.DbContext;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Google.Cloud.Dialogflow.Cx.V3;

var builder = WebApplication.CreateBuilder(args);
var credentialsPath = builder.Configuration["GoogleCredentialsPath"];
System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddSingleton<AgentsClient>(sp =>
{
    // Initializes the Dialogflow CX client
    return AgentsClient.Create();
});
builder.Services.AddSingleton<IntentsClient>(sp =>
{
    // Initializes the Dialogflow CX client
    return IntentsClient.Create();
});
builder.Services.AddSingleton<EntityTypesClient>(sp =>
{
    // Initializes the Dialogflow CX client
    return EntityTypesClient.Create();
});
builder.Services.AddSingleton<FlowsClient>(sp =>
{
    // Initializes the Dialogflow CX client
    return FlowsClient.Create();
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Dry-Cleaning", Version = "v1" });
    c.DescribeAllParametersInCamelCase();
    //c.DescribeAllEnumsAsStrings();
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please Insert token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference=new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        new string[]{}
        }
    });
});

builder.Services
    .AddDbContext<DialogFlowDbContext>(options => options.UseSqlServer(builder.Configuration!.GetConnectionString("DialogFlowdb")!).EnableSensitiveDataLogging());

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DialogFlowDbContext>()
    .AddDefaultTokenProviders();
//builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
//builder.Services.AddScoped<ICustomerService, CustomerService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IOrderPerWeightService, OrderPerWeightService>();
//builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddHttpContextAccessor();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})



// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});
//var credentialsPath = "C:\\Users\\HOME\\Downloads\\default-yrln-7ca437e6ea13.json";
//Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
