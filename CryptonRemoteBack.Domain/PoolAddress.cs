using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptonRemoteBack.Domain
{
    [Table("PoolAddresses")]
    public class PoolAddress
    {
        [Key] public Guid Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public Pool Pool { get; set; } = null!;
    }
}
