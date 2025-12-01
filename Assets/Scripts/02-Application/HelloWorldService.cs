using Domain;
using Domain.EventBus;

namespace Application
{
    public class HelloWorldService
    {
        //this counter holds the truth
        public int Counter { get; private set; }

        public HelloWorldService()
        {
            Counter = 0;
        }

        public void AddToCounter(int amount)
        {
            int oldValue = Counter;
            Counter += amount;
            ServiceLocator.EventBus.Publish(new CounterChangedEvent(Counter, oldValue));
        }
    }

    public record CounterChangedEvent(int NewValue, int OldValue) : IGameEvent;
}