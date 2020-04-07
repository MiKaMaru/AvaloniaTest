using System.Diagnostics;
using Avalonia;

namespace AvaloniaXmlLoadTest.ParmaDataGrid
{
	/// <summary>
	/// Extensions для parmaDataGrid.
	/// </summary>
	public static class ParmaDataGridExtensions
	{
		public static AppBuilder UseParmaDataGrid(this AppBuilder appBuilder)
		{
			// Костыль. Особенность родного DataGrid. Нужно как-то "обозначить" тип до его полноценного использования.
			Trace.WriteLine(typeof(ParmaDataGrid));
			Trace.WriteLine(typeof(ParmaDataGridOd));
			return appBuilder;
		}
	}
}
