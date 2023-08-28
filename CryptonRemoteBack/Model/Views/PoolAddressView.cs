using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class PoolAddressView
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;

        public PoolAddressView(PoolAddress? input)
        {
            if (input == null) return;
            Id = input.Id;
            Address = input.Address;
        }
    }
}
