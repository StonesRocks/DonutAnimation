using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonutMath
{
    public interface Figure
    {
        //Interface for the figure to be inserted into the 3d array, could be used for other figures as well.

        public int[,,] InsertFigure(int[,,] emptySpace);
    }
}
