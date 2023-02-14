using SixLabors.ImageSharp;
using TaleLearnCode.ThumbnailGenerator.Domain;

namespace TaleLearnCode.ThumbnailGenerator.Models;

internal class ImageDimensions
{

	internal ImageDimensions(
		Image image,
		ThumbnailGeneratorConfig thumbnailGeneratorConfig,
		int defaultThumbnailWidth,
		int defaultThumbnailHeight)
	{

		OriginHeight = image.Height;
		OriginWidth = image.Width;

		if (thumbnailGeneratorConfig.ThumbnailWidth == default && thumbnailGeneratorConfig.ThumbnailHeight == default)
		{
			ThumbnailWidth = defaultThumbnailWidth;
			ThumbnailHeight = defaultThumbnailHeight;
		}
		else if (thumbnailGeneratorConfig.ThumbnailWidth == default)
		{
			ThumbnailHeight = thumbnailGeneratorConfig.ThumbnailHeight;
			ThumbnailWidth = GetThumbnailDimension(image.Width, image.Height, ThumbnailHeight);
		}
		else
		{
			ThumbnailWidth = thumbnailGeneratorConfig.ThumbnailWidth;
			ThumbnailHeight = GetThumbnailDimension(image.Width, image.Height, ThumbnailHeight);
		}
	}

	private static int GetThumbnailDimension(
		int originalDimension,
		int opposingOriginalDimension,
		int opposingThumbnailDimension)
		=> Convert.ToInt32(Math.Round((decimal)(originalDimension / GetAspectRatio(opposingOriginalDimension, opposingThumbnailDimension))));

	private static int GetAspectRatio(
		int originDimension,
		int thumbnailDimension)
		=> originDimension / thumbnailDimension;

	internal int OriginHeight { get; set; }
	internal int OriginWidth { get; set; }
	public int ThumbnailHeight { get; set; }
	public int ThumbnailWidth { get; set; }
}