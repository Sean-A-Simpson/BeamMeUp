using BeamMeUp.DataStructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BeamMeUp
{
    class Program
    {
        private static string product;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                product = string.Format("_{0}",args[0]);
            }

            Serializer ser = new Serializer();

            Console.WriteLine("---=== BEAM ME UP ===---");

            // Read template
            string path = string.Format("{0}\\template{1}.xml", Directory.GetCurrentDirectory(), product);
            if (File.Exists(path))
            {
                if(Transposer.Init(product))
                {
                    Console.WriteLine("Reading template xml file");
                    string xmlInputData = File.ReadAllText(path, Encoding.UTF8);
                    Package package = ser.Deserialize<Package>(xmlInputData);

                    DebugLog("Fetching iOS language map");
                    LanguageMap<string, string> iOSlanguageMap = LanguageMap<string, string>.GetLanguages(Platform.iOS);

                    Console.WriteLine("Reading and transposing iOS Excel sheet data");
                    Transposer.Transpose(package, iOSlanguageMap);

                    Console.WriteLine("Saving iOS transporter output xml file");
                    string xmlOutputData = ser.Serialize(package);
                    string outPath = Directory.GetCurrentDirectory() + @"\transporter.xml";
                    File.WriteAllText(outPath, xmlOutputData);

                    DebugLog("Fetching Google language map");
                    LanguageMap<string, string> googleLanguageMap = LanguageMap<string, string>.GetLanguages(Platform.Google);

                    Console.WriteLine("Reading and transposing Google Excel sheet data");
                    Transposer.Transpose(package, googleLanguageMap);

                    Console.WriteLine("Saving Google description/release xml files");
                    StringBuilder descriptionOut = new StringBuilder();
                    StringBuilder releaseOut = new StringBuilder();

                    foreach (KeyValuePair<string, string> language in googleLanguageMap)
                    {
                        string langCode = language.Value;
                        Locale locale = package.software.software_metadata.versions.versions1[0].locales.locales1.Find(c => c.name == langCode);

                        if (locale != null)
                        {
                            descriptionOut.AppendFormat("<{0}>\n{1}\n</{2}>\n", langCode, locale.description, langCode);
                            releaseOut.AppendFormat("<{0}>\n{1}\n</{2}>\n", langCode, locale.version_whats_new, langCode);
                        }
                        else
                        {
                            Console.WriteLine("***ERROR*** Language {0} not found! Something has gone wrong with the transposition", langCode);
                        }
                    }

                    outPath = Directory.GetCurrentDirectory() + @"\description.xml";
                    File.WriteAllText(outPath, descriptionOut.ToString());

                    outPath = Directory.GetCurrentDirectory() + @"\release.xml";
                    File.WriteAllText(outPath, releaseOut.ToString());

                    Transposer.Kill();

                    Console.WriteLine("Transporter coordinates locked in, Captain!");
                    
                }
                else
                {
                    Console.WriteLine("Failed to open input spreadsheet, aborting...");
                }
            }
            else
            {
                Console.WriteLine("Captain! File {0} not found! Is it missing?", path);
            }

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }

        public static void DebugLog(string format, params object[] arg)
        {
            WriteDebug(format, arg);
        }

        [Conditional("DEBUG")]
        private static void WriteDebug(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
