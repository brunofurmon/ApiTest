using System;


namespace ApiTest.Exceptions
{
    public class OrderException: Exception
    {
        public OrderException(string message, Exception inner = null) : base(message, inner) { }
    }
}