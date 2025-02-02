﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fpm.MainUI.ViewModels.Report
{
    public class ReportViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "profiles")]
        public List<int> Profiles { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public List<string> Parameters { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "isLive")]
        public bool IsLive { get; set; }

        [JsonProperty(PropertyName = "areaTypeIds")]
        public List<string> AreaTypeIds { get; set; }
    }
}