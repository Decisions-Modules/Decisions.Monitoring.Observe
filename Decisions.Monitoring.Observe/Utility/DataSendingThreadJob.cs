using System;
using System.Collections.Generic;
using DecisionsFramework;
using DecisionsFramework.Utilities;

namespace Decisions.Monitoring.Observe.Utility
{
    internal abstract class DataSendingThreadJob<T> : IThreadJob
    {
        private readonly List<T> dataList = new List<T>();
        private readonly TimeSpan delay;
        private readonly string queueName;
        private bool isStarted;

        public DataSendingThreadJob(string aQueueName, TimeSpan runDelay)
        {
            delay = runDelay;
            queueName = aQueueName;
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public void Run()
        {
            T[] data;
            lock (this)
            {
                isStarted = false;
                data = dataList.ToArray();
                dataList.Clear();
            }

            ;
            SendData(data);
        }

        public void AddItem(params T[] data)
        {
            lock (this)
            {
                dataList.AddRange(data);
                TryToStart();
            }
        }

        private void TryToStart()
        {
            if (isStarted) return;
            isStarted = true;

            var startTime = DateUtilities.Now().Add(delay);
            ThreadJobService.AddToQueue(startTime, this, queueName);
        }

        protected abstract void SendData(T[] data);
    }
}