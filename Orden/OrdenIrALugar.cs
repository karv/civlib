using System;
using System.Security.Cryptography;
using System.Threading;
using System.Collections.Generic;
using Graficas.Rutas;
using System.Linq;

namespace Civ.Orden
{
	public class OrdenIrALugar:OrdenSerie
	{
		public OrdenIrALugar(Armada armada, Pseudoposicion destino) : base(armada)
		{
			var LongDict = new Dictionary<IRuta<Terreno>, float>();
			var origen = armada.Posicion;

			var RutaAA = Global.Juego.State.Topologia.CaminoÓptimo(origen.A, destino.A);
			var RutaAB = Global.Juego.State.Topologia.CaminoÓptimo(origen.A, destino.B);
			var RutaBA = Global.Juego.State.Topologia.CaminoÓptimo(origen.B, destino.A);
			var RutaBB = Global.Juego.State.Topologia.CaminoÓptimo(origen.B, destino.B);
			float LongAA = origen.loc + RutaAA.Longitud + destino.loc;
			float LongAB = origen.loc + RutaAB.Longitud + destino.aloc;
			float LongBA = origen.loc + RutaBA.Longitud + destino.loc;
			float LongBB = origen.aloc + RutaBB.Longitud + destino.aloc;
			LongDict.Add(RutaAA, LongAA);
			LongDict.Add(RutaAB, LongAB);
			LongDict.Add(RutaBA, LongBA);
			LongDict.Add(RutaBB, LongBB);

			// Biscar mínimo
			var min = new KeyValuePair<IRuta<Terreno>, float>(null, float.PositiveInfinity);
			foreach (var x in LongDict)
			{
				if (x.Value < min.Value)
					min = x;
			}

			//min es el mínimo



		

		}
	}
}

