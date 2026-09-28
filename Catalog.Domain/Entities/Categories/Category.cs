using Catalog.Domain.Entities.Books;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.Categories
{
    public sealed class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Category()
        {

        }

        public Category(string name, string? description = null)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);

            Id = Guid.CreateVersion7();
            Name = name.Trim();
            Description = description?.Trim();
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException("El nombre de la categoría es requerido.");
            }

            if (name.Trim().Length < 2)
            {
                throw new BusinessRuleException("El nombre de la categoría debe tener al menos 2 caracteres.");
            }

            if (name.Trim().Length > 64)
            {
                throw new BusinessRuleException("El nombre de la categoría debe tener máximo 64 caracteres.");
            }
        }

        private static void ApplyDescriptionRules(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > 512)
            {
                throw new BusinessRuleException("La descripción de la categoría debe tener máximo 512 caracteres.");
            }
        }
    }
}
