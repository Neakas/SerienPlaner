using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDragText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var parent = ((Label)sender).Parent;
            //if (parent.SelectedItem == null) parent.SelectedItem = e.Source;
            //if (e.Source != parent.SelectedItem) parent.SelectedItem = e.Source;
            //var data = e.Source;
            //if (data == null)
            //{
            //    return;
            //}
            //var dragData = new DataObject("Data", data);
            //DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Copy);
        }

        private static object GetDataFromListBox(ItemsControl source, Point point)
        {
            object control;
            try
            {
                var element = source.InputHitTest(point) as UIElement;
                if (element != null)
                {
                    var data = DependencyProperty.UnsetValue;
                    while (data == DependencyProperty.UnsetValue)
                    {
                        if (element == null) return null;
                        data = source.ItemContainerGenerator.ItemFromContainer(element);
                        if (data == DependencyProperty.UnsetValue)
                        {
                            element = VisualTreeHelper.GetParent(element) as UIElement;
                        }
                        if (!Equals(element, source))
                        {
                            continue;
                        }
                        return null;
                    }
                    if (data == DependencyProperty.UnsetValue)
                    {
                        return null;
                    }
                    control = data;
                }
                else
                {
                    control = null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
            return control;
        }

        private bool _isDown;
        private bool _isDragging;
        private Point _startPoint;
        private UIElement _realDragSource;
        private UIElement _dummyDragSource = new UIElement();
        private Cursor customCursor = null;

        private void sp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == this.sp)
            {
            }
            else
            {
                _isDown = true;
                _startPoint = e.GetPosition(this.sp);
            }
        }

        private void sp_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDown = false;
            _isDragging = false;
            _realDragSource.ReleaseMouseCapture();
        }

        private void sp_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) && ((Math.Abs(e.GetPosition(this.sp).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this.sp).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    _isDragging = true;
                    _realDragSource = e.Source as UIElement;
                    _dummyDragSource = _realDragSource;
                    _dummyDragSource.GiveFeedback += (sender1, e1) =>
                    {
                        if (e1.Effects == DragDropEffects.Move)
                        {
                            if (customCursor == null)
                                customCursor = CursorHelper.CreateCursor(e1.Source as UIElement);

                            if (customCursor != null)
                            {
                                e1.UseDefaultCursors = false;
                                Mouse.SetCursor(customCursor);
                            }
                            else
                                e1.UseDefaultCursors = true;

                            e.Handled = true;
                        }
                    };
                    _realDragSource.CaptureMouse();
                    DragDrop.DoDragDrop(_dummyDragSource, new DataObject("UIElement", e.Source, true), DragDropEffects.Move);
                }
            }
        }

        private void sp_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void sp_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UIElement"))
            {
                UIElement droptarget = e.Source as UIElement;
                int droptargetIndex = -1, i = 0;
                foreach (UIElement element in this.sp.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }
                if (droptargetIndex != -1)
                {
                    this.sp.Children.Remove(_realDragSource);
                    this.sp.Children.Insert(droptargetIndex, _realDragSource);
                }

                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }
    }
}
