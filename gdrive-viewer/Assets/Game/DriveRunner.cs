using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game {
    public class DriveRunner : MonoBehaviour {
        public GoogleDriveSettings settings;
        public MeshRenderer quadRenderer;

        public Button btn_download;

        GoogleDrive drive;

        void SetInteractable(bool b) {
            btn_download.interactable = b;
        }

        void Start() {
            btn_download.onClick.AddListener(delegate
            {
                StartCoroutine(FetchImage());
            });

            StartCoroutine(InitGoogleDrive());
        }

        private void Update() {
            // 기어VR 테스트 하려고 간단하게 만든거
            if(Input.GetButtonDown("Fire1")) {
                btn_download.onClick.Invoke();
            }
        }

        IEnumerator InitGoogleDrive() {
            SetInteractable(false);
            
            drive = new GoogleDrive(settings);
            drive.ClientID = settings.ClientID;
            drive.ClientSecret = settings.ClientSecret;
            drive.Scopes = new string[] {
                "https://www.googleapis.com/auth/drive.readonly",
                "https://www.googleapis.com/auth/userinfo.email",
            };

            var authorization = drive.Authorize();
            yield return StartCoroutine(authorization);

            if (authorization.Current is Exception) {
                Debug.LogWarning(authorization.Current as Exception);
                yield break;
            }

            Debug.Log("User Account: " + drive.UserAccount);
            SetInteractable(true);
        }

        IEnumerator FetchImage() {
            SetInteractable(false);

            var list = drive.ListFilesByQueary("title = '828046078431727618_1.jpg'");
            yield return StartCoroutine(list);
            var files = GoogleDrive.GetResult<List<GoogleDrive.File>>(list);

            if(files.Count == 0) {
                yield break;
            }

            var file = files[0];
            // Download file data.
            var download = drive.DownloadFile(file);
            yield return StartCoroutine(download);

            var bytes = GoogleDrive.GetResult<byte[]>(download);
            var tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            quadRenderer.material.mainTexture = tex;
            SetInteractable(true);
        }
    }
}
