using System;

namespace WebAPI_DotNetCore_Demo.Domain.Entities
{
    public sealed class APILog
    {
        // Note that this Id is required by the Serilog library.
        // Its ommission has been attempted but to no avail (nothing logs after),
        // should you find a way to exclude it and make it work with EF. Please do!
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

        #region Enricher
        public string MachineName { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        #endregion

        public string Environment { get; set; }
    }
}
