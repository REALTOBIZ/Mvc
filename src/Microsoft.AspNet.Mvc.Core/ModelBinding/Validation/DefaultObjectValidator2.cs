// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// Recursively validate an object.
    /// </summary>
    public class DefaultObjectValidator : IObjectModelValidator
    {
        private readonly IList<IExcludeTypeValidationFilter> _excludeFilters;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultObjectValidator"/>.
        /// </summary>
        /// <param name="excludeFilters"><see cref="IExcludeTypeValidationFilter"/>s that determine
        /// types to exclude from validation.</param>
        /// <param name="modelMetadataProvider">The <see cref="IModelMetadataProvider"/>.</param>
        public DefaultObjectValidator(
            [NotNull] IList<IExcludeTypeValidationFilter> excludeFilters,
            [NotNull] IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
            _excludeFilters = excludeFilters;
        }

        public void Validate(
            IModelValidatorProvider validatorProvider,
            ModelStateDictionary modelState,
            ValidationStateDictionary validationState,
            object model)
        {
            var visitor = new ValidationVisitor(validatorProvider, modelState);
            visitor.Validate(validationState, model);
        }
    }
}
