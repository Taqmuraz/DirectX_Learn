using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectX_Learn
{
    public sealed class GameObject : NullBool
    {
        public readonly TransformComponent transform = new TransformComponent();
        public readonly string name;

        private readonly List<GameComponent> components = new List<GameComponent>();

        public GameObject (string name)
        {
            this.name = name;
        }

        public T AddComponent<T> () where T : GameComponent, new()
        {
            T c = new T();
            components.Add(c);
            c.OnAwake(this);
            return c;
        }
        public T GetComponent<T> () where T : class
        {
            GameComponent c = components.FirstOrDefault(comp => comp is T);
            return c as T;
        }
    }
}
