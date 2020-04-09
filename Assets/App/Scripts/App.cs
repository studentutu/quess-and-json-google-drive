﻿using Scripts.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        private static SceneManagementService sceneService = null;
        private static IConverter jsonConverter = null;
        private static URLLoader webLoader = null;

        public static SceneManagementService SceneService
        {
            get
            {
                if (sceneService == null)
                {
                    Entry.Instance.Init();
                }
                return sceneService;
            }
        }
        public static IConverter JsonConverter
        {
            get
            {
                if (jsonConverter == null)
                {
                    Entry.Instance.Init();
                }
                return jsonConverter;
            }
        }
        public static URLLoader WebLoader
        {
            get
            {
                if (webLoader == null)
                {
                    Entry.Instance.Init();
                }
                return webLoader;
            }
        }

        private static void FillContainers(IReadOnlyList<IServices> services)
        {
            for (int i = 0; i < services.Count; i++)
            {
                // Put in here services
                // Can be more optimized than that, but I don't have time
                if (sceneService == null)
                {
                    sceneService = services[i] as SceneManagementService;
                }

                if (jsonConverter == null)
                {
                    jsonConverter = services[i] as IConverter;
                }

                if (webLoader == null)
                {
                    webLoader = services[i] as URLLoader;
                }
            }
        }

        public static void Start(IReadOnlyList<IServices> services)
        {
            if (!IsApplicationStarted)
            {
                FillContainers(services);
                IsApplicationStarted = true;
                ApplicationStartEvent?.Invoke();
            }
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
