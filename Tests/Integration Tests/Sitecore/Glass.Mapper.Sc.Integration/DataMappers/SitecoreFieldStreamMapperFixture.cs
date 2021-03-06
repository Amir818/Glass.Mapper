/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    public class SitecoreFieldStreamMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField
        
        [Test]
        public void GetField_FieldContainsData_StreamIsReturned()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/GetField");
            var field = item.Fields[FieldName];
            string expected = "hello world";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
            var mapper = new SitecoreFieldStreamMapper();

            using (new ItemEditing(item, true))
            {
                field.SetBlobStream(stream);
            }

            //Act
            var result = mapper.GetField(field, null, null) as Stream;

            //Assert
            var reader = new StreamReader(result);

            var resultStr = reader.ReadToEnd();
            Assert.AreEqual(expected, resultStr);
        }

        [Test]
        public void GetField_FieldContainsDataTestConnectionLimit_StreamIsReturned()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/GetField");
            var field = item.Fields[FieldName];
            string expected = "hello world";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
            var mapper = new SitecoreFieldStreamMapper();

            using (new ItemEditing(item, true))
            {
                field.SetBlobStream(stream);
            }

            //Act
            var results = new List<Stream>();

            for (int i = 0; i < 1000; i++)
            {
                var result = mapper.GetField(field, null, null) as Stream;
                results.Add(result);
            }

            //Assert
            Assert.AreEqual(1000, results.Count);
        }


        #endregion


        #region Method - SetField

        [Test]
        public void SetField_StreamPassed_FieldContainsStream()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/SetField");
            var field = item.Fields[FieldName];
            string expected = "hello world";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(expected));
            var mapper = new SitecoreFieldStreamMapper();

            using (new ItemEditing(item, true))
            {
                field.SetBlobStream(new MemoryStream());
            }

            //Act
            using (new ItemEditing(item, true))
            {
              mapper.SetField(field, stream, null, null);
            }

            //Assert
            var reader = new StreamReader(field.GetBlobStream());

            var resultStr = reader.ReadToEnd();
            Assert.AreEqual(expected, resultStr);
        }

        [Test]
        public void SetField_NullPassed_NoExceptionThrown()
        {
            //Assign
            var fieldValue = "";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStreamMapper/SetField");
            var field = item.Fields[FieldName];
            string expected = "hello world";

            Stream stream = null;
            var mapper = new SitecoreFieldStreamMapper();

            using (new ItemEditing(item, true))
            {
                field.SetBlobStream(new MemoryStream());
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, stream, null, null);
            }

            //Assert
            var outStream = field.GetBlobStream();
            Assert.IsNull(outStream);

        }
        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldStreamMapper();
            var config = new SitecoreFieldConfiguration();

            config.PropertyInfo = typeof (StubClass).GetProperty("Stream");
            
            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public Stream Stream { get; set; }
        }
        #endregion
    }
}




