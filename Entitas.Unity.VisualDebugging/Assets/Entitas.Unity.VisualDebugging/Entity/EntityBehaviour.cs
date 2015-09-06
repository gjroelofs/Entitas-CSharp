using System;
using UnityEngine;
using Entitas;

namespace Entitas.Unity.VisualDebugging {

    [ExecuteInEditMode]
    public class EntityBehaviour : MonoBehaviour {
        public Pool pool { get { return _pool; } }
        public Entity entity { get { return _entity; } }
        public bool[] unfoldedComponents { get { return _unfoldedComponents; } }

        Pool _pool;
        Entity _entity;
        bool[] _unfoldedComponents;

        public void Init(Pool pool, Entity entity) {
            _pool = pool;
            _entity = entity;
            _unfoldedComponents = new bool[_pool.totalComponents];
            _entity.OnEntityReleased += onEntityReleased;

            UnfoldAllComponents();
        }

        public void UnfoldAllComponents() {
            for (int i = 0; i < _unfoldedComponents.Length; i++) {
                _unfoldedComponents[i] = true;
            }
        }

        public void FoldAllComponents() {
            for (int i = 0; i < _unfoldedComponents.Length; i++) {
                _unfoldedComponents[i] = false;
            }
        }

        public void DestroyBehaviour() {
            _entity.OnEntityReleased -= onEntityReleased;

            if (gameObject != null) {
                // We might be destroyed due to Assembly Reload. 
                // GameObject isn't null, but Unity overrides the == for the null operator to check for destruction.
                if (Application.isEditor)
                    DestroyImmediate(gameObject);
                else
                    Destroy(gameObject);
            }

        }

        public virtual void CreateEntity() {
            
        }

        public virtual void onEntityReleased(Entity e) {
            DestroyBehaviour();
        }

        void Update() {
            name = _entity == null ? "Deleted" : _entity.ToString();
        }
    }
}