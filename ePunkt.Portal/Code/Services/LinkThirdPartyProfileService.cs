using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.SocialConnector;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ePunkt.Portal
{
    public class LinkThirdPartyProfileService
    {
        public async Task<ApplicantResponse> LinkProfileToApplicant([NotNull]ApiHttpClient apiClient, [NotNull] MandatorResponse mandatorResponse, [NotNull]IProfile profile, ThirdParty thirdParty)
        {
            return await LinkProfileToApplicant(apiClient, mandatorResponse, null, profile, thirdParty);
        }

        public async Task<bool> IsEmailAddressAlreadyInUse([NotNull] ApiHttpClient apiClient, [NotNull] MandatorResponse mandatorResponse, string email)
        {
            //check if the email is not in use already
            if (!mandatorResponse.PortalSettings.AllowDuplicateEmail)
            {
                var applicantsWithThisEmail = await new ApplicantsRequest(email).LoadResult(apiClient);
                return applicantsWithThisEmail.Any();
            }
            return false;
        }

        public async Task<ApplicantResponse> LinkProfileToApplicant([NotNull]ApiHttpClient apiClient, [NotNull] MandatorResponse mandatorResponse, [CanBeNull] ApplicantResponse loggedInApplicantResponse, [NotNull] IProfile profile, ThirdParty thirdParty)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            ApplicantResponse applicant;
            try
            {
                applicant = loggedInApplicantResponse ?? await new ApplicantRequest(profile.Id, thirdParty).LoadResult(apiClient);
            }
            catch
            {
                applicant = null;
            }

            //we don't have an applicant that is logged in or has a matching profile
            //so, lets create a new applicant
            if (applicant == null)
            {
                //this should never happen, because we catch this case earlier, before calling this method
                //this is just to make absolutely sure ;)
                if (await IsEmailAddressAlreadyInUse(apiClient, mandatorResponse, profile.Email))
                    return null;

                bool? gender = null;
                if (profile.Gender == Gender.Female)
                    gender = false;
                else if (profile.Gender == Gender.Male)
                    gender = true;

                var parameter = new ApplicantCreateParameter
                    {
                        Email = profile.Email,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Gender = gender
                    };
                applicant = await new ApplicantRequest(parameter).LoadResult(apiClient);
            }

            //now link the profile
            var linkRequest = thirdParty == ThirdParty.Xing
                                  ? new LinkXingRequest(applicant.Id, profile.Id, profile.Url)
                                  : (HttpRequestMessage<ApplicantResponse>)new LinkLinkedInRequest(applicant.Id, profile.Id, profile.Url);
            applicant = await linkRequest.LoadResult(apiClient);

            return applicant;
        }

    }
}