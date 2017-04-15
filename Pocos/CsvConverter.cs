using System;
using System.Linq;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// 
    /// </summary>
    internal class CsvConverter : Converter {
        private const string TITLE_LINE = "Parent,Child,Quantity";

        public const string INVALID_CSV_ERROR = "Invalid CSV input";
        public const string FIRST_LINE_ERROR = "Invalid first line--expected " + TITLE_LINE;
        public const string TOKEN_COUNT_ERROR = "Expected three tokens on line {0}";
        public const string INVALID_QUANTITY_ERROR = "Expected a numerical quantity between 1 and 9 on line {0}";
        public const string ENTRY_VALIDATION_ERROR = "Errors encountered on line {0}:\n{1}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csv"></param>
        internal CsvConverter(string csv) {

            if (string.IsNullOrWhiteSpace(csv)) {
                throw new ArgumentException(INVALID_CSV_ERROR);
            }

            string [] lines = csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines[0].Trim() != TITLE_LINE) {
                throw new ArgumentException(FIRST_LINE_ERROR);
            }

            for (int i = 1; i < lines.Length; i++) {
                string[] tokens = lines[i].Split(',');
                if (tokens.Length != 3) {
                    throw new ArgumentException(string.Format(TOKEN_COUNT_ERROR, i));
                }

                byte quantity = 0;
                if (!byte.TryParse(tokens[2], out quantity)) {
                    throw new ArgumentException(string.Format(INVALID_QUANTITY_ERROR, i));
                }

                Entry entry = new Entry {ParentName = tokens[0], ChildName = tokens[1], Quantity = quantity};
                var result = entry.Validate();
                if (result.Count > 0) {
                    string errors = result.Select(x => x.ErrorMessage).Aggregate( (p, n) => p + "\n" + n);
                    throw new ArgumentException(string.Format(ENTRY_VALIDATION_ERROR, i, errors));
                }
                Entries.Add(entry);
            }
        }
    }
}