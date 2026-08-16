using System;
using TMPro;
using UnityEngine;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    [SerializeField] private TMP_Text createCodeText;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private GameObject loadingPanel;
    public ISession CurrentSession { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
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
        loadingPanel.SetActive(true);
        try
        {
            SessionOptions options = new SessionOptions
            {
                MaxPlayers = 8
            }.WithRelayNetwork();

            CurrentSession = await MultiplayerService.Instance.CreateSessionAsync(options);

            Debug.Log("세션 생성 성공");
            Debug.Log("방 코드 : " + CurrentSession.Code);

            NetworkManager.Singleton.SceneManager.LoadScene("WaitingRoom", LoadSceneMode.Single);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            loadingPanel.SetActive(false);
        }
    }

    public async void JoinSession()
    {
        loadingPanel.SetActive(true);
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
        finally
        {
            loadingPanel.SetActive(false);
        }
    }
}