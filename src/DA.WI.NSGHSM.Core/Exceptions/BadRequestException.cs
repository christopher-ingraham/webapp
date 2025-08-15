using DA.WI.NSGHSM.Core.Extensions;
using System;
using System.Collections.Generic;

namespace DA.WI.NSGHSM.Core.Exceptions
{
    public enum BadRequestType
    {
        REQUIRED_FIELD,
        MISMATCH_ID,
        FIELD_MUST_BE_POSITIVE,
        ALREADY_EXISTS,
        FIELD_OUT_OF_RANGE,
        RELATED_RESOURCES_EXIST,
        FIELD_NOT_VALID,
        NOT_EXISTS
    }

    public class BadRequest {
        public BadRequestType ErrorType { get; }

        public string Key => ErrorType.ToString();

        public object Data { get; }

        public BadRequest(BadRequestType type, object data = null)
            // : base($"Bad request ({type}). Additional information: {data.ToJson()}")
        {
            ErrorType = type;
            Data = data;
        }
        
    }

    public class BadRequestException : ApplicationException
    {
        public Dictionary<string, BadRequest> badRequestMap { get; set; }

        public BadRequestException(BadRequestType type, object data = null)
            : base($"Bad request ({type}). Additional information: {data.ToJson()}")
        {
            badRequestMap = new Dictionary<string, BadRequest>();
            //badRequestMap.Add("", new BadRequest(type, data));
        }
        public BadRequestException()
            : base("")
        {
            badRequestMap = new Dictionary<string, BadRequest>();
        }
         public BadRequestException(Dictionary<string, BadRequest> _badRequestMap)
            : base("Validation error")
        {
            this.badRequestMap = _badRequestMap; 
        }
    }
}
