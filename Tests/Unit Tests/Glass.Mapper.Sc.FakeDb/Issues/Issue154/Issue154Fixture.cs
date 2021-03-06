﻿using System;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue154
{
    [TestFixture]
    public class Issue154Fixture
    {
        [Test]
        public void LoadOrderInClassContext()
        {

            //Arrange 
            var resolver = Utilities.CreateStandardResolver();
            var context =  Context.Create(resolver);

            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(Class1)));

            var class1Config = context.GetTypeConfigurationFromType< SitecoreTypeConfiguration>(typeof (Class1));

            Assert.IsNotNull(class1Config);

        }

        public class Class1 
        {
            public virtual Guid Id { get; set; }
        }

        public class Class2 : Class1
        {
            public virtual string Title { get; set; }
        }
    }
}
