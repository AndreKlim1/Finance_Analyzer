using Microsoft.AspNetCore.HttpOverrides;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Security.Claims;
using FluentValidation;
using System.Text;
using BudgetingService.Extensions;
using BudgetingService.Repositories;
using BudgetingService.Repositories.Interfaces;
using BudgetingService.Repositories.Implementations;
using BudgetingService.Services.Interfaces;
using BudgetingService.Services.Implementations;
using BudgetingService.Services.Validators;
using Microsoft.EntityFrameworkCore;
using BudgetingService.Messaging;
using BudgetingService.Messaging.BackgroundServices;
using BudgetingService.Messaging.Services;
using BudgetingService.Messaging.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBudgetRepository, BudgetRepository>();

builder.Services.AddTransient<IBudgetService, BudgetService>();
builder.Services.AddTransient<IKafkaBudgetService, KafkaBudgetService>();
builder.Services.AddHttpClient<ICurrencyConversionClient, CurrencyConversionClient>();
builder.Services.AddHttpClient<ITransactionsClient, TransactionsClient>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBudgetRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBudgetRequestValidator>();

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
var dbContextOptions = new DbContextOptionsBuilder<BudgetingServiceDbContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).Options;
var context = new BudgetingServiceDbContext(dbContextOptions);
context.Database.Migrate();
builder.Services.AddDbContext<BudgetingServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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
