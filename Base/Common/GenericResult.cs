using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Base.Common
{
    public class GenericResult<T> : Result
    {
        public T First() => Results.Any() ? Results.First() : default;
        public T Last() => Results.Any() ? Results.Last() : default;
        public List<T> Results { get; set; }
        public GenericResult() : base()
        {
            Results = new List<T>();
        }

        public GenericResult(List<T> results)
        {
            Metadata.Code = HttpStatusCode.OK;
            Results = results;
        }
        #region Failed Results

        /// <summary>
        /// Gets failed result with default http status code = 400.
        /// </summary>
        public static GenericResult<T> BadRequest => Fail(string.Empty, HttpStatusCode.BadRequest);

        /// <summary>
        /// Gets failed result with default http status code = 404.
        /// </summary>
        public static GenericResult<T> NotFound => Fail(string.Empty, HttpStatusCode.NotFound);

        /// <summary>
        /// Gets failed result with default http status code = 204.
        /// </summary>
        public static GenericResult<T> Failed => Fail(string.Empty, HttpStatusCode.NoContent);

        /// <summary>
        /// Gets failed result with a message and default http status code = 400.
        /// </summary>
        /// <param name="message">The message.</param>
        public static GenericResult<T> Fail(string message)
        {
            return Fail(message, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Helper to return failed result with a message from exception and default http status code = 400.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static GenericResult<T> Fail(Exception ex)
        {
            return Fail(ex.Message, HttpStatusCode.BadRequest, ex);
        }

        /// <summary>
        /// Helper to return failed result with a http status code.
        /// </summary>
        /// <param name="code">The code.</param>
        public static GenericResult<T> Fail(HttpStatusCode code)
        {
            return Fail(string.Empty, code);
        }

        /// <summary>
        /// Helper to return failed result with explicit a message and http status code.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The http status code.</param>
        /// <param name="ex">The exception.</param>
        public static GenericResult<T> Fail(string message, HttpStatusCode httpStatusCode, Exception ex = null)
        {
            return new GenericResult<T>
            {
                Metadata = new ResponseInfo
                {
                    InnerEx = ex,
                    Success = false,
                    Message = message,
                    Code = httpStatusCode
                }
            };
        }

        /// <summary>
        /// Helper to return successful result with an item with explicit http status code.
        /// </summary>
        public static GenericResult<T> Succeed()
        {
            return new GenericResult<T>
            {
                Metadata = new ResponseInfo
                {
                    Code = HttpStatusCode.OK,
                    Success = true
                }
            };
        }

        #endregion
    }
}