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

        bool initInProgress = false;

        void Start() {
            btn_download.onClick.AddListener(delegate
            {
                StartCoroutine(FetchImage());
            });
            btn_download.interactable = false;

            StartCoroutine(InitGoogleDrive());
        }

        IEnumerator InitGoogleDrive() {
            initInProgress = true;
            
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
                initInProgress = false;
            }

            Debug.Log("User Account: " + drive.UserAccount);
            initInProgress = false;

            btn_download.interactable = true;
        }

        IEnumerator FetchImage() {
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
        }
    }
}
