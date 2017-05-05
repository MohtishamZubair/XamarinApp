using System;
using SQLite;

namespace SalesAgentDistribution.SQLLite
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();        
    }
}