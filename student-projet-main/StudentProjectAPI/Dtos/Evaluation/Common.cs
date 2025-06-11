namespace StudentProjectAPI.DTOs.Common
{
    // DTO pour réponse générique
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ApiResponseDto<T> SuccessResponse(T data, string message = "Opération réussie")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseDto<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

    // DTO pour pagination
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    // DTO pour les types de livrables
    public class DeliverableTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AllowedExtensions { get; set; }
        public int MaxFileSize { get; set; }
        public List<string> AllowedExtensionsList => 
            AllowedExtensions?.Split(',', StringSplitOptions.RemoveEmptyEntries) 
                             .Select(ext => ext.Trim()) 
                             .ToList() ?? new List<string>();
    }
}