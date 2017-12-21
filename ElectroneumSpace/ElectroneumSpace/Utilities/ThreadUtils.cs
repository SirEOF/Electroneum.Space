using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ElectroneumSpace.Utilities
{
    public static class ThreadUtils
    {

        static bool _isSetup { get; set; }

        static int _mainThreadIdentifier { get; set; }

        public static void Setup()
        {
            // Get current thread and treat as main thread
            _mainThreadIdentifier = Thread.CurrentThread.ManagedThreadId;

            // Flag
            _isSetup = true;
        }

        public static bool IsMainThreadBound
        {
            get
            {
                if (!_isSetup)
                    throw new Exception("Thread utilities not setup");
                return Thread.CurrentThread.ManagedThreadId.Equals(_mainThreadIdentifier);
            }
        }

    }
}
