using Translations;

string translation = Translator.Translate("Hello world!", E_Languages.es);

Console.WriteLine(translation);
// Output: "¡Hola Mundo!"


string[] allTranslations = Translator.TranslateAll("Cute", E_Languages.es);

string output = allTranslations[0];
for (int i = 1; i < allTranslations.Length; i++)
    output += ", " + allTranslations[i];

Console.WriteLine(output);
// Output: "Lindo, Linda"