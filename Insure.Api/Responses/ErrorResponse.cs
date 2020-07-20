using Insure.Api.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Resources
{
    public class ErrorResponse<T> : Response
    {
        public T Errors { get; set; }
    }
}
