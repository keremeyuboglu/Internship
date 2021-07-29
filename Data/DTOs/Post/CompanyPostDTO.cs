﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altamira.Data.DTOs.Post
{
    public class CompanyPostDTO
    {
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        [JsonProperty(PropertyName = "bs")]
        public string Business { get; set; }

    }
}
