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
    }
}
