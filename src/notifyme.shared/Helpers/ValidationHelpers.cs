using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace notifyme.shared.Helpers
{
    public static class ValidationHelpers
    {
        public static ReturnMessage ValidateModel<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            var valid = Validator.TryValidateObject(model, ctx, validationResults, true);
            if (valid) return new ReturnMessage(true, "");

            var errorMsg = string.Join(", ",validationResults.Select(x => x.ErrorMessage));
            return new ReturnMessage(false, errorMsg);
        }
    }
}