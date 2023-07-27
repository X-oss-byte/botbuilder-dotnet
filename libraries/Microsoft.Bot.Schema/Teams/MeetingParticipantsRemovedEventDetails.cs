﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.Teams
{
    /// <summary>
    /// Specific details of a Teams meeting participants removed event.
    /// </summary>
    public partial class MeetingParticipantsRemovedEventDetails : MeetingEventDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingParticipantsRemovedEventDetails"/> class.
        /// </summary>
        public MeetingParticipantsRemovedEventDetails()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingParticipantsRemovedEventDetails"/> class.
        /// </summary>
        /// <param name="id">The meeting's Id, encoded as a BASE64 string.</param>
        /// <param name="joinUrl">The URL used to join the meeting.</param>
        /// <param name="title">The title of the meeting.</param>
        /// <param name="meetingType">The meeting's type.</param>
        /// <param name="participantsRemoved">The removed participant accounts.</param>
        public MeetingParticipantsRemovedEventDetails(
            string id,
            Uri joinUrl = null,
            string title = null,
            string meetingType = "Scheduled",
            IList<TeamsChannelAccount> participantsRemoved = default)
            : base(id, joinUrl, title, meetingType)
        {
            ParticipantsRemoved = participantsRemoved;

            CustomInit();
        }

        /// <summary>
        /// Gets the removed meeting participants.
        /// </summary>
        /// <value>
        /// The removed participant accounts.
        /// </value>
        [JsonProperty(PropertyName = "ParticipantsRemoved")]
        public IList<TeamsChannelAccount> ParticipantsRemoved { get; private set; } = new List<TeamsChannelAccount>();

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults.
        /// </summary>
        partial void CustomInit();
    }
}
