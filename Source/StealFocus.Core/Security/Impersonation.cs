// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Impersonation.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the Impersonation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Security
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Security.Principal;
    using System.Threading;

    /// <summary>
    /// Allows the use of impersonation wrapped around a block of code
    /// </summary>
    /// <example>
    /// using(new Impersonation(username, domain, password))
    /// {
    ///     //do some work while impersonating
    /// }
    /// </example>
    public class Impersonation : IDisposable
    {
        #region Fields

        /// <summary>
        /// Holds the impersonation context.
        /// </summary>
        private readonly WindowsImpersonationContext windowsImpersonationContext;

        /// <summary>
        /// Holds whether the object is disposed.
        /// </summary>
        private bool disposed;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonation"/> class.
        /// </summary>
        /// <param name="user">The username.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="CoreException" />
        public Impersonation(string user, string domain, string password)
        {
            WindowsIdentity windowsIdentity = MySafeLogonHandle.Logon(user, domain, password);
            if (windowsIdentity == null)
            {
                throw new CoreException("Logon failed with the given credentials.");
            }

            this.windowsImpersonationContext = windowsIdentity.Impersonate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonation"/> class.
        /// </summary>
        /// <param name="windowsId">The Windows ID.</param>
        /// <exception cref="ArgumentNullException" />
        public Impersonation(WindowsIdentity windowsId)
        {
            if (windowsId == null)
            {
                throw new ArgumentNullException("windowsId");
            }

            this.windowsImpersonationContext = windowsId.Impersonate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Impersonation"/> class.
        /// </summary>
        /// <remarks>
        /// This uses the principal on the current thread to set up impersonation
        /// </remarks>
        public Impersonation()
        {
            WindowsPrincipal winPrincipal = Thread.CurrentPrincipal as WindowsPrincipal;
            if (winPrincipal == null)
            {
                throw new CoreException("The Principal has not been set on the current thread");
            }

            WindowsIdentity winIdentity = winPrincipal.Identity as WindowsIdentity;
            if (winIdentity == null)
            {
                throw new CoreException("The Windows Identity was not available");
            }

            this.windowsImpersonationContext = winIdentity.Impersonate();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Impersonation"/> class. 
        /// </summary>
        ~Impersonation()
        {
            this.Dispose(false);
        }

        #endregion // Constructors

        #region IDisposable

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">Whether to dispose.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.windowsImpersonationContext != null)
                    {
                        this.windowsImpersonationContext.Undo();
                    }
                }

                this.disposed = true;
            }
        }

        #endregion

        #region Nested type: MySafeLogonHandle

        /// <summary>
        /// Wrapper for my token
        /// </summary>    
        private class MySafeLogonHandle : SafeHandle
        {
            #region Constants

            /// <summary>
            /// Holds the Logon Interactive value.
            /// </summary>
            private const int LOGON32LOGONINTERACTIVE = 2;

            /// <summary>
            /// Holds the Logon Network value.
            /// </summary>
            private const int LOGON32LOGONNETWORK = 3;

            /// <summary>
            /// Holds the Logon Batch value.
            /// </summary>
            private const int LOGON32LOGONBATCH = 4;

            /// <summary>
            /// Holds the Logon Service value.
            /// </summary>
            private const int LOGON32LOGONSERVICE = 5;

            /// <summary>
            /// Holds the Logon Unlock value.
            /// </summary>
            private const int LOGON32LOGONUNLOCK = 7;

            /// <summary>
            /// Holds the Logon Network Clear value.
            /// </summary>
            private const int LOGON32LOGONNETWORKCLEARTEXT = 8;

            /// <summary>
            /// Holds the Logon New Credentials value.
            /// </summary>
            private const int LOGON32LOGONNEWCREDENTIALS = 9;

            /// <summary>
            /// Holds the Logon Provider Default value.
            /// </summary>
            private const int LOGON32PROVIDERDEFAULT = 0;

            #endregion

            /// <summary>
            /// Prevents a default instance of the <see cref="MySafeLogonHandle"/> class from being created.
            /// </summary>
            /// <remarks>
            /// Create a SafeHandle, informing the base class
            /// that this SafeHandle instance "owns" the handle,
            /// and therefore SafeHandle should call
            /// our ReleaseHandle method when the SafeHandle
            /// is no longer in use.
            /// </remarks>
            private MySafeLogonHandle()
                : base(IntPtr.Zero, true)
            {
            }

            /// <summary>
            /// Overrides the isinvalid method
            /// </summary>
            public override bool IsInvalid
            {
                get
                {
                    return IntPtr.Zero.Equals(handle);
                }
            }

            /// <summary>
            /// Logs on the user
            /// </summary>
            /// <param name="user">The username.</param>
            /// <param name="domain">The domain.</param>
            /// <param name="password">The password.</param>
            /// <returns>The Windows Identity.</returns>
            [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static WindowsIdentity Logon(string user, string domain, string password)
            {
                using (MySafeLogonHandle handler = new MySafeLogonHandle())
                {
                    if (NativeMethods.LogonUser(user, domain, password, LOGON32LOGONNETWORK, LOGON32PROVIDERDEFAULT, out handler.handle))
                    {
                        return new WindowsIdentity(handler.handle);
                    }
                }

                return null;
            }

            /// <summary>
            /// Releases the handle
            /// </summary>
            /// <returns>If the handle is closed.</returns>
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                // Here, we must obey all rules for constrained execution regions.
                return NativeMethods.CloseHandle(handle);

                // If ReleaseHandle failed, it can be reported via the
                // "releaseHandleFailed" managed debugging assistant (MDA).  This
                // MDA is disabled by default, but can be enabled in a debugger
                // or during testing to diagnose handle corruption problems.
                // We do not throw an exception because most code could not recover
                // from the problem.
            }

            /// <summary>
            /// Contains native methods
            /// </summary>
            private static class NativeMethods
            {
                /// <summary>
                ///  Free the kernel's file object (close the file).
                /// </summary>
                /// <param name="handle">The handle.</param>
                /// <returns>If the handle is closed.</returns>
                [DllImport("kernel32", SetLastError = true)]
                [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool CloseHandle(IntPtr handle);

                /// <summary>
                /// Logon method
                /// </summary>
                /// <param name="lpszUsername">The username.</param>
                /// <param name="lpszDomain">The domain.</param>
                /// <param name="lpszPassword">The password.</param>
                /// <param name="dwLogonType">The logon type.</param>
                /// <param name="dwLogonProvider">The logon provider.</param>
                /// <param name="phToken">The token.</param>
                /// <returns>If the user is logged on.</returns>
                [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                [return: MarshalAs(UnmanagedType.Bool)]
                internal static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
            }
        }

        #endregion
    }
}