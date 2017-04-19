using System;
using System.Collections.Generic;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// Enum for Input types
    /// </summary>
    public enum InputType { CSV }

    /// <summary>
    /// A factory to generate graphs
    /// </summary>
    public class GraphFactory {
        
        /// <summary>
        /// Generates a Converter based on the input type and data
        /// </summary>
        /// <param name="type">The format the data is in</param>
        /// <param name="data">The data in its native format</param>
        /// <returns>A converter object</returns>
        public static Converter Generate(InputType type, string data) {
            switch (type) {
                case InputType.CSV:
                    return new CsvConverter(data);
                    break;
                default:
                    throw new NotImplementedException("That type cannot be processed at this time");
            }
        }

        /// <summary>
        /// Generates a Converter based on the input type and list of entries
        /// </summary>
        /// <param name="type">The format the data is requested</param>
        /// <param name="entries">The edges</param>
        /// <returns>A converter object</returns>
        public static Converter Generate(InputType type, IList<Entry> entries) {
            switch (type) {
                case InputType.CSV:
                    return new CsvConverter(entries);
                    break;
                default:
                    throw new NotImplementedException("That type cannot be processed at this time");
            }
        }
    }
}