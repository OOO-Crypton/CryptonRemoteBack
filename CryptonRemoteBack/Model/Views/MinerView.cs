using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class MinerView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MinerInfo { get; set; } = string.Empty;

        public MinerView(Miner? input)
        {
            if (input == null) return;
            Id = input.Id;
            Name = input.Name;
            MinerInfo = input.MinerInfo;
        }
    }
}
