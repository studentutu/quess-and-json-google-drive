using Scripts.Services;
using System;
using System.Collections.Generic;

namespace Scripts
{
    public static class App
    {
        public static event Action ApplicationStartEvent;
        public static event Action<bool> ApplicationPauseEvent;
        public static event Action ApplicationQuittingEvent;

        public static bool IsApplicationStarted { get; private set; }
        public static bool IsApplicationPaused { get; private set; }
        public static bool IsApplicationQuitting { get; private set; }

        public static SceneManagementService SceneService { get; private set; }

        static App()
        {
            // Default
            Entry.InitializingServices += FillContainers;

            IsApplicationStarted = false;
            IsApplicationPaused = false;
            IsApplicationQuitting = false;
        }

        private static void FillContainers(IReadOnlyList<IServices> services)
        {
            for (int i = 0; i < services.Count; i++)
            {
                // Put in here services
                if (SceneService == null)
                {
                    SceneService = services[i] as SceneManagementService;
                }
            }
        }

        public static void Start()
        {
            IsApplicationStarted = true;
            ApplicationStartEvent?.Invoke();
        }

        public static void Quit()
        {
            IsApplicationQuitting = true;
            ApplicationQuittingEvent?.Invoke();
        }

        public static void Pause(bool value)
        {
            IsApplicationPaused = value;
            ApplicationPauseEvent?.Invoke(value);
        }
    }
}
