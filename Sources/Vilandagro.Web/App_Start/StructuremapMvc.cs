// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapMvc.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Vilandagro.Web.App_Start;
using Vilandagro.Web.Configuration;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructuremapMvc), "End")]

namespace Vilandagro.Web.App_Start
{
    using System.Web.Mvc;
    using DependencyResolution;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using StructureMap;
    
    public static class StructuremapMvc
    {
        #region Public Properties

        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }

        public static void Start()
        {
            var container = GetConfiguredContainer();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }

        public static IContainer GetConfiguredContainer()
        {
            IContainer container = IoC.Initialize();

            container.Configure(c =>
            {
                c.AddRegistry<MvcConfiguration>();
            });
            return container;
        }

        #endregion
    }
}