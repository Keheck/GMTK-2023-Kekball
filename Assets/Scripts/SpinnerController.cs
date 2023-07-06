using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SpinnerController : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpinAll();
        }
    }

    async void SpinAll() {
        int i = 0;
        List<UniTask> t = new List<UniTask>();
        Spinner[] spinners = FindObjectsOfType<Spinner>();
        foreach (Spinner spinner in spinners) {
            if (i++ < 4) {
                // start the first four all at once
                t.Add(spinner.Spin(60 * i));
            } else {
                // when all of the first four are done, continue each one at a time
                if(t != null) {
                    await UniTask.WhenAll(t.ToArray());
                    t = null;
                }
                await spinner.Spin(60 * i);
            }
        }
    }
}
