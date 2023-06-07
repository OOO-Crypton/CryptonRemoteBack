using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class CurrencyView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public CurrencyView(Currency? input)
        {
            if (input == null) return;

            Id = input.Id;
            Name = input.Name;
        }
    }
}
