using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Entitas.Unity.VisualDebugging {
    public class PoolObserver {

        public const string POOLS_ROOT_NAME = "Debug Pools";
        public const string POOLS_OBJECT_NAME = "Pool {0}";

        public Pool pool { get { return _pool; } }
        public string name { get { return _name; } }
        public GameObject entitiesContainer { get { return _entitiesContainer.gameObject; } }
        public Group[] groups { get { return _groups.ToArray(); }}

        readonly Pool _pool;
        readonly string _name;
        readonly List<Group> _groups;
        readonly Transform _entitiesContainer;

        public PoolObserver(Pool pool, string name = "Pool") {
            _pool = pool;
            _name = name;
            _groups = new List<Group>();

            // FindAll or create the Pool root
            UnityTag poolRootUnityTag = UnityTag.FindFirst(POOLS_ROOT_NAME);
            if (poolRootUnityTag == null) {
                var poolRootObj = new GameObject(POOLS_ROOT_NAME);
                poolRootUnityTag = poolRootObj.AddComponent<UnityTag>();
                poolRootUnityTag.value = POOLS_ROOT_NAME;
            }

            var poolObjName = string.Format(POOLS_OBJECT_NAME, name);
            UnityTag poolUnityTag = UnityTag.FindFirst(poolObjName);
            if (poolUnityTag == null) {
                var poolObj = new GameObject(poolObjName);
                poolUnityTag = poolObj.AddComponent<UnityTag>();
                poolUnityTag.value = poolObjName;
                poolObj.gameObject.transform.parent = poolRootUnityTag.gameObject.transform;
            }

            _entitiesContainer = poolUnityTag.gameObject.transform;

            var poolObserver = _entitiesContainer.gameObject.GetComponent<PoolObserverBehaviour>();
            if (poolObserver == null) {
                poolObserver = _entitiesContainer.gameObject.AddComponent<PoolObserverBehaviour>();
            }
            poolObserver.Init(this);

            _pool.OnEntityCreated += onEntityCreated;
            _pool.OnGroupCreated += onGroupCreated;
        }

        void onEntityCreated(Pool pool, Entity entity) {
            var entityBehaviour = new GameObject().AddComponent<EntityBehaviour>();
            entityBehaviour.Init(_pool, entity);
            entityBehaviour.transform.SetParent(_entitiesContainer, false);
        }

        void onGroupCreated(Pool pool, Group group) {
            _groups.Add(group);
        }

        public override string ToString() {
            return _entitiesContainer.name = 
                _name + " (" +
                _pool.Count + " entities, " +
                _pool.reusableEntitiesCount + " reusable, " +
                _groups.Count + " groups)";
        }
    }
}