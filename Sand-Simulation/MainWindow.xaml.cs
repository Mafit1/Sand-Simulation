using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sand_Simulation
{
    public partial class MainWindow : Window
    {
        SandGrid _sandGrid;
        private const int CellSize = 4;
        private WriteableBitmap _bitmap;
        private DispatcherTimer _timer;

        private bool _isMouseDown = false;

        private Point _mousePosition;


        public MainWindow()
        {
            InitializeComponent();

            int gridWidth = 200;
            int gridHeight = 150;
            _sandGrid = new SandGrid(gridWidth, gridHeight);
            _bitmap = new WriteableBitmap(
                gridWidth * CellSize,
                gridHeight * CellSize,
                96, 96, PixelFormats.Bgra32, null);

            SandImage.Source = _bitmap;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(8);
            _timer.Tick += (s, e) => Update();
            _timer.Start();
        }


        private void Update()
        {
            _sandGrid.Update();
            DrawGrid();

            if (_isMouseDown)
            {
                var x = (int)_mousePosition.X / CellSize;
                var y = (int)_mousePosition.Y / CellSize;
                if (_sandGrid.Grid[x, y] == 0)
                    _sandGrid.AddSand(x, y);
            }
        }


        private void DrawGrid()
        {
            _bitmap.Lock();
            try
            {
                IntPtr buffer = _bitmap.BackBuffer;
                int stride = _bitmap.BackBufferStride;

                for (int x = 0; x < _sandGrid.Width; x++)
                {
                    for (int y = 0; y < _sandGrid.Height; y++)
                    {
                        byte pixel = _sandGrid.Grid[x, y];
                        uint color = pixel == 1 ? 0xFFFFD700 : 0xFF000000;

                        for (int i = 0; i < CellSize; i++)
                        {
                            for (int j = 0; j < CellSize; j++)
                            {
                                int posX = x * CellSize + i;
                                int posY = y * CellSize + j;
                                unsafe
                                {
                                    uint* pixelPtr = (uint*)(buffer + posY * stride + posX * 4);
                                    *pixelPtr = color;
                                }
                            }
                        }
                    }
                }
                _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
            }
            finally
            {
                _bitmap.Unlock();
            }
        }


        private void SandImage_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePosition = e.GetPosition(SandImage);
        }


        private void SandImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _mousePosition = e.GetPosition(SandImage);
        }


        private void SandImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }
    }
}