using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Api.Parameters;
using ePunkt.SocialConnector;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ePunkt.Portal
{
    public class LinkThirdPartyProfileService
    {
        public async Task<Applicant> LinkProfileToApplicant([NotNull]ApiHttpClient apiClient, [NotNull] Mandator mandator, [NotNull]IProfile profile, ThirdParty thirdParty)
        {
            return await LinkProfileToApplicant(apiClient, mandator, null, profile, thirdParty);
        }

        public async Task<bool> IsEmailAddressAlreadyInUse([NotNull] ApiHttpClient apiClient, [NotNull] Mandator mandator, string email)
        {
            //check if the email is not in use already
            if (!mandator.Settings.AllowDuplicateEmail)
            {
                var applicantsWithThisEmail = await apiClient.SendAndReadAsync<IEnumerable<Applicant>>(new ApplicantsRequest(email));
                return applicantsWithThisEmail.Any();
            }
            return false;
        }

        public async Task<Applicant> LinkProfileToApplicant([NotNull]ApiHttpClient apiClient, [NotNull] Mandator mandator, [CanBeNull] Applicant loggedInApplicant, [NotNull] IProfile profile, ThirdParty thirdParty)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            var applicant = loggedInApplicant ?? await apiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(profile.Id, thirdParty));

            //we don't have an applicant that is logged in or has a matching profile
            //so, lets create a new applicant
            if (applicant == null || applicant.Id <= 0)
            {
                //this should never happen, because we catch this case earlier, before calling this method
                //this is just to make absolutely sure ;)
                if (await IsEmailAddressAlreadyInUse(apiClient, mandator, profile.Email))
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
                applicant = await apiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(parameter));
            }

            //now link the profile
            var linkRequest = thirdParty == ThirdParty.Xing
                                  ? new LinkXingRequest(applicant.Id, profile.Id, profile.Url)
                                  : (HttpRequestMessage) new LinkLinkedInRequest(applicant.Id, profile.Id, profile.Url);
            applicant = await apiClient.SendAndReadAsync<Applicant>(linkRequest);

            return applicant;
        }

    }
}