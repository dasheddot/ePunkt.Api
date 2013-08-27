using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class CreateApplicationRequest : PutJsonHttpRequestMessage<ApplicationCreateResponse>
    {
        public CreateApplicationRequest(ApplicationCreateParameter parameter)
            : base("Application", parameter)
        {
        }
    }
}
