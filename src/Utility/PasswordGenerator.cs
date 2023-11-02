namespace oksei_fsot_api.src.Utility
{
    public class PasswordGenerator
    {
        private readonly string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!#&*()_+";

        public string Invoke(int length = 12)
        {
            var random = new Random();
            return new string(
                Enumerable.Repeat(allowedChars, length)
                    .Select(c => c[random.Next(c.Length)])
                    .ToArray());
        }
    }
}