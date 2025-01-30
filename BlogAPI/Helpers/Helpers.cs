namespace BlogAPI.Helpers
{
    public static class Helpers
    {

        public static string GenerateSlug(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            return text.Replace(" and ", " & ").Replace(" ", "-").ToLower();
            
        }
    }
}
