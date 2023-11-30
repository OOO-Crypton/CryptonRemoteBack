using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Параметры разгона
    /// </summary>
    [Table("OverclockingParams")]
    public class OverclockingParams
    {
        [Key] public int Id { get; set; }
        public double СoreFrequency { get; set; }
        public double MemoryFrequency { get; set; }
        public double CoolerSpeed { get; set; }
        public double Consumption { get; set; }
        public int Counter { get; set; }
    }
}
