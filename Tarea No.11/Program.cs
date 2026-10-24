using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace Tarea_No._11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string opcion;
            GastoRepositorio repo = new GastoRepositorio();

            do
            {
                opcion = Menu();

                switch (opcion)
                {
                    case "1":
                        AgregarEnSqlServer(repo);
                        break;
                    case "2":
                        ListarEnSqlServer(repo);
                        break;
                    case "3":
                        BuscarPorId(repo);
                        break;
                    case "4":
                        ActualizarCosto(repo);
                        break;
                    case "5":
                        EliminarGasto(repo);
                        break;
                    case "6":
                        Console.WriteLine("Saliendo del Programa");
                        break;
                    default:
                        Console.WriteLine("Opcion No Valida");
                        break;
                }
            }
            while (opcion != "6");
        }

        public static string Menu()
        {
            Console.WriteLine();
            Console.WriteLine("Registro De Gastos");
            Console.WriteLine("Opcion 1: Registrar Gasto Con SqlServer");
            Console.WriteLine("Opcion 2: Listar Gastos Con SqlServer");
            Console.WriteLine("Opcion 3: Buscar Por Id Con SqlServer");
            Console.WriteLine("Opcion 4: Actualizar Monto Con SqlServer");
            Console.WriteLine("Opcion 5: Eliminar Gasto Con SqlServer");
            Console.WriteLine("Opcion 6: Salir");
            Console.Write("Escoja una Opcion: ");
            return Console.ReadLine();
        }

        public static void AgregarEnSqlServer(GastoRepositorio repo)
        {
            Console.WriteLine();
            Console.Write("Descripcion: ");
            string descripcion = Console.ReadLine().Trim();
            Console.Write("Monto: ");
            bool montoOk = decimal.TryParse(Console.ReadLine(), out decimal monto);
            Console.Write("Categoria: ");
            string Categoria = Console.ReadLine().Trim();

            if (string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(Categoria) || !montoOk || monto <= 0)
            {
                Console.WriteLine("Datos Invalidos");
                return;
            }

            repo.Insertar(new Gasto { descripcion = descripcion, monto = monto, categoria = Categoria });

        }
       
        public static void ListarEnSqlServer(GastoRepositorio repo)
        {
            Console.WriteLine();

            try
            {
                List<Gasto> lista = repo.ListarGastos();

                if (lista.Count == 0)
                {
                    Console.WriteLine("No Hay Productos en SQL Server");
                    return;
                }

                decimal TotalGastado = 0;

                foreach (Gasto g in lista)
                {
                    Console.WriteLine(g);
                    TotalGastado += g.monto;
                }

                Console.WriteLine($"Total Gastado: {TotalGastado}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al listar gastos en SQL Server: {ex.Message}");
            }
        }


        public static void BuscarPorId(GastoRepositorio repo)
        {
            Console.Write("Id del Producto a Buscar: ");
            if(!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }
            try
            {
                Gasto? gasto = repo.BuscarPorId(id);
                Console.WriteLine(gasto == null ? "No Existe Ese Id" :  gasto.ToString());
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error al buscar: {ex.Message}");
            }
        }
        public static void ActualizarCosto(GastoRepositorio repo)
        {

            Console.Write("Id del Gasto a Actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }
            try
            {
                Gasto? gasto = repo.BuscarPorId(id);
                if (gasto == null)
                {
                    Console.WriteLine("No existe ese Id.");
                    return;
                }
                Console.WriteLine($"Gasto Selecionado: {gasto}");
                Console.Write($"Nuevo Monto: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal monto) || monto <= 0)
                {
                    Console.WriteLine("Monto inválido");
                    return;
                }
                gasto.monto = monto;
                Console.WriteLine(repo.Actualizar(gasto) ? "Gasto Actualizado" : "No se actualizó");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al Actualizar {ex.Message}");
            }
        }
        private static void EliminarGasto(GastoRepositorio repo)
        {
            Console.Write("Id del producto a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }
            try
            {
                bool ok = repo.Eliminar(id);
                Console.WriteLine(ok ? "Eliminado" : "No existe ese Id");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
            }
        }
    }
}