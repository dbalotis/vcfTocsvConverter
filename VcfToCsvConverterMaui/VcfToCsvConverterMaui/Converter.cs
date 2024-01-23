using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VcfToCsvConverterMaui
{
    public static class Converter
    {
        public static string ConvertVcfFileToCsvContent(string path)
        {
            var cardReader = new VCardReader.VCardReader();
            bool hasMore = false;
            StringBuilder sb = new StringBuilder("FormattedName;GivenName;FamilyName;AdditionalNames;Birthdate;Phones;Addresses;Emails;Websites;Notes\n");

            using (TextReader reader = new StreamReader(path))
            {
                do
                {
                    VCardReader.VCard card = new VCardReader.VCard();
                    cardReader.ReadInto(card, reader);
                    hasMore = card.Phones.Count > 0 || card.EmailAddresses.Count > 0;

                    if (!hasMore) continue;

                    var csvLine = $"{card.FormattedName};{card.GivenName};{card.FamilyName};{card.AdditionalNames};" +
                        $"{card.BirthDate?.ToShortDateString()};" +
                        $"{string.Join(" - ", card.Phones.Select(x => $"{x.PhoneType} {x.FullNumber}").Distinct())};" +
                        $"{string.Join(" - ", card.DeliveryAddresses.Select(x => $"{x.Street} {x.Region} {x.City} {x.PostalCode} {x.Country}"))};" +
                        $"{string.Join(" - ", card.EmailAddresses.Select(x => x.Address))};" +
                        $"{string.Join(" - ", card.Websites.Select(x => x.Url))};" +
                        $"{string.Join(" - ", card.Notes.Select(x => x.Text))}";

                    csvLine = csvLine.Replace("\r", " ").Replace("\n", " ");

                    sb.AppendLine(csvLine);

                } while (hasMore);

                return sb.ToString();
            }
        }
    }
}
