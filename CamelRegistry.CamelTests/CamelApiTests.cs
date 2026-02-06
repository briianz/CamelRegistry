using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CamelRegistry.Models;

namespace CamelRegistry.Tests
{
    public class CamelApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //Http kliens létrehozása a tesztekhez
        private readonly HttpClient _client;
        public CamelApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }


        //Érvénytelen ID-val rendelkező teve lekérdezése tesztelése
        [Fact]
        public void GetCamel_WithInvalidId_ShouldReturnNotFound()
        {
            // Nem létező teve ID
            var response = _client.GetAsync("/api/camels/999").Result;

            // Ellenőrizzük, hogy a válasz státuszkódja 404 Not Found
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        //Érvényes ID-val rendelkező teve törlésének tesztelése
        [Fact]
        public void DeleteCamel_WithValidId_ShouldReturnNoContent()
        {
            // Először létrehozunk egy új tevéket a teszteléshez
            var newCamel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 1,
                LastFed = DateTime.Now
            };
            var createResponse = _client.PostAsJsonAsync("/api/camels", newCamel).Result;
            createResponse.EnsureSuccessStatusCode();
            var createdCamel = createResponse.Content.ReadFromJsonAsync<Camel>().Result;

            // Ezután töröljük a létrehozott tevéket
            var deleteResponse = _client.DeleteAsync($"/api/camels/{createdCamel.Id}").Result;

            // Ellenőrizzük, hogy a válasz státuszkódja 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }


        //Érvényes teve létrehozásának tesztelése
        [Fact]
        public void CreateCamel_WithValidData_ShouldReturnCreated()
        {
            var newCamel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 1,
                LastFed = DateTime.Now
            };

            var response = _client.PostAsJsonAsync("/api/camels", newCamel).Result;

            // Ellenőrizzük, hogy a válasz státuszkódja 201 Created
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Ellenőrizzük, hogy a válasz tartalmazza a létrehozott teve adatait
            var createdCamel = response.Content.ReadFromJsonAsync<Camel>().Result;
            Assert.NotNull(createdCamel);
            Assert.Equal(newCamel.Name, createdCamel.Name);
            Assert.Equal(newCamel.HumpCount, createdCamel.HumpCount);
        }

        //Érvénytelen teve létrehozásának tesztelése
        [Fact]
        public void CreateCamel_WithInvalidData_ShouldReturnBadRequest()
        {
            var newCamel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 3,
                LastFed = DateTime.Now
            };

            var response = _client.PostAsJsonAsync("/api/camels", newCamel).Result;

            // Ellenőrizzük, hogy a válasz státuszkódja 400 Bad Request
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //Módosított teve adatainak tesztelése
        [Fact]
        public void UpdateCamel_WithValidData_ShouldReturnOk()
        {
            // Először létrehozunk egy új tevéket a teszteléshez
            var newCamel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 1,
                LastFed = DateTime.Now
            };
            var createResponse = _client.PostAsJsonAsync("/api/camels", newCamel).Result;
            createResponse.EnsureSuccessStatusCode();
            var createdCamel = createResponse.Content.ReadFromJsonAsync<Camel>().Result;

            // Ezután módosítjuk a létrehozott tevéket
            createdCamel.Name = "Módosított Teve";
            var updateResponse = _client.PutAsJsonAsync($"/api/camels/{createdCamel.Id}", createdCamel).Result;

            // Ellenőrizzük, hogy a válasz státuszkódja 200 OK
            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            // Ellenőrizzük, hogy a válasz tartalmazza a módosított teve adatait
            var updatedCamel = updateResponse.Content.ReadFromJsonAsync<Camel>().Result;
            Assert.NotNull(updatedCamel);
            Assert.Equal("Módosított Teve", updatedCamel.Name);
        }
    }
}
