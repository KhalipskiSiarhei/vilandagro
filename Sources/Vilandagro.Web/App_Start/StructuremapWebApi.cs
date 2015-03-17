// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructuremapWebApi.cs" company="Web Advanced">
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

using System.Web.Http;
using StructureMap;
using Vilandagro.Web.Configuration;
using Vilandagro.Web.DependencyResolution;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Vilandagro.Web.App_Start.StructuremapWebApi), "Start")]

namespace Vilandagro.Web.App_Start
{
    public static class StructuremapWebApi
    {
        public static void Start()
        {
            var container = GetConfiguredContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }

        public static IContainer GetConfiguredContainer()
        {
            var container = IoC.Initialize();
            container.Configure(c =>
            {
                c.AddRegistry<WebApiConfiguration>();
            });
            return container;
        }
    }
}