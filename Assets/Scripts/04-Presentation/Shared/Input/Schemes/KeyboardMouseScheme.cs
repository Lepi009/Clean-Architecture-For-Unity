using System.Collections.Generic;
using Domain;
using UnityEngine.InputSystem;

namespace Presentation
{
    public class KeyboardMouseScheme : IInputScheme
    {
        public void Activate()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public bool IsUsedThisFrame()
        {
            return Keyboard.current.anyKey.wasPressedThisFrame;
        }

        public Queue<InputCommand> RetrieveCommands(float deltaTime)
        {
            Queue<InputCommand> result = new();
            
            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                result.Enqueue(new HelloWorldInputCommand());
            }

            return result;
        }
    }
}