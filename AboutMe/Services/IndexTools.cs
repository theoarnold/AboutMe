namespace AboutMe.Services
{
    public class IndexTools
    {
        public static string GetPrimaryLanguageCssClass(string primaryLanguage)
        {
            switch (primaryLanguage)
            {
                case "C#":
                    return "CS-text";
                case "Java":
                    return "Java-text";
                case "JavaScript":
                    return "JavaScript-text";
                case "Python":
                case "CSS":
                    return "Python-text";
                default:
                    return string.Empty;
            }

        }

        // This is for use on a specific repo in my github that shows as being primarily css when the focus of the project is the python code.
        public static string ModifyPrimaryLanguage(string primaryLanguage)
        {
            if (primaryLanguage is "CSS")
            {
                GetPrimaryLanguageCssClass("Python");
                return "Python";
            }

            return primaryLanguage;
        }

    }
}