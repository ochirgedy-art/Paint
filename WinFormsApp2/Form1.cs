using System.Drawing.Imaging;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        bool isDrawing = false;
        Point startPoint;
        Point mouseLocation;
        Color currentColor = Color.Black;
      
        Point currentPoint;
        float brushSize = 5f;
        float eraserSize = 10f;

        private Bitmap drawingBitmap;
        private Graphics drawingGraphics;

        private enum ToolType { Pencil, Rectangle, Circle, Eraser }
        private ToolType currentTool = ToolType.Pencil;

        public Form1()
        {
            InitializeComponent();
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            button1.Click += button1_Click;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            button3.Click += button3_Click;
            button2.Click += button2_Click;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (drawingBitmap != null)
            {
                e.Graphics.DrawImage(drawingBitmap, Point.Empty);
            }

            
            }
        


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Move(object sender, EventArgs e)
        {

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                // Permanently draw the shape to the bitmap when releasing the mouse
                if (currentTool == ToolType.Rectangle || currentTool == ToolType.Circle)
                {
                    Rectangle rect = GetRectangle(startPoint, e.Location);
                    using (Pen pen = new Pen(currentColor, brushSize))
                    {
                        if (currentTool == ToolType.Rectangle)
                        {
                            drawingGraphics.DrawRectangle(pen, rect);
                        }
                        else if (currentTool == ToolType.Circle)
                        {
                            drawingGraphics.DrawEllipse(pen, rect);
                        }
                    }
                }

                isDrawing = false;
                panel1.Invalidate();


                isDrawing = false;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = e.Location;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                currentColor = cd.Color;
                button1.BackColor = cd.Color;
                currentTool = ToolType.Pencil;
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = e.Location;
            if (!isDrawing) return;

            if (currentTool == ToolType.Pencil)
            {
                using (Pen pen = new Pen(currentColor, brushSize))
                {
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    drawingGraphics.DrawLine(pen, startPoint, e.Location);
                }
                startPoint = e.Location;
                panel1.Invalidate();
            }
            else if (currentTool == ToolType.Eraser)
            {
                // Баллуур сонгогдсон үед цагаан өнгөөр, хэмжээгээр зурах
                using (Pen eraserPen = new Pen(Color.White, eraserSize))
                {
                    eraserPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                    eraserPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                    drawingGraphics.DrawLine(eraserPen, startPoint, e.Location);
                }
                startPoint = e.Location;
                panel1.Invalidate();
            }



        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            brushSize = (float)numericUpDown1.Value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int width = panel1.Width > 0 ? panel1.Width : 800;
            int height = panel1.Height > 0 ? panel1.Height : 600;

            drawingBitmap = new Bitmap(width, height);
            drawingGraphics = Graphics.FromImage(drawingBitmap);
            drawingGraphics.Clear(Color.White);

            this.DoubleBuffered = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Rectangle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Circle;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            currentTool = ToolType.Eraser;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            eraserSize = (float)numericUpDown2.Value;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (drawingGraphics != null)
            {
                drawingGraphics.Clear(Color.White);
                panel1.Invalidate();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                saveFileDialog.Title = "Зургийг хадгалах";
                saveFileDialog.FileName = "my_drawing";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat format = ImageFormat.Png;
                    if (saveFileDialog.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        format = ImageFormat.Jpeg;
                    }

                    drawingBitmap.Save(saveFileDialog.FileName, format);
                    MessageBox.Show("Зураг амжилттай хадгалагдлаа!", "Мэдээлэл", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private Rectangle GetRectangle(Point p1, Point p2)
        {
            int x = Math.Min(p1.X, p2.X);
            int y = Math.Min(p1.Y, p2.Y);
            int width = Math.Abs(p1.X - p2.X);
            int height = Math.Abs(p1.Y - p2.Y);
            return new Rectangle(x, y, Math.Max(width, 1), Math.Max(height, 1));
        }
    }
}
