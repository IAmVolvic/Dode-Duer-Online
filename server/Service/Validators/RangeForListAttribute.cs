
using System.ComponentModel.DataAnnotations;

public class RangeForListAttribute : ValidationAttribute
{
    private readonly int _min;
    private readonly int _max;

    public RangeForListAttribute(int min, int max)
    {
        _min = min;
        _max = max;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is List<int> list)
        {
            if (list.Any(x => x < _min || x > _max))
            {
                return new ValidationResult($"All values must be between {_min} and {_max}.");
            }
        }

        return ValidationResult.Success;
    }
}