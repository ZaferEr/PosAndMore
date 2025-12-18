using System.Text;

namespace Dapper.SimpleLoadCore
{
    public interface IQuery
    {
        string Sql { get; }
        string SplitOn { get; }
    }
}