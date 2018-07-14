using System;
using Bubbio.Repository.MongoDb.Tests.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Builders
{
    public class TestDocumentBuilder
    {
        private readonly TestDocument _testDocument;

        public TestDocumentBuilder()
        {
            _testDocument = new TestDocument
            {
                Id = Guid.NewGuid(),
                Name = "Martin Ponce",
                Version = 1
            };
        }

        public TestDocumentBuilder WithId(Guid id)
        {
            _testDocument.Id = id;
            return this;
        }

        public TestDocumentBuilder WithName(string name)
        {
            _testDocument.Name = name;
            return this;
        }

        public TestDocumentBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _testDocument.Timestamp = timestamp;
            return this;
        }

        public TestDocumentBuilder WithVersion(int version)
        {
            _testDocument.Version = version;
            return this;
        }

        public TestDocument Build()
        {
            return _testDocument;
        }
    }
}