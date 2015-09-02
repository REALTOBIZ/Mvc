// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class ValidationVisitor
    {
        private readonly IModelValidatorProvider _validatorProvider;
        private readonly ModelStateDictionary _modelState;
        private readonly ValidationStateDictionary _validationState;

        private object _container;
        private string _key;
        private object _model;
        private ModelMetadata _metadata;

        public ValidationVisitor(
            [NotNull] IModelValidatorProvider validatorProvider, 
            [NotNull] ModelStateDictionary modelState,
            [NotNull] ValidationStateDictionary validationState,
            object model)
        {
            _validatorProvider = validatorProvider;
            _modelState = modelState;
            _validationState = validationState;

            _model = model;
        }

        public bool Validate()
        {
            var entry = _validationState[_model];
            return Visit(entry.Key, entry.Metadata, _model);
        }

        private bool Visit(string key, ModelMetadata metadata, object model)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();

            _container = model;

            if (_metadata.IsCollectionType)
            {
                return VisitCollectionType();
            }
            else if (_metadata.IsComplexType)
            {
                return VisitComplexType();
            }
            else
            {
                return VisitSimpleType();
            }
        }

        private bool VisitCollectionType()
        {
            throw null;
        }

        private bool VisitComplexType()
        {
            foreach (var property in _metadata.Properties)
            {
                var model = property.PropertyGetter(_model);

                ValidationState entry;
                _
            }
        }

        private bool VisitSimpleType()
        {
            var validators = GetValidators(_metadata);

            var results = new List<ModelValidationResult>();
            foreach (var validator in validators)
            {
                var context = new ModelValidationContext()
                {
                    Container = _container,
                    Model = _model,
                    Metadata = _metadata,
                };

                results.AddRange(validator.Validate(context));
            }

            foreach (var result in results)
            {
                var key = ModelNames.CreatePropertyModelName(_key, result.MemberName);
                _modelState.TryAddModelError(key, result.Message);
            }

            var state = _modelState.GetFieldValidationState(_key);
            if (state == ModelValidationState.Invalid)
            {
                return false;
            }
            else
            {
                _modelState.MarkFieldValid(_key);
                return true;
            }
        }

        private IList<IModelValidator> GetValidators(ModelMetadata metadata)
        {
            var context = new ModelValidatorProviderContext(metadata);
            _validatorProvider.GetValidators(context);
            return context.Validators;
        }

        private struct Recursifier : IDisposable
        {
            private readonly ValidationVisitor _visitor;
            private readonly string _key;
            private readonly ModelMetadata _metadata;
            private readonly object _model;

            public static Recursifier GetBusy(ValidationVisitor vistor, string key, ModelMetadata metadata, object model)
            {
                return new Recursifier(visitor, key, metadata, model);
            }

            public Recursifier(ValidationVisitor visitor, string key, ModelMetadata metadata, object model)
            {
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
