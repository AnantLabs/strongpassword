using System;

namespace StrongPassword
{
    public delegate void ForceCloseDelegate();

    public class ForceCloseHandler
    {
        public ForceCloseDelegate forceCloseEvent;
        public bool IsClosing { get; private set; }

        public event ForceCloseDelegate ForceCloseEvent
        {
            add 
            { 
                forceCloseEvent += value; 
            }
            remove 
            { 
                if (forceCloseEvent != null) 
                    forceCloseEvent -= value; 
            }
        }

        public void Run()
        {
            IsClosing = true;

            if (forceCloseEvent != null) 
                forceCloseEvent.Invoke();
        }
    }
}