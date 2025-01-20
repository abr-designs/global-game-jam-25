using UnityEngine;
using Utilities;

namespace Protoype.Alex
{
    public class Factory : HiddenSingleton<Factory>
    {
        [SerializeField]
        private Bullet bulletPrefab;

        public static Bullet CreateBullet()
        {
            return Instantiate(Instance.bulletPrefab, Instance.transform);
            
        }
    }
}