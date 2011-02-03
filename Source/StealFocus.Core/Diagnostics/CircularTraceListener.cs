// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircularTraceListener.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
//   http://msdn.microsoft.com/en-us/library/aa395205.aspx
// 
//   Defines the CircularTraceListener type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using IO;

    /// <summary>
    /// CircularTraceListener Class.
    /// </summary>
    /// <remarks>
    /// Usage as follows.
    /// <![CDATA[
    ///     <system.diagnostics>
    ///         <sharedListeners>
    ///             <add
    ///                name="CircularTraceListener"
    ///                 type="StealFocus.Core.Diagnostics.CircularTraceListener, StealFocus.Core"
    ///                 initializeData="C:\Windows\Temp\MySvcTraceFile.svclog"
    ///                 maxFileSizeKB="10000" />
    ///         </sharedListeners>
    ///         <sources>
    ///             <source
    ///                 name="System.ServiceModel"
    ///                 switchValue="Information, ActivityTracing"
    ///                 propagateActivity="true" >
    ///                 <listeners>
    ///                     <add name="CircularTraceListener"/>
    ///                 </listeners>
    ///             </source>
    ///             <source 
    ///                 name="System.ServiceModel.MessageLogging">
    ///                 <listeners>
    ///                     <add name="CircularTraceListener" />
    ///                 </listeners>
    ///             </source>
    ///         <sources>
    ///     <system.diagnostics>
    /// ]]>
    /// </remarks>
    public class CircularTraceListener : XmlWriterTraceListener
    {
        /// <summary>
        /// Holds the FileQuotaAttribute.
        /// </summary>
        private const string FileQuotaAttribute = "maxFileSizeKB";

        /// <summary>
        /// Holds the DefaultMaxQuota.
        /// </summary>
        private const long DefaultMaxQuota = 1000;

        /// <summary>
        /// Holds the DefaultTraceFile.
        /// </summary>
        private const string DefaultTraceFile = "E2ETraces.svclog";

        /// <summary>
        /// Holds the circularStream.
        /// </summary>
        private static CircularStream circularStream;

        /// <summary>
        /// Holds the maxQuotaInitialized.
        /// </summary>
        private bool maxQuotaInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularTraceListener"/> class.
        /// </summary>
        /// <param name="file">The file path.</param>
        public CircularTraceListener(string file)
            : base(circularStream = new CircularStream(file))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularTraceListener"/> class.
        /// </summary>
        public CircularTraceListener()
            : base(circularStream = new CircularStream(DefaultTraceFile))
        {
        }

        /// <summary>
        /// Gets MaxQuotaSize.
        /// </summary>
        private long MaxQuotaSize
        {
            // Get the MaxQuotaSize from configuration file
            // Set to Default Value if there are any problems
            get
            {
                long maxFileQuota = 0;
                if (!this.maxQuotaInitialized)
                {
                    try
                    {
                        string maxQuotaOption = this.Attributes[FileQuotaAttribute];
                        if (maxQuotaOption == null)
                        {
                            maxFileQuota = DefaultMaxQuota;
                        }
                        else
                        {
                            maxFileQuota = int.Parse(maxQuotaOption, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (Exception)
                    {
                        maxFileQuota = DefaultMaxQuota;
                    }
                    finally
                    {
                        this.maxQuotaInitialized = true;
                    }
                }

                if (maxFileQuota <= 0)
                {
                    maxFileQuota = DefaultMaxQuota;
                }

                // MaxFileQuota is in KB in the configuration file, convert to bytes
                maxFileQuota = maxFileQuota * 1024;
                return maxFileQuota;
            }
        }

        #region XmlWriterTraceListener Functions

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            this.DetermineOverQuota();
            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="id">The ID value.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            this.DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id);
        }

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            this.DetermineOverQuota();
            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args data.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            this.DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="eventType">The event type.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="message">The message.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            this.DetermineOverQuota();
            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Trace the data.
        /// </summary>
        /// <param name="eventCache">The event cache.</param>
        /// <param name="source">The source.</param>
        /// <param name="id">The ID value.</param>
        /// <param name="message">The message.</param>
        /// <param name="relatedActivityId">The related Activity ID.</param>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            this.DetermineOverQuota();
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        /// <summary>
        /// Get the supported attributes.
        /// </summary>
        /// <returns>
        /// The attributes.
        /// </returns>
        protected override string[] GetSupportedAttributes()
        {
            return new[] { FileQuotaAttribute };
        }

        #endregion

        /// <summary>
        /// Determine if over quota.
        /// </summary>
        private void DetermineOverQuota()
        {
            // Set the MaxQuota on the circularStream if it hasn't been done
            if (!this.maxQuotaInitialized)
            {
                circularStream.MaxQuotaSize = this.MaxQuotaSize;
            }

            // If we're past the Quota, flush, then switch files
            if (circularStream.IsOverQuota)
            {
                this.Flush();
                circularStream.SwitchFiles();
            }
        }
    }
}