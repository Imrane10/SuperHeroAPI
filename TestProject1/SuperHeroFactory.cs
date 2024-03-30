using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SuperHeroAPI;
using SuperHeroAPI.Data;
using Xunit;

namespace TestProject1
{
    internal class SuperHeroFactory : WebApplicationFactory<Program>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll( typeof(DbContextOptions<DbContext>));
                    var connString = GetConnectionString();
                    services.AddSqlServer<DbContext>(connString);
                    var dbContext = CreateDbContext(services);
                   // dbContext.Database.EnsureDeleted();
                });
            }
        private static string? GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
             .AddUserSecrets< SuperHeroFactory>()
             .Build();
            var connString = configuration.GetConnectionString("test");
            return connString;
        }
        private static DbContext CreateDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
            return dbContext;
        }
    }
    //public class RegisterControllerIntegrationTests : IClassFixture<SuperHeroControllerIntegrationTests<Program>>
    //{
    //    private readonly HttpClient _client;
    //    public RegisterControllerIntegrationTests(SuperHeroControllerIntegrationTests<Program> factory)
    //    {
    //        _client = factory.CreateClient();
    //    }
     
    //    [Fact]
    //    public async Task CreateSuperHero_ReturnsSuccessAndHeroCreated()
    //    {

    //        // Create a sample SuperHero object
    //        var hero = new SuperHero
    //        {
    //            Name = "Spider-Man",
    //            FirstName = "Peter",
    //            LastName = "Parker",
    //            Place = "New York"
    //        };

    //        var content = new StringContent(JsonSerializer.Serialize(hero), Encoding.UTF8, "application/json");

    //        // Act
    //        var response = await _client.PostAsync("/api/SuperHero", content);

    //        // Assert
    //        response.EnsureSuccessStatusCode(); // Status Code 200-299

    //        // Deserialize response content to check if hero was created
    //        var responseContent = await response.Content.ReadAsStringAsync();
    //        var createdHeroes = JsonSerializer.Deserialize<List<SuperHero>>(responseContent);

    //        Assert.NotNull(createdHeroes);
    //        Assert.Contains(createdHeroes, h => h.Name == hero.Name && h.FirstName == hero.FirstName && h.LastName == hero.LastName && h.Place == hero.Place);
    //    }

    //}
}
