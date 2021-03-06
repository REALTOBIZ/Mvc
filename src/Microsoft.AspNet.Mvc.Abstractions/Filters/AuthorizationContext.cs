// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Actions;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.Filters
{
    public class AuthorizationContext : FilterContext
    {
        public AuthorizationContext(
            [NotNull] ActionContext actionContext,
            [NotNull] IList<IFilterMetadata> filters)
            : base(actionContext, filters)
        {
        }

        public virtual IActionResult Result { get; set; }
    }
}
