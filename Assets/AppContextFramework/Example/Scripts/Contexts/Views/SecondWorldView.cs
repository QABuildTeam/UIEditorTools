using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACFW.Views;
using System.Threading.Tasks;

namespace ACFW.Example.Views
{
    public class SecondWorldView : WorldView
    {
        [SerializeField]
        private Transform cylinder;
        [SerializeField]
        private float angularVelocity = 180;

        private IEnumerator CylinderAnimation()
        {
            while (true)
            {
                cylinder.Rotate(new Vector3(angularVelocity * Time.deltaTime, 0, 0), Space.World);
                yield return null;
            }
        }

        public override async Task Show(bool force = false)
        {
            await base.Show(force);
            StartCoroutine(CylinderAnimation());
        }

        public override Task Hide()
        {
            StopAllCoroutines();
            return base.Hide();
        }
    }
}
