namespace TaleLearnCode.ThumbnailGenerator.Exceptions;

[Serializable]
public class OriginBlobNotFoundException : ThumbnailGeneratorException
{

	public OriginBlobNotFoundException() : base() { }

	public OriginBlobNotFoundException(string message) : base(message) { }

	public OriginBlobNotFoundException(string message, string operationId, HttpStatusCode status) : base(message, operationId, status) { }

	public OriginBlobNotFoundException(string message, Exception innerException) : base(message, innerException) { }

	public OriginBlobNotFoundException(string message, Exception innerException, string operationId, HttpStatusCode status) : base(message, innerException, operationId, status) { }

	public OriginBlobNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}