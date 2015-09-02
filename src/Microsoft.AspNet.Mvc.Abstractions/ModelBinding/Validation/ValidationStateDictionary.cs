// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class ValidationStateDictionary : Dictionary<object, ValidationState>
    {
        public ValidationStateDictionary()
            : base(ReferenceEqualityComparer.Instance)
        {
        }

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

            public new bool Equals(object x, object y)
            {
                return Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }
    }

    public class ValidationState
    {
        public string Key { get; set; }

        public ModelMetadata Metadata { get; set; }
    }
}