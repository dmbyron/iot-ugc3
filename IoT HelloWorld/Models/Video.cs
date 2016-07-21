using Windows.Storage;

namespace StudioUgc.Models
{
	public class Video : IUgc
	{
		public StorageFile File { get; set; }
		public string Source { get; private set; }
		public UgcType Type {  get { return UgcType.Video; } }

		public Video(StorageFile file)
		{
			File = file;
			Source = file.Path;
		}
	}
}
