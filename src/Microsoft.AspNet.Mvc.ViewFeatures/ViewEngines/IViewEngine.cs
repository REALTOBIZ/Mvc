// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Mvc.Actions;

namespace Microsoft.AspNet.Mvc.ViewEngines
{
    /// <summary>
    /// Defines the contract for a view engine.
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        /// Finds the specified view by using the specified action context.
        /// </summary>
        /// <param name="context">The action context.</param>
        /// <param name="viewName">The name or full path to the view.</param>
        /// <returns>A result representing the result of locating the view.</returns>
        ViewEngineResult FindView(ActionContext context, string viewName);

        /// <summary>
        /// Finds the specified partial view by using the specified action context.
        /// </summary>
        /// <param name="context">The action context.</param>
        /// <param name="partialViewName">The name or full path to the view.</param>
        /// <returns>A result representing the result of locating the view.</returns>
        ViewEngineResult FindPartialView(ActionContext context, string partialViewName);
    }
}
