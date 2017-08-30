using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace Tesco.Framework.Common.Logging.Entities
{
    class Fault
    {
        #region private variables
        private string filename;
        private string sourceClass;
        private string sourceMethod;
        private StackTrace trace;
        private string innerException;
        private string additionalInfo;
        #endregion

        #region properties
        public string FileName
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }

        public string SourceClass
        {
            get {
                return sourceClass;
            }
            set {
                sourceClass = value;
            }
        }

        public string SourceMethod
        {
            get
            {
                return sourceMethod;
            }
            set
            {
                sourceMethod = value;
            }
        }

        public StackTrace Trace
        {
            get
            {
                return trace;
            }
            set
            {
                trace = value;
            }
        }

        public DateTime FaultId
        {
            get
            {
                return DateTime.Now;
            }
        }

        public string LineNumber
        {
            get 
            {
                return Trace.GetFrame(Trace.FrameCount - 1).GetFileLineNumber().ToString(CultureInfo.CurrentCulture);
            }
        }

        public string InnerException
        {
            get {
                return innerException;
            }
            set
            {
                innerException = value;
            }
        }

        public string AdditionalInfo
        {
            get
            {
                return additionalInfo;
            }
            set
            {
                additionalInfo = value;
            }
        }

        #endregion

        public Fault(Exception ex)
        {
            this.Trace = new StackTrace(ex, true);
            this.FileName = this.Trace.GetFrame(Trace.FrameCount - 1).GetFileName();
            this.SourceClass = this.Trace.GetFrame(Trace.FrameCount - 1).GetMethod().DeclaringType.ToString();
            this.SourceMethod = this.Trace.GetFrame(Trace.FrameCount - 1).GetMethod().ToString();
            if(ex.InnerException != null)
                this.InnerException = ex.InnerException.ToString();

            this.AdditionalInfo = ex.Message;
        }
    }
}
