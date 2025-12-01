using System;
using Application;
using Domain;
using Infrastructure;

namespace Presentation
{
    public class HelloWorldInputLayer : IInputLayer, IDisposable
    {
        public InputLayerType Type => InputLayerType.HUD;
        private readonly HelloWorldPresenter _presenter;
        private readonly InputLayerManager _inputLayerManager;

        public HelloWorldInputLayer(HelloWorldPresenter presenter, InputLayerManager inputLayerManager)
        {
            _presenter = presenter;
            _inputLayerManager = inputLayerManager;
            
            _presenter.Opened += PushLayer;
            _presenter.Closed += PopLayer;
        }
        
        public void Dispose() {
            _presenter.Opened -= PushLayer;
            _presenter.Closed -= PopLayer;
        }

        public InputConsumeType HandleNewInput(InputCommand command)
        {
            if(command is HelloWorldInputCommand)
            {
                _presenter.OnIncreaseCounterRequested();
                return InputConsumeType.Handled;
            }

            return InputConsumeType.NotHandled;
        }
        
        private void PushLayer() {
            _inputLayerManager.PushLayer(this);
        }

        private void PopLayer() {
            _inputLayerManager.PopLayer(this);
        }
    }

    public readonly record struct HelloWorldInputCommand() : InputCommand;
}