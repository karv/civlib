using System;

namespace Civ
{
    public partial class Trabajo
    {
        /// <summary>
        /// Ejecuta un tick de tiempo
        /// </summary>
        public void Tick(float t = 1)
        {
            // Obtener eficioencia (generada por la disponibilidad de recursos)
            float PctProd = 1;
            foreach (var x in RAW.EntradaBase.Keys)
            {
                PctProd = Math.Min(PctProd, Almacén[x] / (RAW.EntradaBase[x] * Trabajadores * t));
            }

            // Consumir recursos
            foreach (var x in RAW.EntradaBase.Keys)
            {
                Almacén[x] -= RAW.EntradaBase[x] * Trabajadores * PctProd * t;
            }


            // Producir recursos
            foreach (var x in RAW.SalidaBase.Keys)
            {
                Almacén[x] += RAW.SalidaBase[x] * Trabajadores * PctProd * t;
            }
        }
    }
}

