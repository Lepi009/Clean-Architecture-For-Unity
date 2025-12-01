using System;
using Application;
using Domain;
using Domain.EventBus;

namespace Infrastructure
{
    public class HelloWorldPresenter : Presenter, IInitializable
    {
        private readonly HelloWorldService _service;

        public event Action<int> CounterUpdated;
        public event Action Opened;
        public event Action Closed;

        public HelloWorldPresenter(IViewManager viewManager, HelloWorldService service) : base(viewManager)
        {
            _service = service;
            _subGroup.Add<CounterChangedEvent>(UpdateCounter);
        }

        public void Initialize()
        {
            _viewManager.RequestViewAsync(this, succ =>
            {
                if(succ)
                {
                    Opened?.Invoke();
                    CounterUpdated?.Invoke(_service.Counter);
                }
            });
        }

        public void OnIncreaseCounterRequested()
        {
            _service.AddToCounter(1);
        }

        private void UpdateCounter(CounterChangedEvent @event)
        {
            CounterUpdated?.Invoke(@event.NewValue);
        }
    }
}