using System;

namespace Infrastructure {
    public interface IViewManager {
        
        public void RequestViewAsync<TPresenter>(TPresenter presenter, Action<bool> OnFinished = null) where TPresenter : Presenter;
        public bool IsViewInstantiated<TPresenter>(TPresenter presenter) where TPresenter : Presenter;
        public bool RemoveView(Presenter presenter);

        public void Register<TView, TPresenter, TViewArgs>(Func<TViewArgs> argsFactory) where TPresenter : Presenter where TView : View<TPresenter, TViewArgs>;
        public void Register<TView, TPresenter>() where TPresenter : Presenter where TView : View<TPresenter, NoArgs>;
    }
}