namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string low_text = input.ToLower();
            int cnt = 0;
            foreach (char el in low_text)
            {
                if (!char.IsWhiteSpace(el) && !char.IsPunctuation(el)) cnt++;
            }

            char[] char_array_of_text = new char[cnt];
            int ind = 0;
            foreach (char el in low_text)
            {
                if (!char.IsWhiteSpace(el) && !char.IsPunctuation(el)) char_array_of_text[ind++] = el;
            }

            for (int i = 0; i < cnt / 2; i++)
            {
                if (char_array_of_text[i] != char_array_of_text[cnt - 1 - i]) return false;
            }
            return true;
        }
    }
}
