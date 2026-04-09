using ArmyCommander.Modules.Player;

namespace ArmyCommander.Core
{
    public interface ICollectible
    {
        void Collect(PlayerCollector collector);
    }
}