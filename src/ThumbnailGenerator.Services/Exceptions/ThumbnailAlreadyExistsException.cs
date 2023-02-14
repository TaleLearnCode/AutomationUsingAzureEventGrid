namespace TaleLearnCode.ThumbnailGenerator.Exceptions;

[Serializable]
public class ThumbnailAlreadyExistsException : ThumbnailGeneratorException
{

	public ThumbnailAlreadyExistsException() : base() { }

	public ThumbnailAlreadyExistsException(string message) : base(message) { }

	public ThumbnailAlreadyExistsException(string message, string operationId, HttpStatusCode status) : base(message, operationId, status) { }

	public ThumbnailAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

	public ThumbnailAlreadyExistsException(string message, Exception innerException, string operationId, HttpStatusCode status) : base(message, innerException, operationId, status) { }

	public ThumbnailAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}