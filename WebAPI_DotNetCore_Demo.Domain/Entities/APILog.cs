using System;

namespace WebAPI_DotNetCore_Demo.Domain.Entities
{
    public sealed class APILog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Exception { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public string RequestBody { get; set; }
        public int ResponseStatusCode { get; set; }
        public string ResponseBody { get; set; }
        public double? ElapsedMs { get; set; }
	    public string UserName { get; set; }
        public string MachineName { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }
}
