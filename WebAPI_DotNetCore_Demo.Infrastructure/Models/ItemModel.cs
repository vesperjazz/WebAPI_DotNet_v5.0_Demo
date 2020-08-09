using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class ItemModel
    {
        public DateTime Timestamp { get; set; }
        public List<ReadingModel> Readings { get; set; }
    }
}
