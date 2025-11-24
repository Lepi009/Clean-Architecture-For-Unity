using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Infrastructure;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Presentation {
    public class AddressablePrefabProvider : IPrefabProvider {
        //include all fields and properties here (private & public)
        #region Fields and Properties

        private readonly Dictionary<Presenter, LoadInfo> _activeLoads = new();

        #endregion


        //include all constructors here
        #region Constructors

        #endregion


        //include all public methods here
        #region Public Methods

        public void TryGetPrefabAsync(Presenter presenter, Type viewType, Action<bool, GameObject> OnFinished) {
            // Safety: prevent duplicate loads for same presenter
            if(_activeLoads.ContainsKey(presenter))
                CancelLoading(presenter);

            LoadInfo loadInfo = new(new CancellationTokenSource(), OnFinished);

            _activeLoads[presenter] = loadInfo;
            string address = viewType.ToString().Split('.').Last();

            var handle = Addressables.LoadAssetAsync<GameObject>(address);

            handle.Completed += op => {
                if(loadInfo.CancellationSource.Token.IsCanceled) {
                    Addressables.Release(op);
                    return;
                }

                if(op.Status == AsyncOperationStatus.Succeeded) {
                    _activeLoads.Remove(presenter);
                    loadInfo.Callback?.Invoke(true, op.Result);
                }
                else {
                    _activeLoads.Remove(presenter);
                    loadInfo.Callback?.Invoke(false, null);
                }
            };

        }

        public void CancelLoading(Presenter presenter) {
            if(_activeLoads.TryGetValue(presenter, out var info)) {

                // Mark token as cancelled
                info.CancellationSource.Cancel();

                // Remove it from dictionary immediately
                _activeLoads.Remove(presenter);
            }
        }

        #endregion

        private readonly record struct LoadInfo(CancellationTokenSource CancellationSource, Action<bool, GameObject> Callback);
    }
}