using System;
using Skitter.Utilities.Extensions;
using static Skitter.Utilities.Extensions.CsvExtensions;

namespace Skitter.Models
{
    /// <summary> A person's full name with reference to their personal and clan names. </summary>
    public record ActorName
    {
        /// <summary> An actor's common, personal name. </summary>
        public GivenName FirstName { get; init; } = GivenName.Empty;

        /// <summary> An actor's family / clan name. </summary>
        public Surname LastName { get; init; } = Surname.Empty;


        /// <inheritdoc/>
        public override String ToString() => $"{FirstName.Romanised} {LastName.Romanised}";


        /// <summary> An empty, default name. </summary>
        public static ActorName Empty => new ActorName();


        /// <summary> Generate a random name. </summary>
        /// <param name="gender"> The gender of the name to generate. A none indicates that all names should be considered. </param>
        /// <returns> The generated name. </returns>
        public static ActorName Random(NameGender gender)
        {
            GivenName[] firstNames = CsvExtensions.LoadData<GivenName>("res://Data/Names/CommonFirstNames.csv");
            Surname[] lastNames = CsvExtensions.LoadData<Surname>("res://Data/Names/CommonLastNames.csv");

            return new ActorName
            {
                FirstName = firstNames.GetRandomElement() ?? GivenName.Empty,
                LastName = lastNames.GetRandomElement() ?? Surname.Empty
            };
        }
    }


    /// <summary> An actor's common, personal name. </summary>
    public record GivenName : IParseable<GivenName>
    {
        /// <summary> The common Alpha-2 designation of the name's country of origin. </summary>
        public String CountryISO { get; init; } = String.Empty;

        /// <summary> The name as it appears in its local language, potentially using non-Latin characters. </summary>
        public String Localised { get; init; } = String.Empty;

        /// <summary> The name as it appears in in English, should only use Latin characters. </summary>
        public String Romanised { get; init; } = String.Empty;

        /// <summary> The name's gender. </summary>
        public NameGender Gender { get; init; } = NameGender.NONE;


        /// <summary> An empty, default name. </summary>
        public static GivenName Empty => new GivenName();


        /// <inheritdoc/>
        public static GivenName Parse(String[] header, String[] data)
        {
            Int32 countryIndex = header.IndexOf("Country");
            Int32 genderIndex = header.IndexOf("Gender");
            Int32 localisedIndex = header.IndexOf("Localized Name");
            Int32 romanisedIndex = header.IndexOf("Romanized Name");

            NameGender gender = NameGender.NONE;
            if (genderIndex != -1)
            {
                switch (data[genderIndex])
                {
                    case "M":
                        gender = NameGender.MALE;
                        break;
                    case "F":
                        gender = NameGender.FEMALE;
                        break;
                }
            }

            return new GivenName
            {
                CountryISO = countryIndex != -1 ? data[countryIndex] : String.Empty,
                Gender = gender,
                Localised = countryIndex != -1 ? data[localisedIndex] : String.Empty,
                Romanised = countryIndex != -1 ? data[romanisedIndex] : String.Empty
            };
        }
    }


    /// <summary> An actor's family / clan name. </summary>
    public record Surname : IParseable<Surname>
    {
        /// <summary> The common Alpha-2 designation of the name's country of origin. </summary>
        public String CountryISO { get; init; } = String.Empty;

        /// <summary> The name as it appears in its local language, potentially using non-Latin characters. </summary>
        public String Localised { get; init; } = String.Empty;

        /// <summary> The name as it appears in in English, should only use Latin characters. </summary>
        public String Romanised { get; init; } = String.Empty;


        /// <summary> An empty, default name. </summary>
        public static Surname Empty => new Surname();


        /// <inheritdoc/>
        public static Surname Parse(String[] header, String[] data)
        {
            Int32 countryIndex = header.IndexOf("Country");
            Int32 localisedIndex = header.IndexOf("Localized Name");
            Int32 romanisedIndex = header.IndexOf("Romanized Name");

            return new Surname
            {
                CountryISO = countryIndex != -1 ? data[countryIndex] : String.Empty,
                Localised = countryIndex != -1 ? data[localisedIndex] : String.Empty,
                Romanised = countryIndex != -1 ? data[romanisedIndex] : String.Empty
            };
        }
    }


    /// <summary> The name's gender. </summary>
    public enum NameGender
    {
        NONE,
        MALE,
        FEMALE
    }
}
