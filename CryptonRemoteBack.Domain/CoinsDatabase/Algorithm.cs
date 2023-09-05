using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Domain.CoinsDatabase
{
    [Table("Algorithms")]
    public class Algorithm
    {
        [Key] public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        /// <summary>
        /// Монеты, использующие алгоритм
        /// </summary>
        public List<Coin> Coins { get; set; } = new List<Coin>();
    }
}
