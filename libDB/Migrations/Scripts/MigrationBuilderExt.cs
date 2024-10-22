using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace libDB.Migrations.Scripts
{
    public static class MigrationBuilderExt
    {
        public static OperationBuilder<SqlOperation> SqlResource(this MigrationBuilder builder, string relativeFilePath)
        {
            using (Stream? stream = Assembly.GetAssembly(typeof(MigrationBuilderExt))
                .GetManifestResourceStream(relativeFilePath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    byte[] data = memoryStream.ToArray();
                    string text = Encoding.UTF8.GetString(data, 3, data.Length - 3);
                    return builder.Sql(text);
                }
            }
        }
    }
}
