using Microsoft.AspNetCore.HttpOverrides;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Security.Claims;
using CaregoryAccountService.Models.Enums;
using CaregoryAccountService.Repositories.Implementations;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Services.Implementations;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Validators;
using FluentValidation;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using CaregoryAccountService.Extensions;
using CaregoryAccountService.Repositories;
using Microsoft.EntityFrameworkCore;
using CategoryAccountService.Messaging.BackgroundServices;
using CategoryAccountService.Messaging.Kafka;
using CategoryAccountService.Messaging.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddHttpClient<ICurrencyConversionClient, CurrencyConversionClient>();


builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAccountRequestValidator>();

builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("KafkaSettings"));

builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<KafkaConsumer<string, string>>();
builder.Services.AddHostedService<TransactionEventConsumerService>();
builder.Services.AddHostedService<TransactionUpdateEventsConsumer>();

#region

builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

//options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
//builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

#endregion
var dbContextOptions = new DbContextOptionsBuilder<CategoryAccountServiceDbContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).Options;
var context = new CategoryAccountServiceDbContext(dbContextOptions);
context.Database.Migrate();
builder.Services.AddDbContext<CategoryAccountServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
context.Dispose();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.ConfigureExceptionHandler();

app.UseCors("CorsPolicy");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
