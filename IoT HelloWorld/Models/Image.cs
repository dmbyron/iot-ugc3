using Windows.Storage;

namespace StudioUgc.Models
{
	public class Image : IUgc
	{
		public StorageFile File { get; set; }
		public string Source { get; private set; }
		public UgcType Type {  get { return UgcType.Image; } }

		public Image(StorageFile file)
		{
			File = file;
			Source = file.Path;
		}
	}
}
