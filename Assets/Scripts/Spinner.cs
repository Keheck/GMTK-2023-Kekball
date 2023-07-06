using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Spinner : MonoBehaviour {

    [SerializeField] float rotationSpeed = 60;

    public async UniTask<float> Spin(float speed = 200) {
        float startTime = Time.time;
        rotationSpeed = speed;
        float rotation = 0;
        while (rotation < 360) {
            rotation += Time.deltaTime * rotationSpeed;
            transform.eulerAngles = new Vector3(0, 0, rotation);
            await UniTask.Yield();
        }
        transform.eulerAngles = new Vector3(0, 0, 0);
        return Time.time - startTime;
    }
}
