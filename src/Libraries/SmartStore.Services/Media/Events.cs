using ImageProcessor;
using System.Drawing;
using System.Web;

namespace SmartStore.Services.Media
{
    /// <summary>
    /// Published after processing finishes and the result is saved to a stream (ProcessImageResult.Result)
    /// </summary>
    public class ImageProcessedEvent
    {
        #region Public Constructors

        public ImageProcessedEvent(ProcessImageQuery query, ImageFactory processor, ProcessImageResult result)
        {
            Query = query;
            Processor = processor;
            Result = result;
        }

        #endregion Public Constructors



        #region Public Properties

        public ImageFactory Processor { get; private set; }

        public ProcessImageQuery Query { get; private set; }

        public ProcessImageResult Result { get; private set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Published before processing begins, but after the source has been loaded.
    /// </summary>
    public class ImageProcessingEvent
    {
        #region Public Constructors

        public ImageProcessingEvent(ProcessImageQuery query, ImageFactory processor)
        {
            Query = query;
            Processor = processor;
        }

        #endregion Public Constructors



        #region Public Properties

        public ImageFactory Processor { get; private set; }

        public ProcessImageQuery Query { get; private set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Published after image query has been created and initialized
    /// by the media middleware controller with data from HttpContent.Request.QueryString.
    /// This event implies that a thumbnail is about to be created.
    /// </summary>
    public class ImageQueryCreatedEvent
    {
        #region Public Constructors

        public ImageQueryCreatedEvent(ProcessImageQuery query, HttpContextBase httpContext, string mimeType, string extension)
        {
            Query = query;
            HttpContext = httpContext;
            MimeType = mimeType;
            Extension = extension;
        }

        #endregion Public Constructors



        #region Public Properties

        public string Extension { get; private set; }

        public HttpContextBase HttpContext { get; private set; }

        public string MimeType { get; private set; }

        public ProcessImageQuery Query { get; private set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Published for every uploaded image which does NOT exceed maximum
    /// allowed size. This gives subscribers the chance to still process the image,
    /// e.g. to achive better compression before saving image data to storage.
    /// This event does NOT get published when the uploaded image is about to be processed anyway.
    /// </summary>
    /// <remarks>
    /// A subscriber should NOT resize the image. But if you do - and you shouldn't :-) - , don't forget to set <see cref="ResultSize"/>.
    /// </remarks>
    public class ImageUploadValidatedEvent
    {
        #region Public Constructors

        public ImageUploadValidatedEvent(ProcessImageQuery query, Size size)
        {
            Query = query;
            Size = size;
        }

        #endregion Public Constructors



        #region Public Properties

        /// <summary>
        /// Contains the source (as byte[]), max size, format and default image quality instructions.
        /// </summary>
        public ProcessImageQuery Query { get; private set; }

        /// <summary>
        /// The processing result. If null, the original data
        /// from <c>Query.Source</c> will be put to storage.
        /// </summary>
        public byte[] ResultBuffer { get; set; }

        /// <summary>
        /// Size of the result image.
        /// </summary>
        public Size ResultSize { get; set; }

        /// <summary>
        /// The original size of the uploaded image. May be empty.
        /// </summary>
        public Size Size { get; private set; }

        #endregion Public Properties
    }
}