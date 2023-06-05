using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.GameCore.GameObjects
{
    interface IGameObject
    {
        public void Draw(ICanvas canvas);
    }
}
