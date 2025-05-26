using Microsoft.AspNetCore.HttpOverrides;
using NotificationsService;
using NotificationsService.Extensions;
using NotificationsService.Messaging;
using NotificationsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("KafkaSettings"));


builder.Services.AddSingleton<KafkaConsumer<string, string>>();
builder.Services.AddHostedService<NotificationEventsConsumer>();


builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddControllers();
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();

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
app.MapHub<NotificationHub>("/notifications");
app.MapControllers();

app.Run();
