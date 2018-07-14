﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Ai.Luis
{
    /// <summary>
    /// Strongly typed information corresponding to LUIS $instance value.
    /// </summary>
    public class InstanceData
    {
        /// <summary>
        /// Gets or sets 0-based index in the analyzed text for where entity starts.
        /// </summary>
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets 0-based index of the first character beyond the recognized entity.
        /// </summary>
        [JsonProperty("endIndex")]
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets word broken and normalized text for the entity.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets optional confidence in the recognition.
        /// </summary>
        [JsonProperty("score")]
        public double? Score { get; set; }

        /// <summary>
        /// Gets or sets any extra properties.
        /// </summary>
        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }
    }
}
