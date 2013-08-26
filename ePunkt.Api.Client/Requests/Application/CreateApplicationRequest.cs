using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class CreateApplicationRequest : PutJsonHttpRequestMessage
    {
        public CreateApplicationRequest(ApplicationCreateParameter parameter)
            : base("Application", parameter)
        {
        }
    }
}
