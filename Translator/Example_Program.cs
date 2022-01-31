using Translations;

string translation = Translator.Translate("Hello world!", E_Languages.es);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(translation);