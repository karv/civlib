using System;
using System.Runtime.Serialization;

namespace Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	public static class g_
	{
        [DataMember (Name = "Data")]
		public static g_Data Data = new g_Data();
		public static g_State State = new g_State();

		private const string archivo = "Data.xml";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			Data = Store.Store<g_Data>.Deserialize (archivo);
		}

		public static void GuardaData() 
		{
			Store.Store<g_Data>.Serialize (archivo, Data);
		}

		public static void GuardaData(string f)
		{
			Store.Store<g_Data>.Serialize (f, Data);
		}

	}
}
