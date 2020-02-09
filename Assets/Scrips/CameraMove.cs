using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraMove : MonoBehaviour
{
    //--------マウス関連------------
    // マウスポジション
    private Vector3 _mousePosition;

    // 前フレームのマウスポジション
    private Vector3 _OldMousePos;

    // 前後のFrameの座標の差
    private Vector3 _distance;

    // 1回目のフラグ
    private bool _isFirstFrame;

    // マウスホイールの回転値を格納する変数
    private float MouseScroll;

    //-------カメラ関連----------
    //カメラコンポーネント
    private Camera _camera;

    //カメラのズームスピード
    [SerializeField, Range(0.1f, 100.0f)]
    private float _ZoomSpeed = 2.0f;

    //カメラの移動スピード
    [SerializeField, Range(0.001f, 1.0f)]
    private float _MoveSpeed = 0.035f;

    //カメラのトランスフォーム
    private Vector3 _presentCamPos;


    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();

        _presentCamPos = this.gameObject.transform.position;

        _isFirstFrame = true;

        _OldMousePos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Director.Instance.QuitGame();
        }
        
        

        ///カメラのズーム
        {
            MouseScroll = Input.GetAxis("Mouse ScrollWheel");

            _camera.orthographicSize += MouseScroll * _ZoomSpeed * Time.deltaTime;

            if (_camera.orthographicSize < 0.3)
            {
                _camera.orthographicSize = 0.3f;
            }
        }


        ///カメラの上下左右移動
        {
            if(Input.GetMouseButton(0))
            {
                _mousePosition = Input.mousePosition;

                if(_isFirstFrame)
                {
                    _OldMousePos = _mousePosition;
                    _isFirstFrame = false;
                }

                _distance = (_mousePosition - _OldMousePos) * Time.deltaTime * _MoveSpeed;

                _camera.transform.position +=
                    new Vector3(_distance.x, -_distance.y, 0);

                _OldMousePos = _mousePosition;
            }

            if(Input.GetMouseButtonUp(0))
            {
                _isFirstFrame = true;
            }

        }




    }
}
