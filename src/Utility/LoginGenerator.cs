namespace oksei_fsot_api.src.Utility
{
    public class LoginGenerator
    {
        public string? Invoke(string name)
        {
            var result = TransliteratorRuToEn.Invoke(name);
            if (result == null)
                return null;

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var login = string.Concat(result, timestamp);
            return login;
        }
    }
}