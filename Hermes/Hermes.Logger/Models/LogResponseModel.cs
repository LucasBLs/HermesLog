using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Hermes.Models
{
    public class LogResponseModel : Log
    {
        public new string? _id { get; set; }
        public new string? Author { get; set; }
        public new string? ProjectName { get; set; }
        public new string? FunctionName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public new DateTime? StartProcessDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public new DateTime? EndProcessDate { get; set; }
        public new List<ExceptionLog> Exceptions { get; set; } = new List<ExceptionLog>();
        public new List<InformationLog> Informations { get; set; } = new List<InformationLog>();
    }
}
