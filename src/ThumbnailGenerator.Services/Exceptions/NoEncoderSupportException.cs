namespace TaleLearnCode.ThumbnailGenerator.Exceptions;

[Serializable]
public class NoEncoderSupportException : ThumbnailGeneratorException
{

	public NoEncoderSupportException() : base() { }

	public NoEncoderSupportException(string message) : base(message) { }

	public NoEncoderSupportException(string message, string operationId, HttpStatusCode status) : base(message, operationId, status) { }

	public NoEncoderSupportException(string message, Exception innerException) : base(message, innerException) { }

	public NoEncoderSupportException(string message, Exception innerException, string operationId, HttpStatusCode status) : base(message, innerException, operationId, status) { }

	public NoEncoderSupportException(SerializationInfo info, StreamingContext context) : base(info, context) { }

}