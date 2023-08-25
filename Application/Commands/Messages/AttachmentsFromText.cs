﻿using System.Text.RegularExpressions;
using Application.Models;

namespace Application.Commands.Messages
{
    internal static class AttachmentsFromText
    {
        private static readonly Regex _urlRegEx =
            new Regex(
                @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/=]*)");
        public static void GetAttachments(string input, Action<Attachment> onGet)
        {
            var urlCollection = _urlRegEx.Matches(input);
            foreach (Match match in urlCollection)
            {
                onGet(new Attachment
                {
                    IsInText = true,
                    Path = match.Value,
                    IsSpoiler = false
                });
            }
        }
    }
}