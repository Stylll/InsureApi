using Insure.Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Insure.Api.Responses
{
    public class SuccessResponse<T> : Response
    {
        public T Data { get; set; }
    }
}
