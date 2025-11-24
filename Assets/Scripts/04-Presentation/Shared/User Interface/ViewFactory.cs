using System;
using Infrastructure;
using UnityEngine;

namespace Presentation {
    public interface IViewFactory {
        View CreateView(GameObject prefab, Presenter presenter);
    }

    public class ViewFactory<TView, TPresenter, TArgs> : IViewFactory
        where TPresenter : Presenter
        where TView : View<TPresenter, TArgs> {
        private readonly Func<TArgs> _argsFactory;

        public ViewFactory(Func<TArgs> argsFactory) {
            _argsFactory = argsFactory;
        }

        public View CreateView(GameObject prefab, Presenter presenter) {
            // Instantiate prefab and get component
            var view = GameObject.Instantiate(prefab).GetComponent<TView>();

            // Assign presenter (strongly typed)
            view.Presenter = (TPresenter)presenter;

            // Lazy creation of args
            TArgs args = _argsFactory();

            // Call strongly typed Construct
            view.Construct(args);

            return view;
        }
    }
}