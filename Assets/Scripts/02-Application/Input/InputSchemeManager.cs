using System.Collections.Generic;
using Domain;
using Domain.EventBus;

namespace Application {
    public class InputSchemeManager : IInitializable, IUpdatable {

        //include all fields and properties here (private & public)
        #region Fields and Properties

        public DeviceType CurrentDevice { get; private set; } = DeviceType.None;

        private readonly Dictionary<DeviceType, IInputScheme> _inputSchemes;
        private readonly InputRouter _router;


        #endregion


        //include all constructors here
        #region Constructors

        public InputSchemeManager(Dictionary<DeviceType, IInputScheme> inputSchemes, DeviceType defaultDevice, InputRouter router) {
            _inputSchemes = inputSchemes;
            _router = router;
            CurrentDevice = defaultDevice;
        }


        public void Initialize() {
            SetDevice(CurrentDevice, true);
        }

        #endregion

        //include all private methods here
        #region Public Methods

        public void Update(float deltaTime) {
            //check if need to switch device
            foreach(var kvp in _inputSchemes) {
                if(kvp.Value == _inputSchemes[CurrentDevice]) continue;

                if(kvp.Value.IsUsedThisFrame()) {
                    SetDevice(kvp.Key);
                    break;
                }
            }

            var commandQueue = _inputSchemes[CurrentDevice].RetrieveCommands(deltaTime);
            while(commandQueue.TryDequeue(out var command))
            {
                _router.OnInputCommand(command);
            }
        }

        #endregion

        #region Private Methods

        private void SetDevice(DeviceType newDevice, bool forceEvent = false) {
            if(!forceEvent && CurrentDevice == newDevice) return;

            var oldDevice = CurrentDevice;
            CurrentDevice = newDevice;
            if(_inputSchemes.TryGetValue(oldDevice, out var oldScheme)) {
                oldScheme.Deactivate();
            }

            if(_inputSchemes.TryGetValue(newDevice, out var newScheme)) {
                newScheme.Activate();
            }

            ServiceLocator.EventBus.Publish(new DeviceChangedGameEvent(newDevice, oldDevice));
        }

        #endregion

    }
    public record DeviceChangedGameEvent(DeviceType OldDevice, DeviceType NewDevice) : IGameEvent;
}