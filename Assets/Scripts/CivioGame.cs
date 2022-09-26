using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class CivioGame : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("Time Remaining until the game starts")]
    private float m_DelayedStartTime = 5.0f;

    //These help to simplify checking server vs client
    //[NSS]: This would also be a great place to add a state machine and use networked vars for this
    private bool m_ClientGameOver;
    private bool m_ClientGameStarted;
    private bool m_ClientStartCountdown;

    private NetworkVariable<bool> m_CountdownStarted = new NetworkVariable<bool>(false);

    // the timer should only be synced at the beginning
    // and then let the client to update it in a predictive manner
    private bool m_ReplicatedTimeSent = false;
    private float m_TimeRemaining;

    public static CivioGame Singleton { get; private set; }

    public NetworkVariable<bool> hasGameStarted { get; } = new NetworkVariable<bool>(false);

    public NetworkVariable<bool> isGameOver { get; } = new NetworkVariable<bool>(false);

    /// <summary>
    ///     Awake
    ///     A good time to initialize server side values
    /// </summary>
    void Awake()
    {
        Assert.IsNull(Singleton, $"Multiple instances of {nameof(CivioGame)} detected. This should not happen.");
        Singleton = this;

        OnSingletonReady?.Invoke();

        if (IsServer)
        {
            hasGameStarted.Value = false;

            //Set our time remaining locally
            m_TimeRemaining = m_DelayedStartTime;

            //Set for server side
            m_ReplicatedTimeSent = false;
        }
        else
        {
            //We do a check for the client side value upon instantiating the class (should be zero)
            Debug.LogFormat("Client side we started with a timer value of {0}", m_TimeRemaining);
        }

        TileAutomata.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        //Is the game over?
        if (IsCurrentGameOver()) return;

        //Update game timer (if the game hasn't started)
        UpdateGameTimer();

        //If we are a connected client, then don't update the enemies (server side only)
        if (!IsServer) return;

        //If we are the server and the game has started, then update the enemies
        //if (HasGameStarted()) UpdateEnemies();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsServer)
        {
            //m_Enemies.Clear();
            //m_Shields.Clear();
        }

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    internal static event Action OnSingletonReady;

    public override void OnNetworkSpawn()
    {
        if (IsClient && !IsServer)
        {
            m_ClientGameOver = false;
            m_ClientStartCountdown = false;
            m_ClientGameStarted = false;

            m_CountdownStarted.OnValueChanged += (oldValue, newValue) =>
            {
                m_ClientStartCountdown = newValue;
                Debug.LogFormat("Client side we were notified the start count down state was {0}", newValue);
            };

            hasGameStarted.OnValueChanged += (oldValue, newValue) =>
            {
                m_ClientGameStarted = newValue;
                Debug.LogFormat("Client side we were notified the game started state was {0}", newValue);
            };

            isGameOver.OnValueChanged += (oldValue, newValue) =>
            {
                m_ClientGameOver = newValue;
                Debug.LogFormat("Client side we were notified the game over state was {0}", newValue);
            };
        }

        //Both client and host/server will set the scene state to "ingame" which places the PlayerControl into the SceneTransitionHandler.SceneStates.INGAME
        //and in turn makes the players visible and allows for the players to be controlled.
        SceneTransitionHandler.sceneTransitionHandler.SetSceneState(SceneTransitionHandler.SceneStates.Ingame);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        base.OnNetworkSpawn();
    }

    private void OnClientConnected(ulong clientId)
    {
        if (m_ReplicatedTimeSent)
        {
            // Send the RPC only to the newly connected client
            SetReplicatedTimeRemainingClientRPC(m_TimeRemaining, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>() { clientId } } });
        }
    }

    /// <summary>
    ///     ShouldStartCountDown
    ///     Determines when the countdown should start
    /// </summary>
    /// <returns>true or false</returns>
    private bool ShouldStartCountDown()
    {
        //If the game has started, then don't bother with the rest of the count down checks.
        if (HasGameStarted()) return false;
        if (IsServer)
        {
            m_CountdownStarted.Value = SceneTransitionHandler.sceneTransitionHandler.AllClientsAreLoaded();

            //While we are counting down, continually set the replicated time remaining value for clients (client should only receive the update once)
            if (m_CountdownStarted.Value && !m_ReplicatedTimeSent)
            {
                SetReplicatedTimeRemainingClientRPC(m_DelayedStartTime);
                m_ReplicatedTimeSent = true;
            }

            return m_CountdownStarted.Value;
        }

        return m_ClientStartCountdown;
    }

    /// <summary>
    ///     We want to send only once the Time Remaining so the clients
    ///     will deal with updating it. For that, we use a ClientRPC
    /// </summary>
    /// <param name="delayedStartTime"></param>
    /// <param name="clientRpcParams"></param>
    [ClientRpc]
    private void SetReplicatedTimeRemainingClientRPC(float delayedStartTime, ClientRpcParams clientRpcParams = new ClientRpcParams())
    {
        // See the ShouldStartCountDown method for when the server updates the value
        if (m_TimeRemaining == 0)
        {
            Debug.LogFormat("Client side our first timer update value is {0}", delayedStartTime);
            m_TimeRemaining = delayedStartTime;
        }
        else
        {
            Debug.LogFormat("Client side we got an update for a timer value of {0} when we shouldn't", delayedStartTime);
        }
    }

    /// <summary>
    ///     IsCurrentGameOver
    ///     Returns whether the game is over or not
    /// </summary>
    /// <returns>true or false</returns>
    private bool IsCurrentGameOver()
    {
        if (IsServer)
            return isGameOver.Value;
        return m_ClientGameOver;
    }

    /// <summary>
    ///     HasGameStarted
    ///     Determine whether the game has started or not
    /// </summary>
    /// <returns>true or false</returns>
    private bool HasGameStarted()
    {
        if (IsServer)
            return hasGameStarted.Value;
        return m_ClientGameStarted;
    }

    /// <summary>
    ///     Client side we try to predictively update the gameTimer
    ///     as there shouldn't be a need to receive another update from the server
    ///     We only got the right m_TimeRemaining value when we started so it will be enough
    /// </summary>
    /// <returns> True when m_HasGameStared is set </returns>
    private void UpdateGameTimer()
    {
        if (!ShouldStartCountDown()) return;
        if (!HasGameStarted() && m_TimeRemaining > 0.0f)
        {
            m_TimeRemaining -= Time.deltaTime;

            if (IsServer && m_TimeRemaining <= 0.0f) // Only the server should be updating this
            {
                m_TimeRemaining = 0.0f;
                hasGameStarted.Value = true;
                OnGameStarted();
            }

            if (m_TimeRemaining > 0.1f)
                OverlayUI.Instance.ShowParaText(Mathf.FloorToInt(m_TimeRemaining).ToString());
        }
    }

    /// <summary>
    ///     OnGameStarted
    ///     Only invoked by the server, this hides the timer text and initializes the enemies and level
    /// </summary>
    private void OnGameStarted()
    {
        //CreateEnemies();
        //CreateShields();
        //CreateSuperEnemy();
    }

    public void ExitGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneTransitionHandler.sceneTransitionHandler.ExitAndLoadStartMenu();
    }

}
