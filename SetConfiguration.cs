using Hermes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Hermes
{
    public static class SetConfiguration
    {
        static string ConnectionString = "mongodb://localhost:27017";
        static readonly string DataBaseName = "HermesLog";
        static string ProjectName = "HermesLogTest";

        /// <summary>
        /// Atribuir informações para o banco de dados
        /// <param name="connectionString">ConnectioString to database MongoDB</param>    
        /// <param name="projectName">Project Name to create table</param>
        /// </summary>
        public static void ConfigureDatabase(string connectionString, string projectName)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(projectName))
                {
                    throw new ArgumentNullException("Params to create database invalid");
                }

                ConnectionString = connectionString;
                ProjectName = projectName;

                CreateDataBase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void CreateDataBase()
        {
            try
            {
                var client = new MongoClient(ConnectionString);
                var database = client.GetDatabase(DataBaseName);

                var filter = new BsonDocument("name", ProjectName);
                var options = new ListCollectionNamesOptions { Filter = filter };

                var isExistDocumentCollection = database.ListCollectionNames(options).Any();

                if (!isExistDocumentCollection)
                    database.CreateCollection(ProjectName);            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static IMongoCollection<Log> GetMongoCollection()
        {
            try
            {
                var client = new MongoClient(ConnectionString);
                var database = client.GetDatabase(DataBaseName);
                return database.GetCollection<Log>(ProjectName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
