using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// Allows only names of 3 uppercase letters for the first and second column,
    /// and a positive single digit number for the quantity
    /// </summary>
    public partial class Entry
    {
        private const string FORMAT = "{0},{1},{2}";
        public const string QUANTITY_RANGE_ERROR = "The field Quantity must be between 1 and 9.";
        public const string QUANTITY_REQUIRED_ERROR = "The field Quantity must be between 1 and 9.";
        public const string PARENT_REQUIRED_ERROR = "The ParentName field is required.";
        public const string PARENT_REGEX_ERROR = "The field ParentName must match the regular expression '^[A-Z]{3}$'.";
        public const string CHILD_REQUIRED_ERROR = "The ChildName field is required.";
        public const string CHILD_REGEX_ERROR = "The field ChildName must match the regular expression '^[A-Z]{3}$'.";

        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string ParentName { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string ChildName { get; set; }
        [Required]
        [Range(1, 9)]
        public byte Quantity { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Entry()
        {
        }

        /// <summary>
        /// Named constructor
        /// </summary>
        /// <param name="parentName">The parent name</param>
        /// <param name="childName">The child name</param>
        /// <param name="quantity">The quantity</param>
        public Entry(string parentName, string childName, byte quantity)
        {
            ParentName = parentName;
            ChildName = childName;
            Quantity = quantity;
        }

        /// <summary>
        /// Validates the entry based on the data annotations
        /// </summary>
        /// <returns>A list of validation errrors</returns>
        public IList<ValidationResult> Validate()
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, ctx, validationResults, true);
            return validationResults;
        }

        /// <summary>
        /// Friendly repredentation of the Entry
        /// </summary>
        /// <returns>A string representation of Entry</returns>
        public override string ToString() {
            return string.Format(FORMAT, ParentName, ChildName, Quantity);
        }
    }
}