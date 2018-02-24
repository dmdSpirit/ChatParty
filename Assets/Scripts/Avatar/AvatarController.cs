using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Main controlling component for Avatar gameObject. Supports the workflow of other components.
/// </summary>
[RequireComponent(typeof(AnimationController), typeof(MessageQueue), typeof(AvatarStats))]
public class AvatarController : MonoBehaviour {
    [HideInInspector]
    public GameObject spriteGO;
    [HideInInspector]
    public Viewer viewer;
    [HideInInspector]
    public int spriteSortOrder;
    [HideInInspector]
    public Animator spriteAnimator;
    [HideInInspector]
    public AvatarStats avatarStats;
    [HideInInspector]
    public AnimationController animationController;

    [SerializeField]
    Text nameText;

    MessageQueue messageQueue;

    private void Awake() {
        animationController = GetComponent<AnimationController>();
        messageQueue = GetComponent<MessageQueue>();
        avatarStats = GetComponent<AvatarStats>();
    }

    private void Start() {
        gameObject.name = viewer.Name;
        if (nameText == null)
            Logger.LogMessage($"{gameObject.name}::AvatarController -- nameText field is not set.", LogType.Error);
        else
            nameText.text = viewer.Name;
        string spriteName = ViewerBaseController.Instance.GetSpriteName(viewer);
        spriteGO = CreateSprite(spriteName);
        spriteAnimator = spriteGO.GetComponent<Animator>();
        avatarStats.LoadStats(avatarName: spriteName);
    }

    /// <summary>
    /// Destroys current Avatar.
    /// </summary>
    public void DestroyAvatar() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Adds message to Avatar message queue.
    /// </summary>
    /// <param name="message">Message.</param>
    public void AddMessage(string message) {
        messageQueue.AddMessage(message);
    }

    public void AddCommand(string command) {
        throw new NotImplementedException();
    }

    private GameObject CreateSprite(string spriteName) {
        GameObject spritePrefab = Resources.Load<GameObject>($"SpritePrefabs/{spriteName}");
        if (spritePrefab == null) {
            Logger.LogMessage($"{gameObject.name} :: could not find spritePrefab called {spriteName}.", LogType.Error);
            return null;
        }
        else {
            GameObject newSpriteGO = Instantiate(spritePrefab, transform);
            newSpriteGO.GetComponent<SpriteRenderer>().sortingOrder = spriteSortOrder;
            return newSpriteGO;
        }
    }
}
