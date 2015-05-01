/************************************************************************
 * * Copyright © 南宁市国土资源信息中心
 * * All rights reserved.
 * *
 * Depiction:   自定义异常
 * Author:      Michael Hou
 * CDT:         2011-05-12
 * Version:     1.0.0.2
 * Note:        
 * * --------------------------Refactoring-------------------------------
 * Rewriter:    
 * UDT:         
 * Desc:        
 ************************************************************************/

using System;
using System.Runtime.Serialization;

namespace Nlic.MicroStation.Interop {
    [Serializable]
    public class ViewNotOpenException : ApplicationException {
        public ViewNotOpenException() { }
        public ViewNotOpenException(string message) : base(message) { }
        public ViewNotOpenException(string message, Exception innerException) : base(message, innerException) { }
        protected ViewNotOpenException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidDgnElementException : ApplicationException {
        public InvalidDgnElementException() { }
        public InvalidDgnElementException(string message) : base(message) { }
        public InvalidDgnElementException(string message, Exception innerException) : base(message, innerException) { }
        protected InvalidDgnElementException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
