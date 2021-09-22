using System.Data.Common;

namespace texo.data.Interfaces
{
    public interface IDatabaseBootstrap
    {
        void Setup();

        DbConnection GetConnection();

    }
}