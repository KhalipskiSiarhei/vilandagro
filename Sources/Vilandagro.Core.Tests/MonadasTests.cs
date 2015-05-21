using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Core.Tests
{
    public class Person
    {
        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }

        public int? HouseNumber { get; set; }

        public string Postcode { get; set; }
    }

    [TestFixture]
    public class MonadasTests
    {
        private Person _person;

        private Address _address;

        [SetUp]
        public void SetUp()
        {
            _address = new Address() { HouseNumber = null, Postcode = "123456", Street = string.Empty };
            _person = new Person() { Address = _address};
        }

        [Test]
        public void With_ValueIsNull_NullReturned()
        {
            var houseNumber = _person.With(p => p.Address).With(a => a.HouseNumber);
            Assert.IsNull(houseNumber);
        }

        [Test]
        public void With_ValueIsNotNull_ValueReturned()
        {
            var postcode = _person.With(p => p.Address).With(a => a.Postcode);
            Assert.IsTrue(postcode == _address.Postcode);
        }

        [Test]
        public void With_ValueIsEmpty_NullReturned()
        {
            var street = _person.With(p => p.Address).With(a => a.Street);
            Assert.IsTrue(string.IsNullOrEmpty(street));
        }

        [Test]
        public void With_InitialValueIsNull_NullReturned()
        {
            _person.Address = null;
            var houseNumber = _person.With(p => p.Address).With(a => a.HouseNumber);
            var postcode = _person.With(p => p.Address).With(a => a.Postcode);

            Assert.IsNull(houseNumber);
            Assert.IsNull(postcode);
        }
    }
}