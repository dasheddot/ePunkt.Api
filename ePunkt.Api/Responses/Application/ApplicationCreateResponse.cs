using System.Collections.Generic;

namespace ePunkt.Api.Responses
{
    public class ApplicationCreateResponse
    {
        public IEnumerable<Error> Errors { get; set; }

        public enum Error
        {
            ApplicationAlreadyExists,
            JobIsClosed,
            JobDoesNotExist,
            ApplicantDoesNotExist
        }
    }
}
