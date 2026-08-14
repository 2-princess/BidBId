using System;
using TMPro;
using UnityEngine;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text createCodeText;
    [SerializeField] private TMP_InputField joinCodeInput;

    private async void Start()
    {
        try
        {
            // Unity Gaming Services 준비
            await UnityServices.InitializeAsync();
            // 임시 플레이어 계정으로 로그인
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("로그인 성공");
            Debug.Log("Player ID : " + AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void CreateSession()
    {
        try
        {
            SessionOptions options = new SessionOptions
            {
                MaxPlayers = 8
            }.WithRelayNetwork();

            var session = await MultiplayerService.Instance.CreateSessionAsync(options);

            Debug.Log("세션 생성 성공");
            Debug.Log("방 코드 : " + session.Code);
            createCodeText.text = session.Code;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void JoinSession()
    {
        try
        {
            string joinCode = joinCodeInput.text;
            var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode);

            Debug.Log("세션 참가 성공");
            Debug.Log("Session ID : " + session.Id);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}