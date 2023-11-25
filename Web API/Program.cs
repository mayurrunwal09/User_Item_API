using Domain.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.ContextClass;
using Repository.Repository;
using Repository.Services.Custom.CategoryTypeService;
using Repository.Services.Custom.CustomerService;
using Repository.Services.Custom.ItemService;
using Repository.Services.Custom.SupplierService;
using Repository.Services.Custom.UserTypeService;
using Repository.Services.Generic;
using System.Net;
using System.Text;
using Web_API.Middleware;
using Web_API.Middleware.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
#region Database Connection
builder.Services.AddDbContext<ApplicationDbContextClass>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
#endregion

#region Dependency Injections
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
builder.Services.AddTransient(typeof(ICategoryTypeServices), typeof(CategoryTypeServices));
builder.Services.AddTransient(typeof(IUserTypeServices), typeof(UserTypeServices));
builder.Services.AddTransient(typeof(IItemServices), typeof(ItemServices));
builder.Services.AddTransient(typeof(ISupplierServices), typeof(SupplierServices));
builder.Services.AddTransient(typeof(ICustomerServices), typeof(CustomerServices));
builder.Services.AddTransient<IJWTAuthManager, JWTAuthManager>();

#endregion

#region CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigins", option => option.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
});
#endregion


// swagger.json file generation
// Commands
// dotnet new tool-manifest
// dotnet tool install --version 5.6.3 Swashbuckle.AspNetCore.Cli
#region  Swagger Configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo { Title = "Web.Api" , Version = "v1"});

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\nExample: \"Bearer 1safsfsdfdfd\""

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, Array.Empty<string>()
        }
    });
});

#endregion

#region Authrization and JWT configuration

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            // Call this to skip the default logic and avoid using the default response
            context.HandleResponse();
            // Write to the response in any way you wish
            context.Response.StatusCode = 401;
            // context.Response.Headers.Append("my-custom-header", "custom-value");

            Response<String> response = new()
            {
                Message = "You are not authorized!",
                Status = (int)HttpStatusCode.Unauthorized
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    };
});


#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandlerMiddleware();
app.UseRouting();
app.UseCors(option => option.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuthentication}/{action=Login}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

#region Static Files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"Images/User")),
    RequestPath = @"/Images/User"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"Images/Item")),
    RequestPath = @"/Images/Item"
});
#endregion

app.Run();
