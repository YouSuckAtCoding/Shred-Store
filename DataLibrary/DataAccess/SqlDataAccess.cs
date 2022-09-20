using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {

        private readonly IConfiguration config;

        public SqlDataAccess(IConfiguration _config)
        {
            config = _config;
        }
        /// <summary>
        /// Método para buscar dados do banco
        /// </summary>
        /// <typeparam name="T">Parametro genérico</typeparam>
        /// <typeparam name="U">Parametro genérico</typeparam>
        /// <param name="StoredProcedure">Stored Procedure feita na base de dados</param>
        /// <param name="parameters"></param>
        /// <param name="connectionId">Id da connection string</param>
        /// <returns>Dados</returns>
        public async Task<IEnumerable<T>> LoadData<T, U>(string StoredProcedure,
            U parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId));

            return await connection.QueryAsync<T>(StoredProcedure, parameters, commandType: CommandType.StoredProcedure);

        }
        /// <summary>
        /// Método para salvar no banco de dados
        /// </summary>
        /// <typeparam name="T">Parametro Genérico</typeparam>
        /// <param name="StoredProcedure">Stored Procedure feita na base de dados</param>
        /// <param name="parameters"></param>
        /// <param name="connectionId">Id da connection string</param>
        /// <returns>Nada.</returns>
        public async Task SaveData<T>(string StoredProcedure,
            T parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId));

            await connection.ExecuteAsync(StoredProcedure, parameters, commandType: CommandType.StoredProcedure);

        }
    }
}

