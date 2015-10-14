using System;
using C5;
using System.IO;

namespace Civ.Data.Import
{
	/// <summary>
	/// Permite importar los datos del juego
	/// </summary>
	public static class ImportMachine
	{
		public static Global.GameData Data = Global.Juego.Data;
		#if DEBUG
		public static  string Directorio = AppDomain.CurrentDomain.BaseDirectory + "Data/";
		#else
		public static  string Directorio = AppDomain.CurrentDomain.BaseDirectory + "Content/CivObjects/";
		#endif
		readonly static IDictionary<string, IImportable> refs = new TreeDictionary<string, IImportable> ();

		/// <summary>
		/// Devuelve la dirección de un objeto en la que aparece o aparecerá un objeto con Id dado.
		/// </summary>
		/// <param name="id">Id del objeto</param>
		public static IImportable Valor (string id)
		{
			if (id == null)
				return null;
			if (refs.Contains (id))
				return refs [id];
			
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
						case ".edificio":
							current = new EdificioRAW ();
							break;
						case ".trabajo":
							current = new TrabajoRAW ();
							break;
						case ".unidad":
							current = new UnidadRAW ();
							break;
						case ".unidadcombate":
							current = new UnidadRAWCombate ();
							break;
						case ".unidadcolono":
							current = new UnidadRAWColono ();
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
				
			// Agregar a Data
			foreach (var x in refs)
			{
				var recurso = x.Value as Recurso;
				if (recurso != null)
				{
					Data.Recursos.Add (recurso);
				}

				var prp = x.Value as Propiedad;
				if (prp != null)
				{
					Data.Propiedades.Add (prp);
					continue;
				}

				var eco = x.Value as Ecosistema;
				if (eco != null)
				{
					Data.Ecosistemas.Add (eco);
					continue;
				}

				var cie = x.Value as Ciencia;
				if (cie != null)
				{
					Data.Ciencias.Add (cie);
					continue;
				}

				var edf = x.Value as EdificioRAW;
				if (edf != null)
				{
					Data.Edificios.Add (edf);
					continue;
				}

				var tbj = x.Value as TrabajoRAW;
				if (tbj != null)
				{
					Data.Trabajos.Add (tbj);
					continue;
				}

				var uni = x.Value as IUnidadRAW;
				if (uni != null)
				{
					Data.Unidades.Add (uni);
					continue;
				}
			}
		}
	}
}