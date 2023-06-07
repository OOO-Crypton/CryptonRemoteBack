using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class PoolView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<PoolAddressView> PoolAddresses { get; set; } = new();

        public PoolView(Pool? input)
        {
            if (input == null) return;
            Id = input.Id;
            Name = input.Name;
            PoolAddresses = input.PoolAddresses.Select(x => new PoolAddressView(x)).ToList();
        }
    }
}
