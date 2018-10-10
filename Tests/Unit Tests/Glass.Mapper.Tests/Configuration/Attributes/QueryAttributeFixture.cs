using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class QueryAttributeFixture
    {
        [Test]
        public void Does_QueryAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(QueryAttribute)));
        }

        [Test]
        [TestCase("Query")]
        [TestCase("IsRelative")]
        [TestCase("InferType")]
        public void Does_QueryAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(QueryAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

       

        [Test]
        public void Does_Constructor_Set_IsRelative_False()
        {
            Assert.IsFalse(new StubQueryAttribute().IsRelative);
        }

        [Test]
        public void Does_Constructor_Set_InferType_False()
        {
            Assert.IsFalse(new StubQueryAttribute().InferType);
        }

        [Test]
        public void Does_Constructor_Set_Query()
        {
            var query = "This is a query";
            Assert.AreEqual(query, new StubQueryAttribute(query).Query);
        }

        #region Method - Configure

        [Test]
        public void Configure_DefaultValues_ConfigHasDefaults()
        {
            //Assign
            var attr = new StubQueryAttribute(string.Empty);
            var config = new QueryConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsFalse(config.InferType);
            Assert.IsFalse(config.IsRelative);
            Assert.IsEmpty(config.Query);
        }

        [Test]
        public void Configure_QueryHasValue_ConfigQueryHasValue()
        {
            //Assign
            var query = "some query";
            var attr = new StubQueryAttribute(query);
            var config = new QueryConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsFalse(config.InferType);
            Assert.IsFalse(config.IsRelative);
            Assert.AreEqual(query, config.Query);
        }

        [Test]
        public void Configure_InferTypeIsTrue_ConfigInferTypeIsTrue()
        {
            //Assign
            var attr = new StubQueryAttribute(string.Empty);
            var config = new QueryConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");
            
            attr.InferType = true;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsTrue(config.InferType);
            Assert.IsFalse(config.IsRelative);
            Assert.IsEmpty(config.Query);
        }

        [Test]
        public void Configure_IsRelativeIsTrue_ConfigIsRelativeIsTrue()
        {
            //Assign
            var attr = new StubQueryAttribute(string.Empty);
            var config = new QueryConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");

            attr.IsRelative = true;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsFalse(config.InferType);
            Assert.IsTrue(config.IsRelative);
            Assert.IsEmpty(config.Query);
        }

       

        #endregion

        #region Stubs

        private class StubQueryAttribute : QueryAttribute
        {
            public StubQueryAttribute() : base("This is a query") { }
            public StubQueryAttribute(string query) : base(query) { }

            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

		public class StubItem
		{
			public object X { get; set; }
		}

        #endregion
    }
}




