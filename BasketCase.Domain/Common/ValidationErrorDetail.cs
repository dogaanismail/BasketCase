﻿namespace BasketCase.Domain.Common
{
    public class ValidationErrorDetail
    {
        public string Field { get; set; }
        public string Message { get; set; }


        public ValidationErrorDetail(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}
