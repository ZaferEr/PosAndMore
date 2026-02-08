using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdmin.Models
{
    public record ApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public T? Data { get; init; }
        public int StatusCode { get; init; }
        public string? ErrorMessage { get; init; }
        public Dictionary<string, string[]>? Errors { get; init; }

        public static ApiResponse<T> Success(T data, int statusCode = 200)
            => new() { IsSuccess = true, Data = data, StatusCode = statusCode };

        public static ApiResponse<T> Failure(int statusCode, string message, Dictionary<string, string[]>? errors = null)
            => new() { IsSuccess = false, StatusCode = statusCode, ErrorMessage = message, Errors = errors };

        public static ApiResponse<T> FromException(Exception ex, int statusCode = 500)
            => Failure(statusCode, ex.Message);
    }
}
