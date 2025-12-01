using System;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class HelloWorldView : View<HelloWorldPresenter, NoArgs>, IDisposable
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text; 

        public override void Construct(NoArgs args)
        {
            Presenter.CounterUpdated += DisplayCounter;
            _button.onClick.AddListener(Button_Clicked);
        }

        public void Dispose()
        {
            Presenter.CounterUpdated -= DisplayCounter;
            _button.onClick.RemoveAllListeners();
        }

        private void DisplayCounter(int newValue)
        {
            _text.text = "Hello World";
            if(newValue > 0)
            {
                _text.text += $"\nYou pressed {newValue} times";
            }
        }

        private void Button_Clicked()
        {
            Presenter.OnIncreaseCounterRequested();
        }
    }
}