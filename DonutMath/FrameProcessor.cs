using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonutMath
{
    public class FrameProcessor
    {
        int dimensionLength = 0;
        public FrameProcessor()
        {
            // This is where the user can input the size of the space. too big can cause flickering, i suggest 20-30
            Console.WriteLine("Insert space size in integer:");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int size))
                {
                    dimensionLength = size;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
            // Specifically for the donut but can easily be generalized to other figures
            AnimateDonut();
        }

        private void AnimateDonut()
        {
            // Creates the 3d space
            int[,,] space = new int[dimensionLength, dimensionLength, dimensionLength];
            Donut donut = new Donut();
            // Inserts the donut into the space using the figure interface
            donut.InsertFigure(space);
            // Rotates the donut so it leans
            space = RotateObject(space, 45);
            // Creates a list of ascii frames
            List<string> animation = GetAnim(space);

            // Prints the frames to the console
            int top = Console.CursorTop;
            while (true)
            {
                foreach (string frame in animation)
                {
                    Console.SetCursorPosition(0, top);
                    Console.WriteLine(frame);
                    System.Threading.Thread.Sleep(50); // Adjust the sleep time if needed
                }
            }
        }
        private List<string> GetAnim(int[,,] space)
        {
            List<string> animation = new List<string>();

            //this is where i rotate it along Y axis to create the loop
            for (int i = 0; i < 360; i++)
            {
                int[,,] tempSpace = new int[space.GetLength(0), space.GetLength(1), space.GetLength(2)];

                for (int x = 0; x < tempSpace.GetLength(0); x++)
                {
                    for (int y = 0; y < tempSpace.GetLength(1); y++)
                    {
                        for (int z = 0; z < tempSpace.GetLength(2); z++)
                        {
                            if (space[x, y, z] != 0)
                            {
                                int val = space[x, y, z];
                                Point3D newPoint = RotateY( new Point3D(x, y, z),
                                                            new Point3D(space.GetLength(0) / 2, space.GetLength(1) / 2, space.GetLength(2) / 2),
                                                            i
                                                            );
                                // This is where the main rotation for the animation happens but its still a 3d space
                                tempSpace[(int)newPoint.X, (int)newPoint.Y, (int)newPoint.Z] = val;
                            }
                        }
                    }
                }
                // Since the object still is in a 3d space it has depth and we can raycast "photons" to create highlights
                ProjectLight(tempSpace);
                // Creates the frames
                animation.Add(CreateFrames(tempSpace));
            }
            return animation;
        }
        private string CreateFrames(int[,,] space)
        {
            // This is the 2d projection of the 3d space
            int[,] Projection = new int[space.GetLength(0), space.GetLength(1)];

            for (int y = 0; y < space.GetLength(1); y++)
            {
                for (int x = 0; x < space.GetLength(0); x++)
                {
                    for (int z = 0; z < space.GetLength(2); z++)
                    {
                        // 0 is empty, 1 is the donut, 2 is the light on the donut
                        if (space[x, y, z] == 2)
                        {
                            Projection[x, y] = 2;
                            break;
                        }
                        else if (space[x, y, z] == 1)
                        {
                            Projection[x, y] = 1;
                            break;
                        }
                    }

                }
            }

            // Construct string representation of the 2d array
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < Projection.GetLength(1); y++)
            {
                for (int x = 0; x < Projection.GetLength(0); x++)
                {
                    if (Projection[x, y] == 2)
                    {
                        sb.Append("#".PadLeft(2));
                    }
                    else if (Projection[x, y] == 1)
                    {
                        sb.Append("*".PadLeft(2));
                    }
                    else
                    {
                        sb.Append(".".PadLeft(2));
                    }

                }
                sb.AppendLine();
            }

            // The result is a string representation of the 2d projection of the 3d space
            return sb.ToString();
        }
        private void ProjectLight(int[,,] space)
        {
            for (int z = 0; z < space.GetLength(2); z++)
            {
                for (int x = 0; x < space.GetLength(0); x++)
                {
                    for (int y = 0; y < space.GetLength(1); y++)
                    {
                        if (space[x, y, z] == 1)
                        {
                            space[x, y, z] = 2;
                            break;
                        }
                    }
                }
            }
        }
        private int[,,] RotateObject(int[,,] space, int deg)
        {
            int[,,] tempSpace = new int[space.GetLength(0), space.GetLength(1), space.GetLength(2)];
            for (int x = 0; x < space.GetLength(0); x++)
            {
                for (int y = 0; y < space.GetLength(1); y++)
                {
                    for (int z = 0; z < space.GetLength(2); z++)
                    {
                        if (space[x, y, z] != 0)
                        {
                            int val = space[x, y, z];
                            Point3D newPoint = RotateX(
                                                        new Point3D(x, y, z),
                                                        new Point3D(space.GetLength(0) / 2, space.GetLength(1) / 2, space.GetLength(2) / 2),
                                                        deg
                                                        );
                            tempSpace[(int)newPoint.X, (int)newPoint.Y, (int)newPoint.Z] = val;
                        }
                    }
                }
            }
            return tempSpace;
        }
        private Point3D RotateX (Point3D point, Point3D axis, double deg)
        {
            double rad = deg * Math.PI / 180;

            double cosTheta = Math.Cos(rad);
            double sinTheta = Math.Sin(rad);
        
            double y = (point.Y - axis.Y) * cosTheta - (point.Z - axis.Z) * sinTheta + axis.Y;
            double z = (point.Y - axis.Y) * sinTheta + (point.Z - axis.Z) * cosTheta + axis.Z;
        
            return new Point3D(point.X, y, z);
        }
        private Point3D RotateY(Point3D point, Point3D axis, double angle)
        {
            double radians = angle * Math.PI / 180.0;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);

            double x = (point.X - axis.X) * cosTheta + (point.Z - axis.Z) * sinTheta + axis.X;
            double z = -(point.X - axis.X) * sinTheta + (point.Z - axis.Z) * cosTheta + axis.Z;

            return new Point3D(x, point.Y, z);
        }
        private Point3D RotateZ(Point3D point, Point3D axis, double angle)
        {
            double radians = angle * Math.PI / 180.0;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);

            double x = (point.X - axis.X) * cosTheta - (point.Y - axis.Y) * sinTheta + axis.X;
            double y = (point.X - axis.X) * sinTheta + (point.Y - axis.Y) * cosTheta + axis.Y;

            return new Point3D(x, y, point.Z);
        }
    }
}
