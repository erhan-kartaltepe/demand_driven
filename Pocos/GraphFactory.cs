using System;

namespace DemandDriven.Pocos
{
    public enum InputType { CSV }

    /// <summary>
    /// A factory to generate graphs
    /// </summary>
    public class GraphFactory {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csv">The input type as CSV (can be refactoed to </param>
        /// <returns></returns>
        public static Converter Generate(InputType type, string data) {
            switch (type) {
                case InputType.CSV:
                    return new CsvConverter(data);
                    break;
                default:
                    throw new NotImplementedException("That type cannot be processed at this time");
            }
        }
    }
}