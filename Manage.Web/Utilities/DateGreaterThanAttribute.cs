using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Web.Utilities
{
    
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class DateGreaterThanAttribute : ValidationAttribute
        {
            string otherPropertyName;

            public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
                : base(errorMessage)
            {
                this.otherPropertyName = otherPropertyName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult validationResult = ValidationResult.Success;
               
                    // Using reflection we can get a reference to the other date property, in this example the from date
                    var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                    // Let's check that otherProperty is of type DateTime as we expect it to be
                    if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
                    {
                        DateTime toValidate = (DateTime)value;
                        DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                        // if the till date is lower than the from date, than the validationResult will be set to false and return
                        // a properly formatted error message
                        if (toValidate.CompareTo(referenceProperty) < 1 && toValidate.CompareTo(referenceProperty) != 0)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                    }
          
                return validationResult;
            }
        }
}
