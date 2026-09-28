using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.Books.ValueObjects
{
    public sealed record Isbn
    {
        public const int LENGTH = 13;

        public string Value { get; private set; } = null!;

        private Isbn()
        {

        }

        public Isbn(string value)
        {
            string normalized = Normalize(value);

            ApplyValueRules(normalized);

            Value = normalized;
        }

        public override string ToString()
        {
            return Value;
        }

        // Permite escribir el ISBN con guiones o espacios. Ej: 978-0-307-47472-8
        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Replace("-", string.Empty)
                        .Replace(" ", string.Empty);
        }

        private static void ApplyValueRules(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessRuleException("El ISBN es requerido.");
            }

            if (value.Length != LENGTH)
            {
                throw new BusinessRuleException($"El ISBN debe tener {LENGTH} dígitos.");
            }

            if (!value.All(char.IsAsciiDigit))
            {
                throw new BusinessRuleException("El ISBN solo puede contener dígitos.");
            }

            if (!HasValidCheckDigit(value))
            {
                throw new BusinessRuleException("El dígito de control del ISBN no es válido.");
            }
        }

        // ISBN-13: los primeros 12 dígitos se multiplican alternando 1 y 3.
        // El dígito de control complementa la suma hasta el siguiente múltiplo de 10.
        private static bool HasValidCheckDigit(string value)
        {
            int sum = 0;

            for (int i = 0; i < LENGTH - 1; i++)
            {
                int digit = value[i] - '0';
                sum += i % 2 == 0 ? digit : digit * 3;
            }

            int checkDigit = (10 - (sum % 10)) % 10;

            return checkDigit == value[LENGTH - 1] - '0';
        }
    }
}
