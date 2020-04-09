﻿using System;
using System.Collections.Generic;

namespace Scripts.Utils.Async.Hidden
{
    public class ThreadToolsHelper : SingletonPersistent<ThreadToolsHelper>
    {
        private readonly object lockObject = new object();
        private readonly List<Action> actions = new List<Action>();

        public void Add(Action action)
        {
            lock (lockObject)
            {
                actions.Add(action);
            }
        }

        private void Update()
        {
            lock (lockObject)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] != null)
                    {
                        actions[i]();
                    }
                }
                actions.Clear();
            }
        }
    }
}
