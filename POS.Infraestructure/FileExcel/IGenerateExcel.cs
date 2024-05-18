using POS.Utilities.Static;

namespace POS.Infraestructure.FileExcel
{
    public interface IGenerateExcel
    {
        MemoryStream GenerateToExcel<T>(IEnumerable<T> data, List<TableColumn> columns);
    }
}
