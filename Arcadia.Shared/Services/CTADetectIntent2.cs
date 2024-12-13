using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using System.Collections.Generic;

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent2 : ICTADetectIntent2
    {
        private readonly IStringSearcher2 _stringSearcher2;

        // Constructor injection of IStringSearcher2
        public CTADetectIntent2(IStringSearcher2 stringSearcher2)
        {
            _stringSearcher2 = stringSearcher2;
        }

        public CorporateTravelAssistantIntent2 GetCorporateTravelAssistantIntent2(string input)
        {
            // Define keyword lists for different intents

            // 1. Book Flight
            var bookFlightMustHave = new List<string> { "book", "flight" };
            var bookFlightSearchWords = new List<string> { "reserve", "schedule", "ticket", "departure", "arrival" };

            if (_stringSearcher2.Search(input, bookFlightMustHave, bookFlightSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.BookFlight;
            }

            // 2. Book Car Hire
            var bookCarHireMustHave = new List<string> { "book", "car", "hire" };
            var bookCarHireSearchWords = new List<string> { "rent", "vehicle", "automobile", "pickup", "dropoff" };

            if (_stringSearcher2.Search(input, bookCarHireMustHave, bookCarHireSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.BookCarHire;
            }

            // 3. Book Accommodation
            var bookAccommodationMustHave = new List<string> { "book", "hotel" };
            var bookAccommodationSearchWords = new List<string> { "reserve", "room", "stay", "lodging", "check-in", "check-out" };

            if (_stringSearcher2.Search(input, bookAccommodationMustHave, bookAccommodationSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.BookAccommodation;
            }

            // 4. Modify Booking
            var modifyBookingMustHave = new List<string> { "modify", "booking" };
            var modifyBookingSearchWords = new List<string> { "change", "update", "date", "time", "details", "information" };

            if (_stringSearcher2.Search(input, modifyBookingMustHave, modifyBookingSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.ModifyBooking;
            }

            // 5. Cancel Booking
            var cancelBookingMustHave = new List<string> { "cancel", "booking" };
            var cancelBookingSearchWords = new List<string> { "reservation", "flight", "car", "hotel" };

            if (_stringSearcher2.Search(input, cancelBookingMustHave, cancelBookingSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.CancelBooking;
            }

            // 6. Check Flight Status
            var checkFlightStatusMustHave = new List<string> { "flight", "status" };
            var checkFlightStatusSearchWords = new List<string> { "current", "time", "delay", "arrival", "departure" };

            if (_stringSearcher2.Search(input, checkFlightStatusMustHave, checkFlightStatusSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.CheckFlightStatus;
            }

            // 7. Check Booking Status
            var checkBookingStatusMustHave = new List<string> { "booking", "status" };
            var checkBookingStatusSearchWords = new List<string> { "details", "information", "current" };

            if (_stringSearcher2.Search(input, checkBookingStatusMustHave, checkBookingStatusSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.CheckBookingStatus;
            }

            // 8. Get Available Options
            var getAvailableOptionsMustHave = new List<string> { "available", "options" };
            var getAvailableOptionsSearchWords = new List<string> { "flights", "cars", "hotels", "services" };

            if (_stringSearcher2.Search(input, getAvailableOptionsMustHave, getAvailableOptionsSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.GetAvailableOptions;
            }

            // 9. Get Pricing Information
            var getPricingInfoMustHave = new List<string> { "pricing", "information" };
            var getPricingInfoSearchWords = new List<string> { "cost", "price", "fee", "flights", "cars", "hotels", "services" };

            if (_stringSearcher2.Search(input, getPricingInfoMustHave, getPricingInfoSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.GetPricingInformation;
            }

            // 10. Get Travel Policies
            var getTravelPoliciesMustHave = new List<string> { "travel", "policies" };
            var getTravelPoliciesSearchWords = new List<string> { "rules", "guidelines", "company", "policy", "requirements" };

            if (_stringSearcher2.Search(input, getTravelPoliciesMustHave, getTravelPoliciesSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.GetTravelPolicies;
            }

            // 11. Technical Support
            var technicalSupportMustHave = new List<string> { "technical", "support" };
            var technicalSupportSearchWords = new List<string> { "help", "issue", "problem", "error", "bug", "assistance" };

            if (_stringSearcher2.Search(input, technicalSupportMustHave, technicalSupportSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.TechnicalSupport;
            }

            // 12. Payment Issues
            var paymentIssuesMustHave = new List<string> { "payment", "issues" };
            var paymentIssuesSearchWords = new List<string> { "billing", "invoice", "charge", "refund", "credit", "debit", "transaction" };

            if (_stringSearcher2.Search(input, paymentIssuesMustHave, paymentIssuesSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.PaymentIssues;
            }

            // 13. Feedback
            var feedbackMustHave = new List<string> { "feedback" };
            var feedbackSearchWords = new List<string> { "suggestion", "comment", "review", "improvement", "experience", "service" };

            if (_stringSearcher2.Search(input, feedbackMustHave, feedbackSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.Feedback;
            }

            // 14. Authenticate User
            var authenticateUserMustHave = new List<string> { "authenticate", "user" };
            var authenticateUserSearchWords = new List<string> { "login", "verify", "identity", "credentials", "password" };

            if (_stringSearcher2.Search(input, authenticateUserMustHave, authenticateUserSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.AuthenticateUser;
            }

            // 15. Update User Profile
            var updateUserProfileMustHave = new List<string> { "update", "profile" };
            var updateUserProfileSearchWords = new List<string> { "change", "modify", "information", "details", "contact" };

            if (_stringSearcher2.Search(input, updateUserProfileMustHave, updateUserProfileSearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.UpdateUserProfile;
            }

            // 16. View Booking History
            var viewBookingHistoryMustHave = new List<string> { "view", "booking", "history" };
            var viewBookingHistorySearchWords = new List<string> { "reservations", "past", "flights", "cars", "hotels" };

            if (_stringSearcher2.Search(input, viewBookingHistoryMustHave, viewBookingHistorySearchWords, isContainsAny: true))
            {
                return CorporateTravelAssistantIntent2.ViewBookingHistory;
            }

            // If no intent matches, return Unknown
            return CorporateTravelAssistantIntent2.Unknown;
        }
    }
}
