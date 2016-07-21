using StudioUgc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace StudioUgc.Services
{
	public class UgcService
	{
		public async Task<IList<IUgc>> GetUgc(String path)
		{
			//var path = "\\\\WIN81-BZVECSJS\\StudioUgc";
			//var path = "C:\\Users\\dillon.byron\\Pictures\\PresentationPhotos";

			var folder = await StorageFolder.GetFolderFromPathAsync(path);
			var fileList = await folder.GetFilesAsync();
			var ugc = fileList.Select(CreateUgc).Where(x => x != null).ToList();

			return ugc;
		}

		private IUgc CreateUgc(StorageFile file)
		{
			switch (file.FileType.ToLower())
			{
				case ".jpg":
				case ".png":
				case ".bmp":
					return new Image(file);
				case ".mp4":
					return new Video(file);
			}
			return null;
		}

		//private Image CreatePhotoUgc(StorageFile file)
		//{
		//	return new Image(file);
		//}

		//private Video CreateVideoUgc(StorageFile file)
		//{
		//	return new Video(file);
		//}
	}
}
