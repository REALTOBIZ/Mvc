// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc.Localization.Internal;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Localization;

namespace Microsoft.Framework.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring MVC view localization.
    /// </summary>
    public static class MvcLocalizationMvcCoreBuilderExtensions
    {
        /// <summary>
        /// Adds MVC localization to the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// Adding localization also adds support for views via
        /// <see cref="MvcViewFeaturesMvcCoreBuilderExtensions.AddViews(IMvcCoreBuilder)"/> and the Razor view engine
        /// via <see cref="MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngine(IMvcCoreBuilder)"/>.
        /// </remarks>
        public static IMvcCoreBuilder AddViewLocalization([NotNull] this IMvcCoreBuilder builder)
        {
            return AddViewLocalization(builder, LanguageViewLocationExpanderFormat.Suffix);
        }

        /// <summary>
        ///  Adds MVC localization to the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="format">The view format for localized views.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// Adding localization also adds support for views via
        /// <see cref="MvcViewFeaturesMvcCoreBuilderExtensions.AddViews(IMvcCoreBuilder)"/> and the Razor view engine
        /// via <see cref="MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngine(IMvcCoreBuilder)"/>.
        /// </remarks>
        public static IMvcCoreBuilder AddViewLocalization(
            [NotNull] this IMvcCoreBuilder builder,
            LanguageViewLocationExpanderFormat format)
        {

            builder.AddViews();
            builder.AddRazorViewEngine();

            MvcLocalizationServices.AddLocalizationServices(builder.Services, format);
            return builder;
        }

        /// <summary>
        /// Adds MVC localization to the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setupAction">An action to configure the <see cref="LocalizationOptions"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// Adding localization also adds support for views via
        /// <see cref="MvcViewFeaturesMvcCoreBuilderExtensions.AddViews(IMvcCoreBuilder)"/> and the Razor view engine
        /// via <see cref="MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngine(IMvcCoreBuilder)"/>.
        /// </remarks>
        public static IMvcCoreBuilder AddViewLocalization(
            [NotNull] this IMvcCoreBuilder builder,
            Action<LocalizationOptions> setupAction)
        {
            return AddViewLocalization(builder, LanguageViewLocationExpanderFormat.Suffix, setupAction);
        }

        /// <summary>
        ///  Adds MVC localization to the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="format">The view format for localized views.</param>
        /// <param name="setupAction">An action to configure the <see cref="LocalizationOptions"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        /// <remarks>
        /// Adding localization also adds support for views via
        /// <see cref="MvcViewFeaturesMvcCoreBuilderExtensions.AddViews(IMvcCoreBuilder)"/> and the Razor view engine
        /// via <see cref="MvcRazorMvcCoreBuilderExtensions.AddRazorViewEngine(IMvcCoreBuilder)"/>.
        /// </remarks>
        public static IMvcCoreBuilder AddViewLocalization(
            [NotNull] this IMvcCoreBuilder builder,
            LanguageViewLocationExpanderFormat format,
            Action<LocalizationOptions> setupAction)
        {

            builder.AddViews();
            builder.AddRazorViewEngine();

            MvcLocalizationServices.AddLocalizationServices(builder.Services, format, setupAction);
            return builder;
        }
    }
}