using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        private const string _file = @"C:\temp\problem2.txt";
        
        static void Main(string[] args)
        {
            //you can submit your own file path via command line argument
            var input = OpenFile(args.Length == 0 ? _file : args[0]);
            var formattedOutput = ParseFile(input);
            DisplayFormatted(formattedOutput);
        }

        private static string OpenFile(string path)
        {
            var fileContents = string.Empty;

            using (var sr = new StreamReader(path))
            { fileContents = sr.ReadToEnd(); }

            return fileContents;
        }

        private static void DisplayFormatted(List<string> formattedOutput)
        {
            foreach (var line in formattedOutput)
            { Console.WriteLine(line); }
        }

        private static List<string> ParseFile(string input)
        {
            var wc = new WordCount()
            {
                CurrentLine = string.Empty,
                CurrentWord = string.Empty,
                CurrentCharcterCount = 0,
                Output = new List<string>()
            };
            var words = input.Split(new char[] { ' ' });

            //iterate over words
            foreach (var word in words)
            {
                wc.CurrentWord = word;

                //check if word contains new line characters
                if (wc.CurrentWord.Contains('\n'))
                {
                    //check if dealing with true line break
                    var newLineCount = wc.CurrentWord.Count(c => c.Equals('\n'));
                    if (newLineCount > 1)
                    {
                        //split paragraph
                        var paragraphSplit = wc.CurrentWord.Split("\n\n");
                        for (var i = 0; i < paragraphSplit.Length; i++)
                        {
                            wc.CurrentWord = paragraphSplit[i];
                            wc = TryAddWordToLine(wc);

                            //if its the very last item we want it to become the start of our line
                            if (i != paragraphSplit.Length - 1)
                            {
                                //paragraph break
                                wc.Output.Add(wc.CurrentLine);
                                wc.Output.Add(Environment.NewLine);

                                //reset
                                wc.CurrentLine = string.Empty;
                                wc.CurrentCharcterCount = 0;
                            }
                        }
                    }
                    else
                    {
                        //not sure if this was an actual requirement or not 
                        //if you don't want random new lines making it look weird uncomment the next line
                        //wc.CurrentWord = wc.CurrentWord.Replace("\n", " ");
                        wc = TryAddWordToLine(wc);
                    }
                }
                else
                { wc= TryAddWordToLine(wc); }
            }

            return wc.Output;
        }

        private static WordCount TryAddWordToLine(WordCount input)
        {
            //check if proper length
            if (input.CurrentWord.Length + input.CurrentCharcterCount <= 80)
            {
                // add to the current line
                input.CurrentLine += input.CurrentWord + " ";
                input.CurrentCharcterCount += input.CurrentWord.Length;
            }
            else
            {
                //line break
                input.Output.Add(input.CurrentLine);

                //reset
                input.CurrentLine = input.CurrentWord + " ";
                input.CurrentCharcterCount = input.CurrentWord.Length;
            }

            return input;
        }
    }

}

/*

        private static List<string> ParseParagraph(string input)
        {
            var wordCounter = new WordCount()
            {
                CurrentLine = string.Empty,
                CurrentWord = string.Empty,
                CurrentCharcterCount = 0,
                Output = new List<string>()
            };
            var count = 0;
            var line = string.Empty;
            var output = new List<string>();
            var wordArray = input.Split(new char[] { ' ' });

            //iterate over words
            foreach (var word in wordArray)
            {


                //check if word contains new line characters
                if (word.Contains('\n'))
                {
                    //check if dealing with true line break
                    var newLineCount = word.Count(c => c.Equals('\n'));
                    if (newLineCount > 1)
                    {
                        //split paragraph
                        var paragraphSplit = word.Split("\n\n");
                        for (var i = 0; i < paragraphSplit.Length; i++)
                        {
                            var paragraphWord = paragraphSplit[i];

                            //check if word can fit on the line
                            if (paragraphWord.Length + count <= 80)
                            {
                                // add to the current line
                                line += paragraphWord + " ";
                                count += paragraphWord.Length;

                                //if its the very last item we want it to become the start of our line
                                if (i != paragraphSplit.Length - 1)
                                {
                                    //paragraph break
                                    output.Add(line);
                                    output.Add(Environment.NewLine);

                                    //reset
                                    line = string.Empty;
                                    count = 0;
                                }
                            }
                            else
                            {
                                //if its the very last item we want it to become the start of our line
                                if (i != paragraphSplit.Length - 1)
                                {
                                    //paragraph break
                                    output.Add(line);
                                    output.Add(paragraphWord);
                                    output.Add(Environment.NewLine);

                                    //reset
                                    line = paragraphWord + " ";
                                    count = paragraphWord.Length;
                                }
                            }
                        }
                    }
                    else
                    {
                        //not sure if this was an actual requirement but singl \n don't do anything so i just santized them
                        var santizedWord = word.Replace("\n", " ");

                        //check if proper length
                        if (santizedWord.Length + count <= 80)
                        {
                            // add to the current line
                            line += santizedWord + " ";
                            count += santizedWord.Length;
                        }
                        else
                        {
                            //line break
                            output.Add(line);

                            //reset
                            line = santizedWord + " ";
                            count = santizedWord.Length;
                        }
                    }
                }
                else
                {
                    //check if proper length
                    if (word.Length + count <= 80)
                    {
                        // add to the current line
                        line += word + " ";
                        count += word.Length;
                    }
                    else
                    {
                        //line break
                        output.Add(line);

                        //reset
                        line = word + " ";
                        count = word.Length;
                    }
                }
            }

            return output;
        }
*/
