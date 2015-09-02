// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Test
{
    public class ModelBindingResultTest
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange

            // Act
            var result = new ModelBindingResult(
                "someName",
                "some string",
                isModelSet: true,
                validationNode: null);

            // Assert
            Assert.Equal("some string", result.Model);
            Assert.True(result.IsModelSet);
            Assert.Equal("someName", result.Key);
            Assert.Null(result.ValidationNode);
        }
    }
}
