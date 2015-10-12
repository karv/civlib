using System;
using C5;
using System.IO;
using System.Threading;

namespace Civ.Data.Import
{
	/// <summary>
	/// Permite importar los datos del juego
	/// </summary>
	public static class ImportMachine
	{
		public static Global.GameData data = Global.Juego.Data;
		#if DEBUG
		public static  string Directorio = AppDomain.CurrentDomain.BaseDirectory + "Data/";
		#else
		public static  string Directorio = AppDomain.CurrentDomain.BaseDirectory + "Content/Data/";
		#endif
		readonly static IDictionary<string, IImportable> refs = new TreeDictionary<string, IImportable> ();

		/// <summary>
		/// Devuelve la dirección de un objeto en la que aparece o aparecerá un objeto con Id dado.
		/// </summary>
		/// <param name="id">Id del objeto</param>
		public static IImportable Valor (string id)
		{
			if (refs.Contains (id))
				return refs [id];
			else
				throw new Exception (id + " nunca se definió.");
		}

		/// <summary>
		/// Carga la información en el juego
		/// </summary>
		public static void Cargar ()
		{
			
		}

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
						case ".propiedad":
							current = new Propiedad ();
							break;
						case ".tasaprodconstante":
							current = new TasaProd.TasaProdConstante ();
							break;
						case ".tasaprodexp":
							current = new TasaProd.TasaProdExp ();
							break;
						case ".ecosistema":
							current = new Ecosistema ();
							break;
						case ".ciencia":
							current = new Ciencia ();
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

			// Crear referencias cruzadas
			foreach (var x in refs)
			{
				x.Value.Vincular ();
			}
		}
	}
}