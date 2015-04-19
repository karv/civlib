using System;
using Global;
using System.IO;
using Civ;

namespace CivLibrary.Debug
{
	public interface IPlainSerializable
	{
		string PlainSerialize(int tabs);
	}

	/// <summary>
	/// Crea archivos para el usuario,
	/// permite que la edición sea más humana.
	/// </summary>
	public static class Debug
	{
		static Debug()
		{
		}

		/// <summary>
		/// Crea un archivo de g_.Data que sea humana leíble con reqs cruzados
		/// </summary>
		/// <param name="f">F.</param>
		public static void CrearArchivoLeible(string f)
		{
			g_Data glob = g_.Data; //Dandole nuevo nombre al archivo.
			StreamWriter sw = new StreamWriter(f, false);
			// System.IO.FileStream stream = new FileStream(f, FileMode.Create); // Sobreescribe.


			foreach (IPlainSerializable x in glob.Ciencias)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Edificios)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Propiedades)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Recursos)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Trabajos)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			foreach (IPlainSerializable x in glob.Unidades)
			{
				sw.WriteLine(x.PlainSerialize(0) + "\n");
			}

			sw.Close();

		}

		public static void CrearArchivoObjetosAbiertos(string f, object Obj)
		{
			g_Data glob = g_.Data; //Dandole nuevo nombre al archivo.
			StreamWriter sw = new StreamWriter(f, false);

			foreach (var x in glob.Ciencias)
			{
				if (Obj is Ciencia && x.Reqs.Ciencias.Contains((Ciencia)Obj))
					sw.WriteLine("(Ciencia)" + x);
				if (Obj is Recurso && x.Reqs.Recursos.ContainsKey((Recurso)Obj))
					sw.WriteLine("(Ciencia)" + x);
			}

			foreach (var x in glob.Edificios)
			{
				if (Obj is Recurso && x.ReqRecursos.ContainsKey((Recurso)Obj))
					sw.WriteLine("(Edificio)" + x);
				if (Obj is IRequerimiento<Ciudad> && x.Requiere.Requiere().Contains((IRequerimiento<Ciudad>)Obj))
					sw.WriteLine("(Edificio)" + x);
			}

			foreach (var x in glob.Trabajos)
			{
				if (x.Edificio == Obj)
					sw.WriteLine("(Trabajo)" + x);
				if (Obj is IRequerimiento<Ciudad> && x.Reqs().Contains((IRequerimiento<Ciudad>)Obj))
					sw.WriteLine("(Trabajo)" + x);
			}
			foreach (var x in glob.Unidades)
			{
				if (x.ReqCiencia == Obj)
					sw.WriteLine("(Unidad)" + x);
				if (Obj is Recurso && x.Reqs.ContainsKey((Recurso)Obj))
					sw.WriteLine("(Unidad)" + x);
			}

			sw.Close();
		}

		public static void CrearArchivoObjetosAbiertos()
		{
			string[] Dirs = {"Doc", "Doc/Ciencias", "Doc/Ecosistemas", "Doc/Edificios", "Doc/Propiedades", "Doc/Recursos", "Doc/Trabajos", "Doc/Unidades"};

			foreach (var x in Dirs) {
				if (!Directory.Exists(x))
					Directory.CreateDirectory(x);
			}

			foreach (var x in Global.g_.Data.Ciencias)
			{
				CrearArchivoObjetosAbiertos("Doc/Ciencias/" + x.Nombre + ".Ciencia.txt", x);
			}

			foreach (var x in Global.g_.Data.Ecosistemas)
			{
				CrearArchivoObjetosAbiertos("Doc/Ecosistemas/" + x.Nombre + ".Ecosistema.txt", x);
			}
			
			foreach (var x in Global.g_.Data.Edificios)
			{
				CrearArchivoObjetosAbiertos("Doc/Edificios/" + x.Nombre + ".Edificio.txt", x);
			}
			
			foreach (var x in Global.g_.Data.Propiedades)
			{
				CrearArchivoObjetosAbiertos("Doc/Propiedades/" + x.Nombre + ".Propiedad.txt", x);
			}
			
			foreach (var x in Global.g_.Data.Recursos)
			{
				CrearArchivoObjetosAbiertos("Doc/Recursos/" + x.Nombre + ".Recurso.txt", x);
			}
			
			foreach (var x in Global.g_.Data.Trabajos)
			{
				CrearArchivoObjetosAbiertos("Doc/Trabajos/" + x.Nombre + ".Trabajo.txt", x);
			}
			
			foreach (var x in Global.g_.Data.Unidades)
			{
				CrearArchivoObjetosAbiertos("Doc/Unidades/" + x.Nombre + ".Unidad.txt", x);
			}
		}
	}
}

