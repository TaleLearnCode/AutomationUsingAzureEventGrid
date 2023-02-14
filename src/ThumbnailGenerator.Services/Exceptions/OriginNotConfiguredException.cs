namespace TaleLearnCode.ThumbnailGenerator.Exceptions;

[Serializable]
public class OriginNotConfiguredException : ThumbnailGeneratorException
{

	public OriginNotConfiguredException() : base() { }

	public OriginNotConfiguredException(string message) : base(message) { }

	public OriginNotConfiguredException(string message, string operationId, HttpStatusCode status) : base(message, operationId, status) { }

	public OriginNotConfiguredException(string message, Exception innerException) : base(message, innerException) { }

	public OriginNotConfiguredException(string message, Exception innerException, string operationId, HttpStatusCode status) : base(message, innerException, operationId, status) { }

	public OriginNotConfiguredException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}