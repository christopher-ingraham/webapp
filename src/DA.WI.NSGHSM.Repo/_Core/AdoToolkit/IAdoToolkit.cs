using System.Data;

namespace DA.WI.NSGHSM.Repo._Core.AdoToolkit
{
    internal interface IAdoToolkit
    {

        string GetDatabaseName();
        bool IsDatabaseExisting();
        void DeleteDatabase();
        void CreateDatabase();


        string GetLastInsertedIdStatement(string tableName);

        void InitializeDatabase(string scriptToExcecute);
    }
}
