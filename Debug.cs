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
	}
}

