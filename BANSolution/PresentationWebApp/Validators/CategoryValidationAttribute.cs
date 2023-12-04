using DataAccess.Repositories;
using Humanizer.Localisation;
using PresentationWebApp.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace PresentationWebApp.Validators
{
    public class CategoryValidationAttribute: ValidationAttribute
    {
          public CategoryValidationAttribute() { 
        }
        public string GetErrorMessage() =>
            $"Category input is not acceptable";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var product = (CreateProductViewModel)validationContext.ObjectInstance;

            if (value == null) { return new ValidationResult(GetErrorMessage()); }

            var categoryIdInput = (int)value;

            var _categoriesRepository =(CategoriesRepository) (validationContext.GetService(typeof(CategoriesRepository)));

            var minCategoryId = _categoriesRepository.GetCategories().Min(x => x.Id);
            var maxCategoryId = _categoriesRepository.GetCategories().Max(x => x.Id);

            if (categoryIdInput >= minCategoryId && categoryIdInput <= maxCategoryId)
            {
                return ValidationResult.Success;

            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}


 