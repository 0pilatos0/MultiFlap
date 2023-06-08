using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Server;
using Server.Hubs;
using Server.Services;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
	   builder =>
	   {
		   builder.WithOrigins("http://localhost:4040")
				  .WithHeaders("Authorization");
	   });
});

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
		  .AddJwtBearer(options =>
		  {
			  options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
			  options.Audience = builder.Configuration["Auth0:Audience"];

			  options.Events = new JwtBearerEvents
			  {
				  OnChallenge = context =>
				  {
					  context.Response.OnStarting(async () =>
					  {
						  await context.Response.WriteAsync(
							  JsonSerializer.Serialize(new ApiResponse("You are not authorized!"))
							  );
					  });

					  return Task.CompletedTask;
				  }
			  };
		  });

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy =>
		  policy.RequireAssertion(context =>
			  context.User.HasClaim(c =>
				  (c.Type == "permissions" &&
				  c.Value == "read:admin-messages") &&
				  c.Issuer == $"https://{builder.Configuration["Auth0:Domain"]}/")));

});

builder.Services.AddControllers();
builder.Services.AddDbContext<MultiFlapDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("MultiFlapDb"));
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IGameService, GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

if(!app.Environment.IsDevelopment())
	app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<GameHub>("/game"); 

app.Run();
