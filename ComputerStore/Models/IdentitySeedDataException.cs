using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ComputerStore.Models
{
    public sealed class IdentitySeedDataException : Exception
    {
        public IdentitySeedDataException(string message)
            : base(message)
        {
        }

        public IdentitySeedDataException(string message, IEnumerable<IdentityError> errors)
            : base(message)
        {
            Errors = errors;
        }

        public IEnumerable<IdentityError> Errors { get; }
    }
}
