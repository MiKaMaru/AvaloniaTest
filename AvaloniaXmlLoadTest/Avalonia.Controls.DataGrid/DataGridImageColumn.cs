using System;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Avalonia.Controls
{
    public class DataGridImageColumn : DataGridBoundColumn
    {
        public DataGridImageColumn()
        {
            BindingTarget = Image.SourceProperty;
        }

        protected override IControl GenerateElement(DataGridCell cell, object dataItem)
        {
         var image=new Image();
         image.Stretch = Stretch.None;
         if (Binding != null)
            {
                image.Bind(BindingTarget, Binding);
            }
            return image;
        }

        protected override object PrepareCellForEdit(IControl editingElement, RoutedEventArgs editingEventArgs)
        {
            return false;
        }

        protected override IControl GenerateEditingElementDirect(DataGridCell cell, object dataItem)
        {
            var image = new Image();
            return image;
        }
    }
}
