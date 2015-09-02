// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// Provides methods to validate an object graph.
    /// </summary>
    public interface IObjectModelValidator
    {
        void Validate(
            IModelValidatorProvider validatorProvider,
            ModelStateDictionary modelState,
            ValidationStateDictionary validationState,
            object model);
    }
}
