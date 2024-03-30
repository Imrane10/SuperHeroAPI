using Microsoft.Extensions.Options;
using SuperHeroAPI;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TestProject1
{
    public class SupperControlerTest
    {
        [Fact]
        public async Task Create()
        {
            //Arange
            var application = new SuperHeroFactory();
            var hero = new SuperHero
          {
                Name = "Spider-Man",
                FirstName = "Peter",
                LastName = "Parker",
                Place = "New York"
          };
            var client = application.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(hero), Encoding.UTF8, "application/json");
           // Act
            var response = await client.PostAsync("/api/SuperHero", content);

           // Assert
                   response.EnsureSuccessStatusCode();
           // Deserialize response content to check if hero was created
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdHeroes = JsonSerializer.Deserialize<List<SuperHero>>(responseContent);
        }
        [Fact]
        public async Task Update()
        {
            // Arrange
            var application = new SuperHeroFactory();
            var hero = new SuperHero
            {
                Name = "Spider-Man",
                FirstName = "Peter",
                LastName = "Parker",
                Place = "New York"
            };

            var client = application.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(hero), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsJsonAsync("/api/SuperHero", content);
            createResponse.EnsureSuccessStatusCode();
            if (createResponse.IsSuccessStatusCode)
            {
                var createdHeroJson = await createResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<SuperHero> superHeroes = JsonSerializer.Deserialize<List<SuperHero>>(createdHeroJson, options);

                foreach (var createdHero in superHeroes)
                {
                    createdHero.Name = "Iron Man";
                    var contentd = new StringContent(JsonSerializer.Serialize(createdHero), Encoding.UTF8, "application/json");

                  var updateResponse = await client.PutAsync("/api/SuperHero", new StringContent(JsonSerializer.Serialize(createdHero), Encoding.UTF8, "application/json"));


                    updateResponse.EnsureSuccessStatusCode();
                }
            }
            else
            {
            }
           

        }

        [Fact]
public async Task Delete()
{
    // Arrange
    var application = new SuperHeroFactory();
    var hero = new SuperHero
    {
        Name = "Spider-Man",
        FirstName = "Peter",
        LastName = "Parker",
        Place = "New York"
    };

            // Create the hero
            var client = application.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(hero), Encoding.UTF8, "application/json");
            var createResponse = await client.PostAsJsonAsync("/api/SuperHero", content);
            createResponse.EnsureSuccessStatusCode();

            // Get the created hero from the response
            if (createResponse.IsSuccessStatusCode)
            {
                var createdHeroJson = await createResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                List<SuperHero> superHeroes = JsonSerializer.Deserialize<List<SuperHero>>(createdHeroJson, options);

                foreach (var createdHero in superHeroes)
                {
                    var deleteResponse = await client.DeleteAsync($"/api/SuperHero/{createdHero.Id}");
                    deleteResponse.EnsureSuccessStatusCode();
                }
            }
            else
            {
            }

    
}

    }
}