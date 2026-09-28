using Catalog.Domain.Entities.Books;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.Authors
{
    public sealed class Author
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string? Nationality { get; private set; }
        public ICollection<Book> Books { get; private set; } = new List<Book>();

        public string FullName => $"{FirstName} {LastName}";

        private Author()
        {

        }

        public Author(string firstName, string lastName, string? nationality = null)
        {
            ApplyFirstNameRules(firstName);
            ApplyLastNameRules(lastName);
            ApplyNationalityRules(nationality);

            Id = Guid.CreateVersion7();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Nationality = nationality?.Trim();
        }

        private static void ApplyFirstNameRules(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new BusinessRuleException("El nombre del autor es requerido.");
            }

            if (firstName.Trim().Length < 2)
            {
                throw new BusinessRuleException("El nombre del autor debe tener al menos 2 caracteres.");
            }

            if (firstName.Trim().Length > 64)
            {
                throw new BusinessRuleException("El nombre del autor debe tener máximo 64 caracteres.");
            }
        }

        private static void ApplyLastNameRules(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new BusinessRuleException("El apellido del autor es requerido.");
            }

            if (lastName.Trim().Length < 2)
            {
                throw new BusinessRuleException("El apellido del autor debe tener al menos 2 caracteres.");
            }

            if (lastName.Trim().Length > 64)
            {
                throw new BusinessRuleException("El apellido del autor debe tener máximo 64 caracteres.");
            }
        }

        private static void ApplyNationalityRules(string? nationality)
        {
            if (!string.IsNullOrWhiteSpace(nationality) && nationality.Trim().Length > 64)
            {
                throw new BusinessRuleException("La nacionalidad del autor debe tener máximo 64 caracteres.");
            }
        }
    }
}
