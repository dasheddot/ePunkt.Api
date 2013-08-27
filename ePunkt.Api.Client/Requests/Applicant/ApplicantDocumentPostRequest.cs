﻿using ePunkt.Api.Parameters;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicantDocumentPostRequest : PostJsonHttpRequestMessage<string>
    {
        public ApplicantDocumentPostRequest(int applicantId, ApplicantDocumentParameter file)
            : base("Applicant/Document/" + applicantId, file)
        {
        }
    }
}
