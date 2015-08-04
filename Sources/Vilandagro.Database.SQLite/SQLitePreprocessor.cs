using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp.Engine;

namespace Vilandagro.Database.SQLite
{
    public class SQLitePreprocessor : IScriptPreprocessor
    {
        public string Process(string contents)
        {
            // TODO: Replace it by using RegExp instead of Replace
            var content = 
                contents.Replace("GO;", string.Empty)
                    .Replace("SET IDENTITY_INSERT [Category] ON;", string.Empty)
                    .Replace("SET IDENTITY_INSERT [Category] OFF;", string.Empty);

            return content;
        }
    }
}