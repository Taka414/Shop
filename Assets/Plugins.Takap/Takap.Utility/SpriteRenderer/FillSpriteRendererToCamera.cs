//
// (C) 2022 Takap.
//

using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// SpriteRendererを指定したカメラの画面いっぱいに広げます。
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class FillSpriteRendererToCamera : MonoBehaviour
    {
        //
        // Inspector
        // - - - - - - - - - - - - - - - - - - - -

        // フィット対象のカメラ
        [SerializeField] Camera _camera;
        // カメラとの距離
        [SerializeField] float _cameraDistance = 10.0f;

        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        // フィットさせる画像
        SpriteRenderer _sr;
        // 画面サイズ
        float _size;
        float _height;
        float _width;
        // 自分自身のキャッシュ
        Transform _transform;
        // カメラの位置
        Transform _cameraTransform;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -


        //
        // Unity Events
        // - - - - - - - - - - - - - - - - - - - -

        void Awake()
        {
            this.SetComponent(ref _sr);
            _transform = transform;

            // 未設定ならメインカメラを仮設定する
            if (!_camera)
            {
                _camera = Camera.main;
            }
            _cameraTransform = _camera.transform;
        }

        void Update()
        {
            FollowCamera();


            float _tmpHeight = Screen.height;
            float _tmpWidth = Screen.width;
            if (_size == _camera.orthographicSize && _height == _tmpHeight && _width == _tmpWidth)
            {
                return;
            }
            _size = _camera.orthographicSize;
            _height = _tmpHeight;
            _width = _tmpWidth;

            FitImage();
        }

        //
        // Methods
        // - - - - - - - - - - - - - - - - - - - -

        void FitImage()
        {
            if (_sr.sprite == null)
            {
                Log.Warn("SpriteRendererに画像が設定されていません。", this);
                return;
            }

            switch (_sr.drawMode)
            {
                case SpriteDrawMode.Simple: FitImageSimple(); break;
                case SpriteDrawMode.Sliced:
                case SpriteDrawMode.Tiled: FitImageTiled(); break;
                default: break; // nop
            }
        }

        // Simpleの時に画面いっぱいに広げる、Transform.scaleを操作する
        void FitImageSimple()
        {
            // カメラの外枠のスケールをワールド座標系で取得
            float worldScreenHeight = _camera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            // スプライトのスケールもワールド座標系で取得
            float width = _sr.sprite.bounds.size.x;
            float height = _sr.sprite.bounds.size.y;

            //  両者の比率を出してスプライトのローカル座標系に反映
            _transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);
        }

        // SpriteRenderer.sizeを操作する
        void FitImageTiled()
        {
            // カメラのサイズを取得
            float cameraHeight = _camera.orthographicSize * 2.0f;
            float cameraWidth = cameraHeight * _camera.aspect;

            _transform.SetLocalScale(1);
            _sr.size = new Vector2(cameraWidth, cameraHeight);
        }

        // カメラの位置に追従する(3D回転を考慮)
        void FollowCamera()
        {
            // カメラの位置からカメラの前方ベクトルに距離をかけた位置
            Quaternion rotation = _cameraTransform.rotation;
            Vector3 desiredPosition = _cameraTransform.position + (rotation * Vector3.forward * _cameraDistance);

            // 自分自身の距離と回転を反映する
            _transform.position = desiredPosition;
            transform.rotation = rotation;
        }
    }
}
