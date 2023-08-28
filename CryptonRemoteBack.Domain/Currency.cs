using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Монета
    /// </summary>
    [Table("Currencies")]
    public class Currency
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Связанные кошельки
        /// </summary>
        public List<Wallet> Wallets { get; set; } = new();
    }
}
