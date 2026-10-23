using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_No._11
{
    public class Gasto
    {
        public int id { get; set; }

        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public string categoria { get; set; }

        public override string ToString()
        {
            return $"{id}| Descripcion: {descripcion}| Monto: {monto}| Categoria: {categoria}";
        }
    }
}
