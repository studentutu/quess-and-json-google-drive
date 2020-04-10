using UnityEngine;
namespace Scripts.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static bool ApplicationIsQuitting = false;

        private const string PARENT_NAME = "[Singleton]";
        private const string FORMATING_STRING = "{0} {1}";

        protected static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null && !ApplicationIsQuitting)
                    {
                        _instance = new GameObject(string.Format(FORMATING_STRING, PARENT_NAME, typeof(T).Name)).AddComponent<T>();
                        if (_instance.IsLoadFromPrefab)
                        {
                            var _prefab = Resources.Load(_instance.GetPrefabPath);
                            if (_prefab != null)
                            {
                                DestroyImmediate(_instance.gameObject);
                                var _go = Instantiate(_prefab, null) as GameObject;
                                _go.name = _prefab.name;
                                Resources.UnloadAsset(_prefab);
                                _instance = _go as T;
                                if (_instance == null)
                                {
                                    _instance = _go.GetComponentInChildren<T>();
                                }
                            }
                        }
                    }

                    if (_instance != null)
                    {
                        _instance.CheckInstance(_instance);
                    }
                }

                return _instance;
            }
        }

        public virtual string GetPrefabPath { get; }
        protected virtual bool IsLoadFromPrefab { get { return false; } }
        protected virtual bool IsDontDestroy { get { return false; } }
        public static bool IsExist { get { return _instance != null; } }

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                DestroyImmediate(this.gameObject);
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // [Unity.IL2CPP.CompilerServices.Il2CppSetOption(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
        private void CheckInstance(Singleton<T> _instanceCheck)
        {
            if (_instanceCheck != null && _instanceCheck != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this as T;
            _instance.InitInstance();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            if (_instance.IsDontDestroy) DontDestroyOnLoad(_instance.transform.root.gameObject);
        }

        protected virtual void InitInstance()
        {

        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }

    }

    public abstract class SingletonPersistent<T> : Singleton<T> where T : SingletonPersistent<T>
    {
        protected override bool IsDontDestroy { get { return true; } }
    }

    public abstract class SingletonSelfCreator<T> : Singleton<T> where T : SingletonSelfCreator<T>
    {
        protected abstract string PrefabPath { get; }
        public override string GetPrefabPath { get { return PrefabPath; } }
        protected override bool IsLoadFromPrefab { get { return true; } }
        protected override bool IsDontDestroy { get { return true; } }
    }
}