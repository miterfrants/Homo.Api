using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Homo.Core.Constants;

namespace Homo.Api
{
    public class Required : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string stringify = (string)value;
            if (System.String.IsNullOrEmpty(stringify))
            {
                ValidationLocalizer validationLocalizer = validationContext.GetService(typeof(ValidationLocalizer)) as ValidationLocalizer;
                return new ValidationResult(validationLocalizer.Get($"{validationContext.MemberName} is required"));
            }
            return ValidationResult.Success;
        }
    }
}