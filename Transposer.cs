using BeamMeUp.DataStructure;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BeamMeUp
{
    public class Transposer
    {
        public static Excel.Application xlApp;
        public static Excel.Workbook xlWorkbook;
        public static Excel._Worksheet xlWorksheet;
        public static Excel.Range xlRange;

        public enum RowMap
        {
            Language = 1,
            Title = 2,
            Subtitle = 3,
            Description = 5,
            WhatsNew = 6,
            Keywords = 7
        }

        private static Dictionary<RowMap, int> MaxChars = new Dictionary<RowMap, int>()
        {
            { RowMap.Description, 4000 },
            { RowMap.Keywords, 100 },
            { RowMap.Subtitle, 30 },
            { RowMap.Title, 30 },
            { RowMap.WhatsNew, 4000 }
        };

        public static bool Init(string product)
        {
            bool success = false;
            string inputPath = string.Format("{0}\\input{1}.xlsx", Directory.GetCurrentDirectory(), product);
            Console.WriteLine("Opening Excel input spreadsheet {0}", inputPath);
            if (File.Exists(inputPath))
            {
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(inputPath);

                PrintSheets();
                success = true;
            }
            else
            {
                Console.WriteLine("***ERROR*** Spreadsheet file {0} not found, is it missing?", inputPath);
            }

            return success;
        }

        [Conditional("DEBUG")]
        private static void PrintSheets()
        {
            foreach (Excel.Worksheet sheet in xlWorkbook.Sheets)
            {
                Console.WriteLine("- Sheet found: {0}", sheet.Name);
            }
        }
        
        

        public static void Transpose (Package package, LanguageMap<string, string> languageMap)
        {
            xlWorksheet = null;
            foreach(Excel.Worksheet sheet in xlWorkbook.Sheets)
            {
                if (sheet.Name.Contains(languageMap.platform))
                {
                    Program.DebugLog("[Transposer] Found sheet for platform {0}: {1}", languageMap.platform, sheet.Name);
                    xlWorksheet = sheet;
                    break;
                }
            }

            if (xlWorksheet == null)
            {
                Console.WriteLine("***ERROR*** Worksheet for platform {0} not found!", languageMap.platform);
                return;
            }

            xlRange     = xlWorksheet.UsedRange;

            int colCount = xlRange.Columns.Count;
            Program.DebugLog("- {0} columns detected", colCount);

            if (xlRange.Cells[1,1].Value2 != null)
            {
                Program.DebugLog("Value in first cell: '{0}'", xlRange.Cells[1, 1].Value2);
            }

            for (int column = 2; column <= colCount; column++)
            {
                //Exit loop if there is no value in the language row
                if (xlRange.Cells[RowMap.Language, column].Value2 == null)
                {
                    Program.DebugLog("- no language found in column {0}, skipping", column);
                    continue;
                }

                string lang = xlRange.Cells[RowMap.Language, column].Value2.ToString().Trim();

                if (languageMap.ContainsKey(lang))
                {
                    Program.DebugLog("- transposing values for {0}", lang);
                    string targetLang = languageMap[lang];
                    Locales locales = package.software.software_metadata.versions.versions1[0].locales;
                    Locale locale = locales.locales1.Find(c => c.name == targetLang);

                    if (locale == null && languageMap.platform != Platform.iOS)
                    {
                        Program.DebugLog("Language {0} not found, creating...", targetLang);
                        locale = locales.AddLocale(targetLang);
                    }

                    if (locale != null)
                    {
                        if (xlRange.Cells[RowMap.Title, column].Value2 != null)
                        {
                            locale.title = xlRange.Cells[RowMap.Title, column].Value2.ToString().Trim();
                            if (locale.title.Length > MaxChars[RowMap.Title])
                            {
                                Console.WriteLine("*** VALIDATION ERROR *** - Title too long for {0}, is {1} characters", lang, locale.title.Length);
                            }
                        }
                        
                        if (xlRange.Cells[RowMap.Subtitle, column].Value2 != null)
                        {
                            locale.subtitle = xlRange.Cells[RowMap.Subtitle, column].Value2.ToString().Trim();
                            if (locale.subtitle.Length > MaxChars[RowMap.Subtitle])
                            {
                                Console.WriteLine("*** VALIDATION ERROR *** - Subtitle too long for {0}, contains {1} characters", lang, locale.subtitle.Length);
                            }
                        }
                        
                        if (xlRange.Cells[RowMap.Description, column].Value2 != null)
                        {
                            locale.description = xlRange.Cells[RowMap.Description, column].Value2.ToString().Trim();
                            if (locale.description.Length > MaxChars[RowMap.Description])
                            {
                                Console.WriteLine("*** VALIDATION ERROR *** - Description too long for {0}, contains {1} characters", lang, locale.description.Length);
                            }
                        }
                        
                        if (xlRange.Cells[RowMap.WhatsNew, column].Value2 != null)
                        {
                            locale.version_whats_new = xlRange.Cells[RowMap.WhatsNew, column].Value2.ToString().Trim();
                            if (locale.version_whats_new.Length > MaxChars[RowMap.WhatsNew])
                            {
                                Console.WriteLine("*** VALIDATION ERROR *** - What's new too long for {0}, contains {1} characters", lang, locale.version_whats_new.Length);
                            }
                        }
                        
                        if (xlRange.Cells[RowMap.Keywords, column].Value2 != null)
                        {
                            string keywordsString = xlRange.Cells[RowMap.Keywords, column].Value2.ToString().Trim();
                            if (keywordsString.Length > MaxChars[RowMap.Keywords])
                            {
                                Console.WriteLine("*** VALIDATION ERROR *** - Keywords too long for {0}, contains {1} characters", lang, keywordsString.Length);
                            }

                            // Split keywords by regular comma, Arabic comma, and Chinese comma
                            List<string> keywords = keywordsString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (keywords.Count == 1)
                            {
                                keywords = keywordsString.Split(new string[] { "،" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            }
                            if (keywords.Count == 1)
                            {
                                keywords = keywordsString.Split(new string[] { "、" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            }

                            locale.keywords = keywords;
                        }
                    }
                    else
                    {
                        Console.WriteLine("-- ***ERROR*** ioS language code '{0}' not found! Is this present in the template...?", targetLang);
                    }
                }
                else
                {
                    Console.WriteLine("-- ***ERROR*** Language '{0}' not found!", lang);
                }
            }
        }

        public static void Kill()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
