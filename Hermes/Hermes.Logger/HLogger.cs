using Hermes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes
{
    public static class HLogger
    {
        public static async Task LogInformation(Log log)
        {
            log._id = ObjectId.GenerateNewId().ToString();
            var collection = SetConfiguration.GetMongoCollection();
            await collection.InsertOneAsync(log);
        }

        public static async Task<List<LogResponseModel>> SearchLogs(DateTime startProcessDateTime, DateTime endProcessDateTime, int page = 1, int pageSize = 10)
        {
            try
            {
                var response = new List<LogResponseModel>();
                var collection = SetConfiguration.GetMongoCollection();
               
                var filter = Builders<Log>.Filter.Where(x => x.StartProcessDate >= startProcessDateTime && x.EndProcessDate <= endProcessDateTime);            
                var resultList = await collection.Find(filter)
                  .Skip((page - 1) * pageSize)
                  .Limit(pageSize)                                    
                  .SortByDescending(a => a.EndProcessDate)
                  .ToListAsync();

                foreach (var item in resultList)
                {
                    var temp = new LogResponseModel()
                    {
                        _id = item._id,
                        Author = item.Author,
                        EndProcessDate = item.EndProcessDate,
                        Exceptions = item.Exceptions,
                        FunctionName = item.FunctionName,
                        Informations = item.Informations,
                        ProjectName = item.ProjectName,
                        StartProcessDate = item.StartProcessDate
                    };

                    response.Add(temp);               
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
