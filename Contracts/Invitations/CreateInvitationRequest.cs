﻿namespace Sparkle.Contracts.Invitations
{
    public record CreateInvitationRequest
    {
        /// <summary>
        /// Indicates whether to include user information in the invitation.
        /// </summary>
        public bool IncludeUser { get; set; }

        /// <summary>
        /// The expiration time of the invitation. (Optional)
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
