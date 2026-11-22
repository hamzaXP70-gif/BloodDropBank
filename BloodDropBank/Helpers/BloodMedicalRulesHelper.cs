
using System;
using System.Linq;
using BloodDropBank.Enums;

namespace BloodDropBank.Helpers
{
    public static class BloodMedicalRulesHelper
    {
        // =============== Medical Rules ===============

        public const int MinAge = 18;
        public const int MaxAge = 65;

        public const decimal MinWeightKg = 50m;
        public const decimal MaxWeightKg = 250m;

        public const int MinDonationIntervalDays = 60;

        public const int UnitsPerDonation = 1;


        // =============== 1. Calculate Age ===============

        public static int CalculateAge(DateTime birthDate, DateTime? referenceDate = null)
        {
            DateTime today = (referenceDate ?? DateTime.UtcNow).Date;

            DateTime birth = birthDate.Date;

            if (birth > today)
                throw new ArgumentException("Birth date cannot be in the future.");

            int age = today.Year - birth.Year;

            if (birth > today.AddYears(-age))
                age--;

            return age;
        }


        // =============== 2. Validate Age ===============

        public static bool IsValidAge(DateTime? birthDate, DateTime? referenceDate = null)
        {
            if (!birthDate.HasValue)
                return false;

            DateTime today = (referenceDate ?? DateTime.UtcNow).Date;

            if (birthDate.Value.Date > today)
                return false;

            int age = CalculateAge(birthDate.Value, today);

            return age >= MinAge && age <= MaxAge;
        }


        // =============== 3. Validate Weight ===============

        public static bool IsValidWeight(decimal? weightKg)
        {
            return weightKg.HasValue && weightKg.Value >= MinWeightKg && weightKg.Value <= MaxWeightKg;
        }


        // =============== 4. Donation Interval ===============

        public static bool IsValidDonationInterval(DateTime? lastDonationDate, DateTime? referenceDate = null)
        {
            if (!lastDonationDate.HasValue)
                return true;

            DateTime today = (referenceDate ?? DateTime.UtcNow).Date;

            DateTime lastDate = lastDonationDate.Value.Date;

            int days = (today - lastDate).Days;

            return days >= MinDonationIntervalDays;
        }


        // =============== 5. Next Donation Date ===============

        public static DateTime? GetNextDonationDate(DateTime? lastDonationDate)
        {
            if (!lastDonationDate.HasValue)
                return null;

            return lastDonationDate.Value.Date.AddDays(MinDonationIntervalDays);
        }


        // =============== 6. Validate Units ===============

        public static bool IsValidDonationUnits(int units)
        {
            return units == UnitsPerDonation;
        }


        // =============== 7. Validate Blood Group ===============

        public static bool IsValidBloodGroup(BloodGroup bloodGroup)
        {
            return Enum.IsDefined(typeof(BloodGroup), bloodGroup);
        }


        // =============== 8. Compatible Recipients ===============

        public static BloodGroup[] GetCompatibleRecipientGroups(BloodGroup donorGroup)
        {
            return donorGroup switch
            {
                BloodGroup.O_Neg => new[]
                {
                    BloodGroup.O_Neg,
                    BloodGroup.O_Pos,
                    BloodGroup.A_Neg,
                    BloodGroup.A_Pos,
                    BloodGroup.B_Neg,
                    BloodGroup.B_Pos,
                    BloodGroup.AB_Neg,
                    BloodGroup.AB_Pos
                },

                BloodGroup.O_Pos => new[]
                {
                    BloodGroup.O_Pos,
                    BloodGroup.A_Pos,
                    BloodGroup.B_Pos,
                    BloodGroup.AB_Pos
                },

                BloodGroup.A_Neg => new[]
                {
                    BloodGroup.A_Neg,
                    BloodGroup.A_Pos,
                    BloodGroup.AB_Neg,
                    BloodGroup.AB_Pos
                },

                BloodGroup.A_Pos => new[]
                {
                    BloodGroup.A_Pos,
                    BloodGroup.AB_Pos
                },

                BloodGroup.B_Neg => new[]
                {
                    BloodGroup.B_Neg,
                    BloodGroup.B_Pos,
                    BloodGroup.AB_Neg,
                    BloodGroup.AB_Pos
                },

                BloodGroup.B_Pos => new[]
                {
                    BloodGroup.B_Pos,
                    BloodGroup.AB_Pos
                },

                BloodGroup.AB_Neg => new[]
                {
                    BloodGroup.AB_Neg,
                    BloodGroup.AB_Pos
                },

                BloodGroup.AB_Pos => new[]
                {
                    BloodGroup.AB_Pos
                },

                _ => Array.Empty<BloodGroup>()
            };
        }


        // =============== 9. Red Cell Compatibility ===============

        public static bool CanDonateRedCellsTo(BloodGroup donorGroup, BloodGroup recipientGroup)
        {
            if (!IsValidBloodGroup(donorGroup) || !IsValidBloodGroup(recipientGroup))
            {
                return false;
            }

            return GetCompatibleRecipientGroups(donorGroup).Contains(recipientGroup);
        }
    }
}
