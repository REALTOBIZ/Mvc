// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using Microsoft.AspNet.Mvc.ActionConstraints;
using Microsoft.AspNet.Mvc.Actions;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Routing;

namespace Microsoft.AspNet.Mvc.WebApiCompatShim
{
    public class OverloadActionConstraint : IActionConstraint
    {
        public int Order { get; } = int.MaxValue;

        public bool Accept(ActionConstraintContext context)
        {
            var candidates = context.Candidates.Select(c => new
            {
                Action = c,
                Parameters = GetOverloadableParameters(c),
            });

            // Combined route value keys and query string keys. These are the values available for overload selection.
            var requestKeys = GetCombinedKeys(context.RouteContext);

            // Group candidates by the highest number of keys, and then process them until we find an action
            // with all parameters satisfied.
            foreach (var group in candidates.GroupBy(c => c.Parameters?.Count ?? 0).OrderByDescending(g => g.Key))
            {
                var foundMatch = false;
                foreach (var candidate in group)
                {
                    var allFound = true;
                    if (candidate.Parameters != null)
                    {
                        foreach (var parameter in candidate.Parameters)
                        {
                            if (!requestKeys.Contains(parameter.Prefix))
                            {
                                if (candidate.Action.Action == context.CurrentCandidate.Action)
                                {
                                    return false;
                                }

                                allFound = false;
                                break;
                            }
                        }
                    }

                    if (allFound)
                    {
                        foundMatch = true;
                    }
                }

                if (foundMatch)
                {
                    return group.Any(c => c.Action.Action == context.CurrentCandidate.Action);
                }
            }

            return false;
        }

        private List<OverloadedParameter> GetOverloadableParameters(ActionSelectorCandidate candidate)
        {
            if (candidate.Action.Parameters == null)
            {
                return null;
            }

            var isOverloaded = false;
            foreach (var constraint in candidate.Constraints)
            {
                if (constraint is OverloadActionConstraint)
                {
                    isOverloaded = true;
                }
            }

            if (!isOverloaded)
            {
                return null;
            }

            var parameters = new List<OverloadedParameter>();
            object optionalParametersObject;
            candidate.Action.Properties.TryGetValue("OptionalParameters", out optionalParametersObject);
            var optionalParameters = (HashSet<string>)optionalParametersObject;
            foreach (var parameter in candidate.Action.Parameters)
            {
                // We only consider parameters that are marked as bound from the URL.
                var source = parameter.BindingInfo?.BindingSource;
                if (source == null)
                {
                    continue;
                }

                if ((source.CanAcceptDataFrom(BindingSource.Path) ||
                    source.CanAcceptDataFrom(BindingSource.Query)) &&
                    CanConvertFromString(parameter.ParameterType))
                {
                    if (optionalParameters != null)
                    {
                        var isOptional = optionalParameters.Contains(parameter.Name);
                        if (isOptional)
                        {
                            // Optional parameters are ignored in overloading. If a parameter doesn't specify that it's
                            // required then treat it as optional (MVC default). WebAPI parameters will all by-default
                            // specify themselves as required unless they have a default value.
                            continue;
                        }
                    }

                    var prefix = parameter.BindingInfo?.BinderModelName ?? parameter.Name;

                    parameters.Add(new OverloadedParameter()
                    {
                        ParameterDescriptor = parameter,
                        Prefix = prefix,
                    });
                }
            }

            return parameters;
        }

        private static ISet<string> GetCombinedKeys(RouteContext routeContext)
        {
            var keys = new HashSet<string>(routeContext.RouteData.Values.Keys, StringComparer.OrdinalIgnoreCase);
            keys.Remove("controller");
            keys.Remove("action");

            var queryString = routeContext.HttpContext.Request.QueryString.ToUriComponent();

            if (queryString.Length > 0)
            {
                // We need to chop off the leading '?'
                var queryData = new FormDataCollection(queryString.Substring(1));

                var queryNameValuePairs = queryData.GetJQueryNameValuePairs();

                if (queryNameValuePairs != null)
                {
                    foreach (var queryNameValuePair in queryNameValuePairs)
                    {
                        keys.Add(queryNameValuePair.Key);
                    }
                }
            }

            return keys;
        }

        private static bool CanConvertFromString(Type destinationType)
        {
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
            return IsSimpleType(destinationType) ||
                   TypeDescriptor.GetConverter(destinationType).CanConvertFrom(typeof(string));
        }

        private static bool IsSimpleType(Type type)
        {
            return type.GetTypeInfo().IsPrimitive ||
                type.Equals(typeof(decimal)) ||
                type.Equals(typeof(string)) ||
                type.Equals(typeof(DateTime)) ||
                type.Equals(typeof(Guid)) ||
                type.Equals(typeof(DateTimeOffset)) ||
                type.Equals(typeof(TimeSpan)) ||
                type.Equals(typeof(Uri));
        }

        private class OverloadedParameter
        {
            public ParameterDescriptor ParameterDescriptor { get; set; }

            public string Prefix { get; set; }
        }
    }
}