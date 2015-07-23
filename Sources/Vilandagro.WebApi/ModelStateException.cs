using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using Vilandagro.Core.Exceptions;

namespace Vilandagro.WebApi
{
    [Serializable]
    public class ModelStateException : BusinessException
    {
        public ModelStateException(ModelStateDictionary modelState)
            : this(string.Empty, modelState)
        {
        }

        public ModelStateException(string message, ModelStateDictionary modeState)
            : base(message)
        {
            ModelState = modeState;
        }

        public ModelStateDictionary ModelState { get; set; }
    }
}