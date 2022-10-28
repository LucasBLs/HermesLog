using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Hermes.Models
{

    public class Log
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
        public Log() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

        public Log(string projectName, string functionName)
        {
            Exceptions = new List<ExceptionLog>();
            Informations = new List<InformationLog>();
            ProjectName = projectName;
            FunctionName = functionName;
            StartProcessDate = SetDateLocal();
        }

        [BsonId]
        public string? _id { get; set; }
        public string? Author { get; set; } = null!;
        public string? ProjectName { get; set; } = null;
        public string? FunctionName { get; set; } = null;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? StartProcessDate { get; set; } = null;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? EndProcessDate { get; set; } = null;
        public List<ExceptionLog> Exceptions { get; set; }
        public List<InformationLog> Informations { get; set; }

        public void SetInformation(string message, object? obj = null, Exception? expt = null, string? reference = null)
        {
            var _info = new InformationLog();
            var date = SetDateLocal();
            _info.Message = $"|#| {message}";
            _info.Object = JsonConvert.SerializeObject(obj ?? "");
            _info.Date = date;
            _info.Reference = reference;
            if (expt == null)
                _info.LogType = "Information";
            else
            {
                _info.LogType = "Error";
                SetException(message, expt ?? new Exception("Error"));
            }

            Informations.Add(_info);
        }

        public void AddInfoStep(string reference, string? message = null, object? obj = null)
        {
            foreach (var item in Informations)
                if (item.Reference == reference)
                {
                    object? itemObject = null;
                    if (obj != null)
                    {
                        itemObject = JsonConvert.SerializeObject(obj);
                    }

                    var step = new InfoStep() { Date = SetDateLocal(), Message = message, Object = itemObject };
                    item.Steps?.Add(step);
                }
        }

        public void SetException(string message, Exception exception)
        {
            var _exp = new ExceptionLog();
            var st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var date = SetDateLocal();

            _exp.Message = $"|#| {message}";
            _exp.Date = date;
            _exp.ExeptionMessage = exception.Message;
            if (frame != null)
            {
                _exp.FileName = frame.GetFileName() ?? "";
                _exp.MethodName = frame.GetMethod().Name ?? "";
                _exp.Line = frame.GetFileLineNumber();
            }

            Exceptions.Add(_exp);
        }

        public void SetEndProcess()
         => EndProcessDate = SetDateLocal();

        private DateTime SetDateLocal()
         => DateTime.Now.ToLocalTime();
    }


    public class InformationLog
    {
        public InformationLog()
            => Steps = new List<InfoStep>();
        public InformationLog(DateTime? date, string? reference, string? message, object? @object, string? logType)
        {
            Date = date;
            Reference = reference;
            Message = message;
            Object = @object;
            LogType = logType;
            Steps = new List<InfoStep>();
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        public string? Reference { get; set; }
        public string? Message { get; set; }
        public object? Object { get; set; }
        public string? LogType { get; set; }
        public List<InfoStep>? Steps { get; set; }

        public void addStep(InfoStep step)
            => Steps?.Add(step);
    }

    public class InfoStep
    {
        public InfoStep() { }
        public InfoStep(string? idInfo, DateTime? date, string? message, object? @object)
        {
            IdInfo = idInfo;
            Date = date;
            Message = message;
            Object = @object;
        }

        public string? IdInfo { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Date { get; set; }
        public string? Message { get; set; }
        public object? Object { get; set; }
    }

    public class ExceptionLog : InformationLog
    {
        public int? Line { get; set; }
        public string? MethodName { get; set; }
        public string? FrameName { get; set; }
        public string? FileName { get; set; }
        public string? ExeptionMessage { get; set; }
    }
}
