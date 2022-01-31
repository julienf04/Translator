using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Translations
{
    /// <summary>
    /// Provides methods to translate words from to language
    /// </summary>
    public static class Translator
    {
        private const string _CHROME_ARGUMENT = "--headless"; //Makes a window not open

        private const string _MAIN_LINK = "https://translate.google.com";
        private const string _INPUT_LANGUAGE_ARGS = "/?sl=";
        private const string _OUTPUT_LANGUAGE_ARGS = "&tl=";
        private const string _TEXT_ARGS = "&text=";
        private const string _TRANSLATION_MODE_ARGS = "&op=";

        private const string _PARENT_XPATH_TRANSLATIONS = "//span[@jsname='jqKxS']";
        private const string _SINGLE_XPATH_TRANSLATION = "//span[@jsname='W297wb'";

        private static IWebDriver _driver;
        private static INavigation _navigation;

        static Translator()
        {
            // Create driver
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(_CHROME_ARGUMENT);
            try
            {
                _driver = new ChromeDriver(options);
                _navigation = _driver.Navigate();
            }
            catch (Exception)
            {
                _Close();
                throw;
            }
        }

        /// <summary>
        /// Translate sentences from to language you want
        /// </summary>
        /// <param name="toTranslate">Sentence/s to translate</param>
        /// <param name="fromLanguage">Input language</param>
        /// <param name="toLanguage">Output language</param>
        /// <param name="allTranslations">True if you need all possible translations in the array, otherwise false</param>
        /// <returns>Return the string translated</returns>
        private static string[] _Translate(string toTranslate, E_Languages fromLanguage, E_Languages toLanguage, bool allTranslations)
        {
            // Fixes errors in language codes occurred for reserved words and characters in C#
            string fixedFromlanguage = fromLanguage == E_Languages.is_ ? "is"
              : fromLanguage == E_Languages.zh_CN ? "zh-CN"
              : fromLanguage.ToString();

            string fixedTolanguage = toLanguage == E_Languages.is_ ? "is"
                : toLanguage == E_Languages.zh_CN ? "zh-CN"
                : toLanguage.ToString();

            // Go to the page with arguments and get output result for all translation
            string[] translations = null;
            if (allTranslations)
            {
                IWebElement[] elements = null;
                Waiter.WaitForNoExceptionAndSleep(() =>
                    _navigation.GoToUrl(
                        _MAIN_LINK
                        + _INPUT_LANGUAGE_ARGS + fixedFromlanguage
                        + _OUTPUT_LANGUAGE_ARGS + fixedTolanguage
                        + _TEXT_ARGS + toTranslate
                        + _TRANSLATION_MODE_ARGS + E_TranslationMode.translate));
                Waiter.WaitForNoExceptionAndSleep(() =>
                {
                    elements = _driver.FindElements(By.XPath(_PARENT_XPATH_TRANSLATIONS)).ToArray();
                    if (elements.Length <= 0)
                        throw new Exception("Elements was not found");
                });

                if (elements.Length == 1)
                    elements[0] = elements[0].FindElement(By.XPath(_SINGLE_XPATH_TRANSLATION));

                translations = elements.Select(x => x.Text).ToArray();
            }
            // Go to the page with arguments and get output result to 1 translation
            else
            {
                IWebElement element = null;
                Waiter.WaitForNoExceptionAndSleep(() =>
                    _navigation.GoToUrl(
                        _MAIN_LINK
                        + _INPUT_LANGUAGE_ARGS + fixedFromlanguage
                        + _OUTPUT_LANGUAGE_ARGS + fixedTolanguage
                        + _TEXT_ARGS + toTranslate
                        + _TRANSLATION_MODE_ARGS + E_TranslationMode.translate));
                Waiter.WaitForNoExceptionAndSleep(() => element = _driver.FindElement(By.XPath(_PARENT_XPATH_TRANSLATIONS)));

                if (element.Text == "")
                    element = element.FindElement(By.XPath(_SINGLE_XPATH_TRANSLATION));


                translations = new string[1] { element.Text };
            }


            // Return output
            return translations;
        }

        public static string Translate(string toTranslate, E_Languages fromLanguage, E_Languages toLanguage) => _Translate(toTranslate, fromLanguage, toLanguage, false)[0];

        /// <summary>
        /// Translate sentences from to language you want. Input language is detected automatically
        /// </summary>
        /// <param name="toTranslate">Sentence/s to translate</param>
        /// <param name="toLanguage">Output language</param>
        /// <returns>Return the string translated. If translation has more than one word, return the first</returns>

        public static string Translate(string toTranslate, E_Languages toLanguage) => _Translate(toTranslate, E_Languages.auto, toLanguage, false)[0];

        /// <summary>
        /// Translate sentences from to language you want. It always return at least 1 value
        /// </summary>
        /// <param name="toTranslate">Sentence/s to translate</param>
        /// <param name="fromLanguage">Input language</param>
        /// <param name="toLanguage">Output language</param>
        /// <returns>Return an array with all possible translations</returns>
        public static string[] TranslateAll(string toTranslate, E_Languages fromLanguage, E_Languages toLanguage) => _Translate(toTranslate, fromLanguage, toLanguage, true);

        /// <summary>
        /// Translate sentences from to language you want. Input language is detected automatically. It always return at least 1 value
        /// </summary>
        /// <param name="toTranslate">Sentence/s to translate</param>
        /// <param name="toLanguage">Output language</param>
        /// <returns>Return an array with all possible translations</returns>
        public static string[] TranslateAll(string toTranslate, E_Languages toLanguage) => _Translate(toTranslate, E_Languages.auto, toLanguage, true);

        /// <summary>
        /// Realese resources of WebDriver
        /// </summary>
        private static void _Close()
        {
            _driver?.Close();
            _driver?.Quit();
        }
    }
}
