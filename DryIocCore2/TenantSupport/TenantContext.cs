using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace DryIocCore2.TenantSupport
{
    /// <summary>
    /// Provides support for async-aware tenant context
    /// </summary>
    public static class TenantContext
    {
        /// <summary>
        /// Gets current tenant context
        /// </summary>
        public static string CurrentTenant => _contextStack.Value == null ? null : _contextStack.Value.IsEmpty ? null : _contextStack.Value.Peek();

        public class TenantScopeEventArgs : EventArgs
        {
            public string Tenant { get; protected set; }

            public TenantScopeEventArgs(string tenant)
            {
                Tenant = tenant;
            }
        }
             
        public delegate void TenantScopeEventHandler(TenantScopeEventArgs e);

        /// <summary>
        /// Subscribe to this event to react on a beginning of a tenant scope
        /// </summary>
        public static event TenantScopeEventHandler OnBeginScope;

        /// <summary>
        /// Subscribe to this event to react on an end of a tenant scope
        /// </summary>
        public static event TenantScopeEventHandler OnEndScope;

        /// <summary>
        /// Creates a new tenant scope and returns IDisposable to be disposed at the end of the scope
        /// </summary>
        /// <param name="tenant">A string Tenant id</param>
        /// <returns>Returns IDisposable to be disposed at the end of the scope</returns>
        public static IDisposable BeginScope(string tenant)
        {
            ContextStack = ContextStack.Push(tenant);
            OnBeginScope?.Invoke(new TenantScopeEventArgs(tenant));
            return new ContextDisposable();
        }

        private sealed class ContextDisposable : IDisposable
        {
            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    Pop();
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        private static readonly AsyncLocal<ImmutableStack<string>> _contextStack = new AsyncLocal<ImmutableStack<string>>();

        private static ImmutableStack<string> ContextStack
        {
            get => _contextStack.Value ?? (_contextStack.Value = ImmutableStack.Create<string>());
            set => _contextStack.Value = value;
        }

        private static void Pop()
        {
            OnEndScope?.Invoke(new TenantScopeEventArgs(CurrentTenant));
            ContextStack = ContextStack.Pop();
        }

        /// <summary>
        /// For debugging purposes
        /// </summary>
        internal static string CurrentStack => string.Join(",", ContextStack.Reverse());
    }
}
