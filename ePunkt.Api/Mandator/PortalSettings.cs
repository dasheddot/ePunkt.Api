
namespace ePunkt.Api
{
    public class PortalSettings
    {
        public bool ForceSsl { get; set; }
        public bool EnableNewsletterSection { get; set; }
        public bool UseImagesForButtons { get; set; }
        public bool EnableSelfServiceZone { get; set; }
        public string StartOnCustomView { get; set; }
        public string FeedbackMailAddress { get; set; }

        public bool StartOnJobsList { get; set; }
        public bool DisplayLocationInJobListing { get; set; }
        public bool DisplayDateInJobListing { get; set; }
        public bool GoToJobDetailPageIfSingleResult { get; set; }
        public bool EnableRegionsFilterOnJobsList { get; set; }
        public bool EnableJobProfilesFilterOnJobsList { get; set; }
        public bool EnableFilterOnJobsList { get; set; }
        public bool EnableJobsList { get; set; }
        public bool MoveJobsFilterToBottomOnJobsList { get; set; }

        public bool DisplayJobsInfoBox { get; set; }
        public bool DisplayHeader { get; set; }
        public bool DisplayBackToOverviewLinkOnJobDetails { get; set; }
        public bool DisplayBackToOverviewLinkOnSuccess { get; set; }
        public bool DisplayProfileInfoBox { get; set; }
        public bool DisplayJobHeader { get; set; }
        public bool DisplayWelcomeHeader { get; set; }
        public bool DisplayProfileHeader { get; set; }

        public bool EnableExtendedXml { get; set; }
        public bool EnableJobTagsInXmlFeed { get; set; }
        public bool EnableFourthJobBlockInXmlFeed { get; set; }
        
        public bool DisplayCaptcha { get; set; }
        public bool AllowDuplicateEmail { get; set; }
        public bool AllowEmptyGender { get; set; }
        public bool EnableCvParser { get; set; }
        public int MaxNumberOfDocuments { get; set; }
        public string DefaultCulture { get; set; }
        public string DefaultCountry { get; set; }
        public bool SendRegistrationEmail { get; set; }
        public bool SendApplicationEmail { get; set; }
        public bool AllowEmptyBirthDate { get; set; }
        public int PasswordPolicyMinLength { get; set; }
        public int PasswordPolicyMinNumeric { get; set; }
        public int PasswordPolicyMinUppercase { get; set; }
        public int PasswordPolicyMinSymbols { get; set; }

        public bool DisplayMatchingJobs { get; set; }
        public bool AllowSignUpWithoutJob { get; set; }
        public int UnsolicitedApplicationJobId { get; set; }
        public bool AllowDeletionOfApplicant { get; set; }
        public bool AllowProfilePdfDownload { get; set; }
        public bool AllowEuropeanCvDownload { get; set; }
        public bool DisplayProfileOverview { get; set; }
        public bool AllowActiveInactiveSwitch { get; set; }

        public ApplicationOnSignUpType DisplayApplicationOnSignUp { get; set; }
        public bool AskForApplicationOnSuccess { get; set; }
        public bool AskForEmailConfirmation { get; set; }
        public bool AskForCredentialsOnSignUp { get; set; }
        public bool AskForDocuments { get; set; }
        public bool AskForOvertime { get; set; }
        public bool AskForFreelancer { get; set; }
        public bool AskForReferrerOnSuccess { get; set; }
        public bool AskForLocation { get; set; }
        public bool AskForRegions { get; set; }
        public bool AskForNationality { get; set; }
        public bool AskForSelfDescription { get; set; }
        public bool AskForJobDescription { get; set; }
        public bool AskForEmploymentType { get; set; }
        public bool AskForSalary { get; set; }
        public bool AskForWillingnessToMove { get; set; }
        public bool AskForDomesticTravelling { get; set; }
        public bool AskForAbroadTravelling { get; set; }
        public bool AskForActivities { get; set; }
        public bool AskForNewsletter { get; set; }
        public bool AskForQuitReason { get; set; }
        public bool AskForReferences { get; set; }
        public bool AskForMatchingJobsEmail { get; set; }
        public bool AskForInternship { get; set; }
        public bool AskForLocationBlock { get; set; }
        public bool DisplayJobProfilesFirst { get; set; }
        public bool AskForBeginDate { get; set; }
        public bool AskForJobProfileKnowledges { get; set; }
        public bool AskForJobRequirements { get; set; }
        public bool AskForCertificates { get; set; }
        public bool AskForExperience { get; set; }
        public bool AskForEducations { get; set; }
        public bool AskForTitles { get; set; }

        public DisplayOnPageType DisplayLanguagesOn { get; set; }
        public DisplayOnPageType DisplayActivitiesOn { get; set; }
        public DisplayOnPageType DisplayPublicationsOn { get; set; }
        public DisplayOnPageType DisplayAdditionalFieldsOn { get; set; }
        public DisplayOnPageType DisplayTagsOn { get; set; }
        public DisplayOnPageType DisplayCareerLevelsOn { get; set; }
        public DisplayOnPageType DisplayJobProfilesOn { get; set; }

        public string XingConsumerKey { get; set; }
        public string XingConsumerSecret { get; set; }
        public string LinkedinConsumerKey { get; set; }
        public string LinkedinConsumerSecret { get; set; }

        public enum DisplayOnPageType
        {
            DisplayOnBaseData,
            DisplayOnAdditionalInformation,
            DisplayOnSignUp,
            DisplayOnJobProfiles,
            DisplayOnJobRequirements,
            DoNotDisplay
        }

        public enum ApplicationOnSignUpType
        {
            DoNotDisplay,
            DisplayRequired,
            DisplayNotRequired
        }
    }
}
