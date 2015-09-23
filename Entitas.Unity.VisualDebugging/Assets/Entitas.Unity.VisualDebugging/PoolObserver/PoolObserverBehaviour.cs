using UnityEngine;

namespace Entitas.Unity.VisualDebugging
{
    [ExecuteInEditMode]
    public class PoolObserverBehaviour : MonoBehaviour {
        public PoolObserver poolObserver { get { return _poolObserver; } }

        PoolObserver _poolObserver;

        public void Init(PoolObserver poolObserver) {
            _poolObserver = poolObserver;
        }

        void Update() {
            if (_poolObserver != null && _poolObserver.entitiesContainer != null) {
                _poolObserver.entitiesContainer.name = _poolObserver.ToString();

                foreach (Transform entityObj in _poolObserver.entitiesContainer.transform)
                {
                    if (null == entityObj) return;

                    var eBeh = entityObj.GetComponent<EntityBehaviour>();

                    if (eBeh == null || eBeh.entity == null) {
                        try
                        {
                            // We might be destroyed due to Assembly Reload. 
                            // GameObject isn't null, but Unity overrides the == for the null operator to check for destruction.
                            if (Application.isEditor)
                                DestroyImmediate(entityObj.gameObject);
                            else
                                Destroy(entityObj.gameObject);
                        }
                        catch { }
                    }
                }
            }
        }
    }
}