using System.Collections.Generic;

namespace Domain {
    public interface IInputScheme {

        public void Activate();
        public void Deactivate();
        public Queue<InputCommand> RetrieveCommands(float deltaTime);
        public bool IsUsedThisFrame();
    }
}