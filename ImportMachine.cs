using System;
using C5;
using System.Threading;
using System.IO;
using System.ComponentModel.Design;
using System.Security.Cryptography;

namespace Civ.Data.Import
{
	/// <summary>
	/// Permite importar los datos del juego
	/// </summary>
	public static class ImportMachine
	{
		public static Global.GameData data = Global.Juego.Data;
		public static  string Directorio = AppDomain.CurrentDomain.BaseDirectory + "Data/";
		static IDictionary<string, IImportable> refs = new C5.TreeDictionary<string, IImportable> ();

		public static void Importar ()
		{
			// Leer los archivos en 
			var directorio = new DirectoryInfo (Directorio);
			var files = directorio.EnumerateFiles ();
			foreach (var file in files)
			{
				var reader = file.OpenText ();
				var header = file.Extension.ToLower ();
				IImportable current;
				var currentId = file.Name;

				if (!refs.Contains (currentId))
				{
					switch (header)
					{
						case ".recurso":
							current = new Recurso ();
							break;
						default:
							throw new Exception (string.Format (
								"No se encuentra clase {0}.",
								header));

					}
					refs.Add (currentId, current);
				}
				current = refs [currentId];
				current.Importar (reader);

				reader.Close ();
				reader.Dispose ();
			}
		}
	}
}