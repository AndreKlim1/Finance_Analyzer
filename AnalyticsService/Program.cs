using AnalyticsService.Extensions;
using AnalyticsService.Messaging.Http.Implementations;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Services.Implementations;
using AnalyticsService.Services.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();

builder.Services.AddTransient<IAccountAnalyticsService, AccountAnalyticsService>();
builder.Services.AddTransient<IAnalyticsDashboardService, AnalyticsDashboardService>();
builder.Services.AddTransient<IBudgetAnalyticsService, BudgetAnalyticsService>();
builder.Services.AddTransient<ISpendingPatternsReportService, SpendingPatternsReportService>();
builder.Services.AddTransient<IIncomeExpenseAnalyticsService, IncomeExpenseAnalyticsService>();

builder.Services.AddHttpClient<IAccountsClient, AccountsClient>();
builder.Services.AddHttpClient<ITransactionsClient, TransactionsClient>();
builder.Services.AddHttpClient<ICurrencyConversionClient, CurrencyConversionClient>();
builder.Services.AddHttpClient<IBudgetsClient, BudgetsClient>();



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