﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Microsoft.Bot.Connector.Client.Models
{
    /// <summary> A response containing a resource ID. </summary>
    public partial class ResourceResponse
    {
        /// <summary> Initializes a new instance of ResourceResponse. </summary>
        public ResourceResponse()
        {
        }

        /// <summary> Initializes a new instance of ResourceResponse. </summary>
        /// <param name="id"> Id of the resource. </param>
        public ResourceResponse(string id)
        {
            Id = id;
        }

        /// <summary> Id of the resource. </summary>
        public string Id { get; }
    }
}
