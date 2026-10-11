using System;

namespace PulseLogicHMR.Services.ABDM
{
    public class ABDMService : IABDMService
    {
        public object VerifyABHA(string abhaNumber)
        {
            if (string.IsNullOrEmpty(abhaNumber) || abhaNumber.Length != 14)
            {
                throw new ArgumentException("Invalid ABHA Number. Must be 14 digits.");
            }

            // Mock Response for Proof of Concept (Phase 1)
            var mockPatientData = new
            {
                FirstName = "Ayushman",
                LastName = "Bharat",
                Gender = "Male",
                YearOfBirth = "1994",
                MonthOfBirth = "01",
                DayOfBirth = "01",
                Mobile = "9999999999",
                Address = "New Delhi, India",
                StateCode = "07",
                DistrictCode = "094"
            };

            return mockPatientData;
        }
    }
}
