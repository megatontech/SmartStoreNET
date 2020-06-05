using System;

namespace SmartStore.Services.Media
{
    public sealed class ProcessImageException : Exception
    {
        #region Public Constructors

        public ProcessImageException()
            : this((string)null, null)
        {
        }

        public ProcessImageException(ProcessImageQuery query)
            : this(query, null)
        {
        }

        public ProcessImageException(string message, ProcessImageQuery query)
            : base(message)
        {
            Query = query;
        }

        public ProcessImageException(ProcessImageQuery query, Exception innerException)
            : base(CreateMessage(query, innerException), innerException)
        {
            Query = query;
        }

        #endregion Public Constructors



        #region Public Properties

        public ProcessImageQuery Query { get; private set; }

        #endregion Public Properties



        #region Private Methods

        private static string CreateMessage(ProcessImageQuery query, Exception innerException)
        {
            var fileName = query?.FileName;

            var msg = fileName.HasValue()
                ? "Error while processing image '{0}'".FormatCurrent(fileName)
                : "Error while processing image";

            if (innerException != null)
            {
                msg += " (" + innerException.Message + ")";
            }

            return msg;
        }

        #endregion Private Methods
    }
}