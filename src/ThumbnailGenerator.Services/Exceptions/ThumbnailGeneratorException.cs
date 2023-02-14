namespace TaleLearnCode.ThumbnailGenerator.Exceptions;

[Serializable]
public class ThumbnailGeneratorException : Exception
{

	public string? OperationId { get; set; }

	public HttpStatusCode Status { get; set; }

	public ThumbnailGeneratorException() : base() { }

	public ThumbnailGeneratorException(string message) : base(message) { }

	public ThumbnailGeneratorException(string message, string operationId, HttpStatusCode status) : base(message)
	{
		OperationId = operationId;
		Status = status;
	}

	public ThumbnailGeneratorException(string message, Exception innerException) : base(message, innerException) { }

	public ThumbnailGeneratorException(string message, Exception innerException, string operationId, HttpStatusCode status) : base(message, innerException)
	{
		OperationId = operationId;
		Status = status;
	}

	public ThumbnailGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}