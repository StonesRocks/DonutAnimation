using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonutMath
{
    public class Donut : Figure
    {
        public int[,,] InsertFigure(int[,,] emptySpace)
        {
            emptySpace = OuterSphere(emptySpace);
            return emptySpace;
        }

        private int[,,] OuterSphere(int[,,] emptySpace)
        {
            // Gets the size of the empty space
            int lengthX = emptySpace.GetLength(0);
            int lengthY = emptySpace.GetLength(1);
            int lengthZ = emptySpace.GetLength(2);

            // The outer and inner radius of the donut
            double outerRadius = lengthX / 2 * 0.7;
            double innerRadius = lengthX / 2 * 0.3;

            // The center of the donut
            Point3D centerPoint = new Point3D(lengthX/2, lengthY/2, lengthZ/2);

            for (int deg = 0; deg < 360; deg++)
            {
                double rad = deg * Math.PI / 180;
                // Advances the outer radius of the donut
                double outerX = Math.Cos(rad)*outerRadius+centerPoint.X;
                double outerY = Math.Sin(rad)*outerRadius+centerPoint.Y;
                double outerZ = centerPoint.Z;

                // Places a sphere at the position of the outer radius
                emptySpace = InnerSphere(emptySpace, new Point3D(outerX, outerY, outerZ), innerRadius);
            }
            return emptySpace;
        }

        private int[,,] InnerSphere(int[,,] emptySpace, Point3D center, double radius)
        {
            // Checks all points within the sphere and flips them to 1 by iterating through a cube with the inscribed sphere, and making sure its not out of bounds of the array.
            double cubeLength = radius * 2;
            double[] cubeStart = { center.X - radius, center.Y - radius, center.Z - radius };
            int length = emptySpace.GetLength(0);

            for (int x = (int)Math.Round(Math.Max(0d, cubeStart[0])); x < (int)Math.Round(Math.Min(length, cubeStart[0] + cubeLength)); x++)
            {
                for (int y = (int)Math.Round(Math.Max(0, cubeStart[1])); y < (int)Math.Round(Math.Min(length, cubeStart[1] + cubeLength)); y++)
                {
                    for (int z = (int)Math.Round(Math.Max(0, cubeStart[2])); z < (int)Math.Round(Math.Min(length, cubeStart[2] + cubeLength)); z++)
                    {
                        if (Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2) + Math.Pow(z - center.Z, 2) < Math.Pow(radius, 2))
                        {
                            emptySpace[x, y, z] = 1;
                        }
                    }
                }
            }
            return emptySpace;
        }

    }
}
