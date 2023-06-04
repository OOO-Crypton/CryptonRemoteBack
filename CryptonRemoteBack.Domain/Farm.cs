using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptonRemoteBack.Domain
{
    [Table("Farms")]
    public class Farm
    {
        [Key] public Guid Id { get; set; }
        public string SystemInfo { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public List<FlightSheet> FlightSheets { get; set; } = new();
    }
}
