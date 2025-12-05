using Domain.EventBus;

namespace Domain {
    public interface IInputDeviceManager {
        public DeviceType CurrentDevice { get; }
    }
    public record DeviceChangedGameEvent(DeviceType OldDevice, DeviceType NewDevice) : IGameEvent;
}