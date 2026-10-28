using System;

namespace DebugUI
{
    internal sealed class DisposableAction : IDisposable
    {
        private Action action;

        public DisposableAction(Action action)
        {
            this.action = action;
        }

        public void Dispose()
        {
            var callback = action;
            action = null;
            callback?.Invoke();
        }
    }
}