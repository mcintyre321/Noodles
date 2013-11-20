using System.ComponentModel.DataAnnotations;

namespace Noodles.Example.Domain
{
    public class MyStringLengthAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public MyStringLengthAttribute(int min, int max)
        {
            _min = min;
            _max = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value as string != null)
            {
                if (value.ToString().Length < _min || value.ToString().Length > _max)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return "" + name + " needs to be between " + _min + " and " + _max + " in length";
        }
    }
}