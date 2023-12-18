using Dapper;
using MassTransit;
using ProjectHorizon.Microservice.Account.Components.Base;
using ProjectHorizon.Microservice.Account.Components.Consumers;
using ProjectHorizon.Microservice.Account.Components.Repositories;
using ProjectHorizon.Shared.Library.Helper;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                             builder =>
                             {
                          builder.WithOrigins("http://localhost:4200")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(builder =>
{
    builder.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediator(cfg =>
{
    cfg.AddConsumer<AccountConsumer>();
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDependencies, Dependencies>();

builder.Services.AddSingleton<IConnectionStringHelper>(s => new ConnectionStringHelper(s.GetRequiredService<IConfiguration>().GetConnectionString("AccountDB")));
builder.Services.AddScoped<IDatabaseHelper, DatabaseHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
