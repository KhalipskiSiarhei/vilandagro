using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp.Engine;

namespace Vilandagro.Database.MySql
{
    public class MySqlPreprocessor : IScriptPreprocessor
    {
        public string Process(string contents)
        {
            // TODO: Replace it by using RegExp instead of Replace
            var content =
                contents.Replace("SET IDENTITY_INSERT [Category] ON;", string.Empty)
                    .Replace("SET IDENTITY_INSERT [Category] OFF;", string.Empty)
                    .Replace("IDENTITY", "AUTO_INCREMENT")
                    .Replace("GO;", string.Empty)
                    .Replace("[", string.Empty)
                    .Replace("]", string.Empty);

            return content;
        }
    }
}
