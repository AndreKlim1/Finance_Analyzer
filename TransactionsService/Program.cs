using Microsoft.AspNetCore.HttpOverrides;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Security.Claims;
using UsersService.Services.Validators;
using System.Text;
using FluentValidation;
using TransactionsService.Extensions;
using TransactionsService.Repositories;
using TransactionsService.Repositories.Interfaces;
using TransactionsService.Repositories.Implementations;
using TransactionsService.Services.Interfaces;
using TransactionsService.Services.Implementations;
using TransactionsService.Services.Validators;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();;

builder.Services.AddTransient<ITransactionService, TransactionService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTransactionRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTransactionRequestValidator>();

builder.Services.AddFluentValidationAutoValidation();

#region

builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

//options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
//builder.Services.AddControllers().AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

#endregion

var dbContextOptions = new DbContextOptionsBuilder<TransactionsServiceDbContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).Options;
var context = new TransactionsServiceDbContext(dbContextOptions);
context.Database.Migrate();
builder.Services.AddDbContext<TransactionsServiceDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


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
