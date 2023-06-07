using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
