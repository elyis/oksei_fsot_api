using System.Text;

namespace oksei_fsot_api.src.Utility
{
    public class TransliteratorRuToEn
    {
        private static readonly Dictionary<char, string> TransliterationTable = new()
        {
            {'а' ,"a"},
            {'б' ,"b"},
            {'в' ,"v"},
            {'г' ,"g"},
            {'д' ,"d"},
            {'е' ,"e"},
            {'ж' ,"zh"},
            {'з' ,"z"},
            {'и' ,"i"},
            {'й' ,"j"},
            {'к' ,"k"},
            {'л' ,"l"},
            {'м' ,"m"},
            {'н' ,"n"},
            {'о' ,"o"},
            {'п' ,"p"},
            {'р' ,"r"},
            {'с' ,"s"},
            {'т' ,"t"},
            {'у' ,"u"},
            {'ф' ,"f"},
            {'х' ,"x"},
            {'ц' ,"c"},
            {'ч' ,"ch"},
            {'ш' ,"sh"},
            {'щ' ,"shh"},
            {'ы' ,"y"},
            {'э' ,"e"},
            {'ю' ,"yu"},
            {'я' ,"ya"},
        };

        public static string? Invoke(string message)
        {
            message = message.ToLower();
            var transliterationMessage = new StringBuilder();
            foreach (var ch in message)
            {
                if (!TransliterationTable.TryGetValue(ch, out var val))
                    return null;
                transliterationMessage.Append(val);
            }
            return transliterationMessage.ToString();
        }
    }
}