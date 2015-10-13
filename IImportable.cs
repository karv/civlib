using System.IO;

namespace Civ.Data.Import
{
	/// <summary>
	/// Representa un objeto que puede ser importado desde un archivo de texto
	/// </summary>
	public interface IImportable
	{
		/// <summary>
		/// Construye este objeto a partir d un archivo de textoe 
		/// </summary>
		/// <param name="reader">Stream de la información</param>
		void Importar (StreamReader reader);

		/// <summary>
		/// Crea las referencias.
		/// Se ejecuta al terminar de importar
		/// </summary>
		void Vincular ();
	}
}

