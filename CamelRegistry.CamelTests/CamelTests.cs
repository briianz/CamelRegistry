using System.ComponentModel.DataAnnotations;
using CamelRegistry.Models;
using Xunit;

namespace CamelRegistry.Tests
{
    public class CamelTests
    {

        //Érvénytelen púpszám tesztelése
        [Fact]
        public void Camel_WithThreeHumps_ShouldBeInvalid()
        {

            var camel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 3, // Érvénytelen púp szám
                LastFed = DateTime.Now
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(camel, new ValidationContext(camel), validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("A tevének csak 1 vagy 2 púpja lehet."));
        }

        //Érvénytelen név tesztelése
        [Fact]
        public void Camel_WithEmptyName_ShouldBeInvalid()
        {
            var camel = new Camel
            {
                Name = "", // Érvénytelen név
                HumpCount = 1,
                LastFed = DateTime.Now
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(camel, new ValidationContext(camel), validationResults, true);

            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("A név mező nem lehet üres."));
        }

        // Érvényes etetési idő tesztelése
        [Fact]
        public void Camel_WithValidLastFed_ShouldBeValid()
        {
            var camel = new Camel
            {
                Name = "Teszt Teve",
                HumpCount = 1,
                LastFed = DateTime.Now // Érvényes etetési idő
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(camel, new ValidationContext(camel), validationResults, true);
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        // Érvényes teve tesztelése
        [Fact]
        public void Camel_WithValidData_ShouldBeValid()
        {
            var camel = new Camel
            {
                Name = "Érvényes Teve",
                HumpCount = 2,
                LastFed = DateTime.Now
            };
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(camel, new ValidationContext(camel), validationResults, true);
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
    }


}
