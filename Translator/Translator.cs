using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

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
        public static string Translate(string toTranslate, E_Languages fromLanguage, E_Languages toLanguage)
        {
            // Fixes errors in language codes occurred for reserved words and characters in C#
            string fixedFromlanguage = fromLanguage == E_Languages.is_ ? "is"
              : toLanguage == E_Languages.zh_CN ? "zh-CN"
              : toLanguage.ToString();

            string fixedTolanguage = toLanguage == E_Languages.is_ ? "is"
                : toLanguage == E_Languages.zh_CN ? "zh-CN"
                : toLanguage.ToString();

            // Go to the page with arguments and get output result
            IWebElement element = null;
            Waiter.WaitForNoExceptionAndSleep(() =>
            {
                _navigation.GoToUrl(
                    _MAIN_LINK
                    + _INPUT_LANGUAGE_ARGS + fixedFromlanguage
                    + _OUTPUT_LANGUAGE_ARGS + fixedTolanguage
                    + _TEXT_ARGS + toTranslate
                    + _TRANSLATION_MODE_ARGS + E_TranslationMode.translate);

                element = _driver.FindElement(By.XPath("/html/body/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[3]/c-wiz[2]/div[6]/div/div[1]/span[1]/span/span"));
            });

            // Return output
            return element.Text;
        }

        /// <summary>
        /// Translate sentences from to language you want
        /// </summary>
        /// <param name="toTranslate">Sentence/s to translate</param>
        /// <param name="toLanguage">Output language</param>
        public static string Translate(string toTranslate, E_Languages toLanguage) => Translate(toTranslate, E_Languages.auto, toLanguage);



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
