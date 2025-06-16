using ShopApi.Data.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShopApi.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_Name_IsRequired()
        {
            // Arrange
            var product = new Product
            {
                Description = "Test description",
                Price = 10.99m,
                CategoryId = 1
            };

            // Act
            var validationResults = ValidateModel(product);

            // Assert
            Assert.Contains(validationResults, 
                v => v.MemberNames.Contains("Name") && 
                     v.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_Description_IsRequired()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 10.99m,
                CategoryId = 1
            };

            // Act
            var validationResults = ValidateModel(product);

            // Assert
            Assert.Contains(validationResults, 
                v => v.MemberNames.Contains("Description") && 
                     v.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void Product_Price_MustBeGreaterThanZero()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test description",
                Price = 0,
                CategoryId = 1
            };

            // Act
            var validationResults = ValidateModel(product);

            // Assert
            Assert.Contains(validationResults, 
                v => v.MemberNames.Contains("Price") && 
                     v.ErrorMessage.Contains("Range"));
        }

        [Fact]
        public void Product_CanBeCreated_WithValidProperties()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Description = "Test description",
                Price = 10.99m,
                CategoryId = 1
            };

            // Act
            var validationResults = ValidateModel(product);

            // Assert
            Assert.Empty(validationResults);
        }

        private static List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}