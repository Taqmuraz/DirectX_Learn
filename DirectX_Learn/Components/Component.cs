using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectX_Learn
{
    public class GameComponent : NullBool
    {
        public GameObject gameObject { get; private set; }
        public TransformComponent transform { get; private set; }

        public GameComponent ()
        {

        }
        public virtual void OnAwake (GameObject parent)
        {
            gameObject = parent;
            transform = parent.transform;
        }
        public virtual void OnDestroy ()
        {

        }
    }
}
