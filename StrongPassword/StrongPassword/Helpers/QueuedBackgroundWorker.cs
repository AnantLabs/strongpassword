using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace StrongPassword
{
    public delegate void PasswordCallback(string text);

    public class QueuedBackgroundWorker
    {
        private readonly BackgroundWorker work;
        private readonly Queue<QueueItem> queue;
        private readonly PasswordCallback passwordEvent;

        public QueuedBackgroundWorker(PasswordCallback passwordCallback)
        {
            passwordEvent += passwordCallback;
            queue = new Queue<QueueItem>();

            work = new BackgroundWorker { WorkerReportsProgress = false, WorkerSupportsCancellation = false };

            work.DoWork += (sender, args) =>
                {
                    QueueItem item = args.Argument as QueueItem;
                    if (item != null)
                    {
                        args.Result = !String.IsNullOrEmpty(item.WeakPassword) ? 
                            PasswordCompressor.GeneratePassword(CryptWrapper.Generate.Encrypt(item.WeakPassword, item.SettingsPassword),(item.Size)) : 
                            string.Empty;
                    }
                };

            work.RunWorkerCompleted += (sender, args) =>
                {
                    passwordEvent.Invoke(args.Result is string ? (string)args.Result : string.Empty);  //Set the textbox value

                    queue.Dequeue();                                //Remove first from queue

                    if (queue.Count > 0)
                        work.RunWorkerAsync(queue.Peek());          //Run next in queue
                };

        }

        public void Add(string weakPassword,string settingsPassword,int size)
        {
            queue.Enqueue(new QueueItem(weakPassword, settingsPassword, size));

            if (queue.Count == 1)
                work.RunWorkerAsync(queue.Peek());              //Start the queue
        }

        //The item in the queue
        private class QueueItem
        {
            public QueueItem(string weakPassword, string settingsPassword,int size)
            {
                WeakPassword = weakPassword;
                SettingsPassword = settingsPassword;
                Size = size;
            }

            public int Size { get; private set; }
            public string WeakPassword { get; private set; }
            public string SettingsPassword { get; private set; }
        }
    }
}
