using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WpfApp38.Models
{
    public class Parametras
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public static void WriteParametersToFile(List<Parametras> parameterList,string toFile)
        {
            StringBuilder path = new StringBuilder(Const.FileFolder);
            path.Append(toFile);
            StringBuilder parameters = new StringBuilder();
            foreach (var parametras in parameterList)
            {
                parameters.Append($"{parametras.Id}:{parametras.Value};");
            }
            File.WriteAllText(path.ToString(), parameters.ToString());
        }
        public static List<Parametras> ReadParametersFromFile(string path)
        {
            string temp = string.Empty;
            List<Parametras> parameters = new List<Parametras>();
            Parametras parametras = new Parametras();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    char c = (char)sr.Read();
                    if (c == ';')
                    {
                        parametras.Value = temp;
                        temp = string.Empty;
                        parameters.Add(parametras);
                        parametras = new Parametras();
                    }
                    else if (c == ':')
                    {
                        parametras.Id = temp;
                        temp = string.Empty;
                    }
                    else
                    {
                        temp += c;
                    }
                }
            }
            return parameters;
        }
    }
}
