using System.Collections.Generic;

namespace DemandDriven.Pocos
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Converter {
        public IList<Entry> Entries {get; protected set;} = new List<Entry>();
    }
}