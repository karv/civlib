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
		/// <param name="file">Archivo de data</param>
		void Importar (StreamReader reader);

		/// <summary>
		/// Devuelve el identificador del objeto, para hacer referecias cruzadas
		/// </summary>
		string Id { get; }
	}
}

