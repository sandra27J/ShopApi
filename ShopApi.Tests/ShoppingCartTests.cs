using ShopApi.Data.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ShopApi.Tests
{
    public class ShoppingCartTests
    {
        [Fact]
        public void ShoppingCart_User_IsRequired()
        {
            // Arrange
            var cart = new ShoppingCart();

            // Act
            var validationResults = ValidateModel(cart);

            // Assert
            Assert.Contains(validationResults, 
                v => v.MemberNames.Contains("User") && 
                     v.ErrorMessage.Contains("required"));
        }

        [Fact]
        public void ShoppingCart_Products_InitializesEmptyList()
        {
            // Arrange & Act
            var cart = new ShoppingCart();

            // Assert
            Assert.NotNull(cart.Products);
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void ShoppingCart_CanAddProduct()
        {
            // Arrange
            var cart = new ShoppingCart();
            var product = new Product { Id = 1, Name = "Test", Description = "Test", Price = 10m, CategoryId = 1 };

            // Act
            cart.Products.Add(product);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(1, cart.Products[0].Id);
        }

        [Fact]
        public void ShoppingCart_CanRemoveProduct()
        {
            // Arrange
            var cart = new ShoppingCart();
            var product = new Product { Id = 1, Name = "Test", Description = "Test", Price = 10m, CategoryId = 1 };
            cart.Products.Add(product);

            // Act
            cart.Products.Remove(product);

            // Assert
            Assert.Empty(cart.Products);
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