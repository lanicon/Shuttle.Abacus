using System.Data;
using Shuttle.Core.Data;

namespace Shuttle.Abacus.DataAccess
{
    public static class ValueSourceTypeColumns
    {
        public static readonly MappedColumn<string> Name = new MappedColumn<string>("Name", DbType.AnsiString);
        public static readonly MappedColumn<string> Text = new MappedColumn<string>("Text", DbType.AnsiString);
        public static readonly MappedColumn<string> Type = new MappedColumn<string>("Type", DbType.AnsiString);
    }
}
