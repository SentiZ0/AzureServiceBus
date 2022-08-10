using BusServiceReceiver.Models;

namespace BusServiceReceiver
{
    public interface IProcessData
    {
        Task Process(UserDTO userId);
    }
}
