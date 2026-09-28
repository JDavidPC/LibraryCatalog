using Catalog.Domain.Entities.Books.ValueObjects;
using Catalog.Domain.Exceptions;

namespace Catalog.Tests.Domain
{
    [TestClass]
    public sealed class IsbnTests
    {
        [TestMethod]
        public void Create_WithValidValue_SetsValue()
        {
            Isbn isbn = new("9780307474728");

            Assert.AreEqual("9780307474728", isbn.Value);
        }

        [TestMethod]
        public void Create_WithHyphensAndSpaces_NormalizesValue()
        {
            Isbn isbn = new("978-0-307-47472-8");

            Assert.AreEqual("9780307474728", isbn.Value);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow("12345")]
        [DataRow("97803074747281")]
        [DataRow("978030747472A")]
        [DataRow("9780307474729")]
        public void Create_WithInvalidValue_ThrowsBusinessRuleException(string value)
        {
            Assert.ThrowsExactly<BusinessRuleException>(() => { _ = new Isbn(value); });
        }
    }
}
