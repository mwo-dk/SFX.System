﻿using System;
using System.Linq;
using System.Text;
using static SFX.Utils.Infrastructure.HashCodeHelpers;

namespace SFX.System.Model
{
    /// <summary>
    /// <see cref="ValueType"/> representation wrappring the salt used by the <see cref="EncryptionService"/>
    /// </summary>
    public sealed class Salt : IEquatable<Salt>
    {
        /// <summary>
        /// The salt value
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Valid means salt is not null
        /// </summary>
        /// <returns></returns>
        public bool IsValid() => !(Value is null) && Value.Length > 0;

        /// <summary>
        /// Convenience method to set the value from a unicode string
        /// </summary>
        /// <param name="value">The unicode string to set</param>
        public void SetStringValue(string value) =>
            Value = value is null ? null : Encoding.Unicode.GetBytes(value);

        /// <summary>
        /// Convenience method to get the value as a unicode string
        /// </summary>
        /// <returns>The salt value as a unicode string</returns>
        public string GetStringValue() =>
            Value is null ? null : Encoding.Unicode.GetString(Value);

        /// <summary>
        /// Compares current <see cref="Salt"/> to <paramref name="other"/>
        /// </summary>
        /// <param name="other">The <see cref="Salt"/> to be compared against</param>
        /// <returns>True if (ignoring case) the file paths are equal. Else false.</returns>
        public bool Equals(Salt other)
        {
            if (Value is null && other?.Value is null)
                return true;
            if (Value is null && !(other?.Value is null) ||
                (!(Value is null) && (other?.Value is null)))
                return false;
            if (Value.Length != other?.Value.Length)
                return false;

            return Value.SequenceEqual(other?.Value);
        }

        /// <summary>
        /// Overrides equals method
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if logically equal (ignoring case). Else false</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Salt))
                return false;
            return Equals((Salt)obj);
        }

        /// <summary>
        /// Determines the hash-code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() =>
            Value is null || Value.Length == 0 ? 0 : Value.ComputeHashCode(x => x);
    }
}
