﻿using AutoFixture;
using SFX.System.Infrastructure;
using System;
using System.Linq;
using Xunit;

namespace SFX.System.Test.Unit.Infrastructure
{
    [Trait("Category", "Unit")]
    public class Base64ServiceTest
    {
        #region Members
        private readonly Fixture _fixture;

        private readonly byte[] _data;
        private readonly string _encodedData;
        #endregion

        #region Test initialization
        public Base64ServiceTest()
        {
            _fixture = new Fixture();

            _data = _fixture.CreateMany<byte>(1024).ToArray();
            _encodedData = Convert.ToBase64String(_data);
        }
        #endregion

        #region Type test
        [Fact]
        public void Base64Service_implements_IBase64Service()
        {
            Assert.True(typeof(IBase64Service).IsAssignableFrom(typeof(Base64Service)));
        }

        [Fact]
        public void Base64Service_is_sealed()
        {
            Assert.True(typeof(Base64Service).IsSealed);
        }
        #endregion

        #region ToBase64String
        [Fact]
        public void ToBase64String_of_null_data_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.ToBase64String(null);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void ToBase64String_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.ToBase64String(_data);

            Assert.True(success);
            Assert.Null(error);
            Assert.Equal(_encodedData, result);
        }
        #endregion

        #region FromBase64String
        [Fact]
        public void FromBase64String_of_null_data_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.FromBase64String(null);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void FromBase64String_of_empty_data_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.FromBase64String("");

            Assert.False(success);
            Assert.Equal("Unable to decode empty string", error);
            Assert.Null(result);
        }

        [Fact]
        public void FromBase64String_of_random_data_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.FromBase64String(_fixture.Create<string>());

            Assert.False(success);
            Assert.Equal("The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.", error);
            Assert.Null(result);
        }

        [Fact]
        public void FromBase64String_works()
        {
            var sut = new Base64Service();

            var (success, error, result) = sut.FromBase64String(_encodedData);

            Assert.True(success);
            Assert.Null(error);
            Assert.True(_data.SequenceEqual(result));
        }
        #endregion
    }
}
