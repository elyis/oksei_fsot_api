namespace oksei_fsot_api.src.Utility
{
    public class LoginGenerator
    {
        public string? Invoke(string name)
        {
            var result = TransliteratorRuToEn.Invoke(name);
            if (result == null)
                return null;

            DateTime.Now.ToString("t");
            string timestamp = DateTime.Now.ToString("HHmmss");
            var login = string.Concat(result, timestamp);
            return login;
        }
    }
}