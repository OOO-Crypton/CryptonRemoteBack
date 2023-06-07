using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Кошелек для криптовалюты
    /// </summary>
    [Table("Wallets")]
    public class Wallet
    {
        [Key] public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; } = string.Empty; 

        /// <summary>
        /// Связанный юзер
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Монета
        /// </summary>
        public Currency Currency { get; set; } = null!;
    }
}
