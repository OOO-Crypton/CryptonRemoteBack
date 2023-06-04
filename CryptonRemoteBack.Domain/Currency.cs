using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Монета
    /// </summary>
    [Table("Currencies")]
    public class Currency
    {
        [Key] public Guid Id { get; set; }

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
