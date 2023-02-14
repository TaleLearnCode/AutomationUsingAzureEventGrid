namespace TaleLearnCode.ThumbnailGenerator.Models;

public class ExceptionResponse
{

	public ExceptionResponse(ThumbnailGeneratorException exception)
	{
		ExceptionType = (exception.GetType().ToString().Split('.')).Last();
		Message = exception.Message;
		StatusCode = exception.Status;
		OperationId = exception.OperationId;
	}

	public ExceptionResponse(
		Exception exception,
		HttpStatusCode statusCode,
		string? operationId = default)
	{
		ExceptionType = (exception.GetType().ToString().Split('.')).Last();
		Message = exception.Message;
		StatusCode = statusCode;
		if (!string.IsNullOrWhiteSpace(OperationId)) OperationId = operationId;
	}

	public string? OperationId { get; set; }
	public HttpStatusCode? StatusCode { get; set; }
	public string? ExceptionType { get; set; }
	public string? Message { get; set; }

}