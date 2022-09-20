namespace DataLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string StoredProcedure, U parameters, string connectionId = "Default");
        Task SaveData<T>(string StoredProcedure, T parameters, string connectionId = "Default");
    }
}