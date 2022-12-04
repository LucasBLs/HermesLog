using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes.Models;
using MongoDB.Bson;
using MongoDB.Driver;

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
                var collection = SetConfiguration.GetMongoCollection();
                var query = collection.AsQueryable().Where(x => x.StartProcessDate >= startProcessDateTime && x.EndProcessDate <= endProcessDateTime);
                var resultList = query.Skip(page).Take(pageSize).OrderByDescending(x => x.EndProcessDate).ToList();

                // var collection = SetConfiguration.GetMongoCollection();
                // var filter = Builders<Log>.Filter.Where(x => x.StartProcessDate >= startProcessDateTime && x.EndProcessDate <= endProcessDateTime);            
                // var resultList = await collection.Find(filter)
                //   .Skip((page - 1) * pageSize)
                //   .Limit(pageSize)                                    
                //   .SortByDescending(a => a.EndProcessDate)
                //   .ToListAsync();

                var response = new List<LogResponseModel>();
                foreach (var item in resultList)
                {
                    response.Add(new LogResponseModel
                    {
                        _id = item._id,
                        Author = item.Author,
                        EndProcessDate = item.EndProcessDate,
                        Exceptions = item.Exceptions,
                        FunctionName = item.FunctionName,
                        Informations = item.Informations,
                        ProjectName = item.ProjectName,
                        StartProcessDate = item.StartProcessDate
                    });
                }
                return await Task.FromResult(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}