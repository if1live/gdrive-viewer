using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Game {
    public class GoogleDriveSettings : ScriptableObject, IGoogleDriveTokenStorage {
        [SerializeField]
        string _clientId = "488206440345-qn4rods7p9tgqnrg1rb5r21ekigg811u.apps.googleusercontent.com";

        [SerializeField]
        string _clientSecret = "bTAdDOYnMsZppcwCELvhjdB8";

        [SerializeField]
        string _accessToken;

        [SerializeField]
        string _refreshToken;

        [SerializeField]
        string _userAccount;

        public string ClientID { get { return _clientId; } }
        public string ClientSecret { get { return _clientSecret; } }

        public string AccessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        public string RefreshToken
        {
            get { return _refreshToken; }
            set { _refreshToken = value; }
        }

        public string UserAccount
        {
            get { return _userAccount; }
            set { _userAccount = value; }
        }
    }
}
