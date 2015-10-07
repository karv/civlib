using System;
using Global;
using System.IO;
using Civ;

namespace CivLibrary.Debug
{
	public interface IPlainSerializable
	{
		string PlainSerialize (int tabs);
	}

	/// <summary>
	/// Crea archivos para el usuario,
	/// permite que la edición sea más humana.
	/// </summary>
	public static class Debug
	{
		static Debug ()
		{
		}

		/// <summary>
		/// Crea un archivo de g_.Data que sea humana leíble con reqs cruzados
		/// </summary>
		/// <param name="f">F.</param>
		public static void CrearArchivoLeible (string f)
		{
			GameData glob = Juego.Data; //Dandole nuevo nombre al archivo.
			var sw = new StreamWriter (f, false);
			// System.IO.FileStream stream = new FileStream(f, FileMode.Create); // Sobreescribe.


			foreach (IPlainSerializable x in glob.Ciencias)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Edificios)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Propiedades)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Recursos)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Trabajos)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Unidades)
			{
				sw.WriteLine (x.PlainSerialize (0) + "\n");
			}

			sw.Close ();

		}

		public static void CrearArchivoObjetosAbiertos (string f, object obj)
		{
			GameData glob = Juego.Data; //Dandole nuevo nombre al archivo.
			var sw = new StreamWriter (f, false);

			foreach (var x in glob.Ciencias)
			{
				if (obj is Ciencia && x.Reqs.Ciencias.Contains ((Ciencia)obj))
					sw.WriteLine ("(Ciencia)" + x);
				if (obj is Recurso && x.Reqs.Recursos.ContainsKey ((Recurso)obj))
					sw.WriteLine ("(Ciencia)" + x);
			}

			foreach (var x in glob.Edificios)
			{
				if (obj is Recurso && x.ReqRecursos.ContainsKey ((Recurso)obj))
					sw.WriteLine ("(Edificio)" + x);
				if (obj is IRequerimiento<ICiudad> && x.Requiere.Requiere ().Contains ((IRequerimiento<ICiudad>)obj))
					sw.WriteLine ("(Edificio)" + x);
			}

			foreach (var x in glob.Trabajos)
			{
				if (x.Edificio == obj)
					sw.WriteLine ("(Trabajo)" + x);
				if (obj is IRequerimiento<ICiudad> && x.Reqs ().Contains ((IRequerimiento<ICiudad>)obj))
					sw.WriteLine ("(Trabajo)" + x);
			}
			foreach (var x in glob.Unidades)
			{
				if (x.ReqCiencia == obj)
					sw.WriteLine ("(Unidad)" + x);
				if (obj is Recurso && x.Reqs.ContainsKey ((Recurso)obj))
					sw.WriteLine ("(Unidad)" + x);
			}

			sw.Close ();
		}

		public static void CrearArchivoObjetosAbiertos ()
		{
			string [] Dirs =
			{
				"Doc",
				"Doc/Ciencias",
				"Doc/Ecosistemas",
				"Doc/Edificios",
				"Doc/Propiedades",
				"Doc/Recursos",
				"Doc/Trabajos",
				"Doc/Unidades"
			};

			foreach (var x in Dirs)
			{
				if (!Directory.Exists (x))
					Directory.CreateDirectory (x);
			}

			foreach (var x in Juego.Data.Ciencias)
			{
				CrearArchivoObjetosAbiertos ("Doc/Ciencias/" + x.Nombre + ".Ciencia.txt", x);
			}

			foreach (var x in Juego.Data.Ecosistemas)
			{
				CrearArchivoObjetosAbiertos (
					"Doc/Ecosistemas/" + x.Nombre + ".Ecosistema.txt",
					x);
			}
			
			foreach (var x in Juego.Data.Edificios)
			{
				CrearArchivoObjetosAbiertos (
					"Doc/Edificios/" + x.Nombre + ".Edificio.txt",
					x);
			}
			
			foreach (var x in Juego.Data.Propiedades)
			{
				CrearArchivoObjetosAbiertos (
					"Doc/Propiedades/" + x.Nombre + ".Propiedad.txt",
					x);
			}
			
			foreach (var x in Juego.Data.Recursos)
			{
				CrearArchivoObjetosAbiertos ("Doc/Recursos/" + x.Nombre + ".Recurso.txt", x);
			}
			
			foreach (var x in Juego.Data.Trabajos)
			{
				CrearArchivoObjetosAbiertos ("Doc/Trabajos/" + x.Nombre + ".Trabajo.txt", x);
			}
			
			foreach (var x in Juego.Data.Unidades)
			{
				CrearArchivoObjetosAbiertos ("Doc/Unidades/" + x.Nombre + ".Unidad.txt", x);
			}
		}
	}
}

