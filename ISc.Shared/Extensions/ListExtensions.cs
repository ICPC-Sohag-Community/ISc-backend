using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace ISc.Shared.Extensions
{
    public static class ListExtensions
    {
        public static Dictionary<string, List<string>> GetDictionary(this List<ValidationFailure> validationFailures)
        {
            Dictionary<string, List<string>> errors = [];
            validationFailures.ForEach(a =>
            {
                if (errors.TryGetValue(a.PropertyName, out var value))
                {
                    value.Add(a.ErrorMessage);
                }
                else
                {
                    errors.Add(a.PropertyName, [a.ErrorMessage]);
                }
            });

            return errors;
        }

        public static Dictionary<string, List<string>> GetDictionary(this List<IdentityError> validationFailures)
        {
            Dictionary<string, List<string>> errors = [];
            validationFailures.ForEach(a =>
            {
                if (errors.TryGetValue(a.Code, out var value))
                {
                    value.Add(a.Description);
                }
                else
                {
                    errors.Add(a.Code, [a.Description]);
                }
            });

            return errors;
        }
    }
}
