using System;

namespace WebAPI_DotNetCore_Demo.Options
{
    public sealed class HangfireSettings
    {
        public TimeSpan CommandBatchMaxTimeout { get; set; }
        public TimeSpan SlidingInvisibilityTimeout { get; set; }
        public TimeSpan QueuePollInterval { get; set; }
        public bool UseRecommendedIsolationLevel { get; set; }
        public bool DisableGlobalLocks { get; set; }
    }
}
