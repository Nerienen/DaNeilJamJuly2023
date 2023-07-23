using ProjectUtils.Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance {  get; private set; }

    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform targetTransform;

    [SerializeField] float placingDuration = 0.5f;

    TimelinePlayer _timelinePlayer;

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(CutsceneController)} exists. This one has been eliminated");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion

        _timelinePlayer = GetComponent<TimelinePlayer>();
    }

    void PlayCutscene()
    {
        _timelinePlayer.Play();
    }

    public void PreparePlayer()
    {
        if (playerTransform != null && cameraTransform != null && targetTransform != null)
        {
            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();

            playerTransform.GetComponent<PlayerController>().enabled = false;
            playerRigidbody.isKinematic = true;
            playerRigidbody.velocity = Vector3.zero;
            cameraTransform.GetComponent<CameraController>().enabled = false;

            playerTransform.DoMove(targetTransform.position, placingDuration, Transitions.TimeScales.Scaled);
            playerTransform.RotateTo(targetTransform.rotation, placingDuration * 0.1f, Transitions.TimeScales.Scaled);

            Invoke(nameof(PlayCutscene), 1f);
        }
    }
}
