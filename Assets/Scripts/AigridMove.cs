using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AigridMove : MonoBehaviour
{
    public enum VisionDirEnum { Up, Down, Left, Right }
    public VisionDirEnum visionDir;
    public Vector3 BeforeTargetPosition;
    public Vector3 BagianKanan;
    public Vector3 BagianKiri;
    public Vector3 BagianDepan;
    public Vector3 BagianBelakang;

    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public List<PlainEffect> effects;
    [SerializeField] public TextMeshProUGUI damages;
    [SerializeField] public TextMeshProUGUI cost;
    [SerializeField] public SpriteRenderer image;
    public CardData cardData;
    [SerializeField] public int attack;
    [SerializeField] public int Area;
    private Model model;

    [Header("Movement Settings")]
    public bool canMove;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public int action = 3;
    public LayerMask obstacleLayer;
    public float decisionInterval = 0.4f;
    [Header("Collision")]
    public LayerMask blockingLayers;

    [Header("Target & Combat")]
    public Transform playerTarget;
    public EnemyCard enemyCard;
    

    // Status
    public bool inAction = false;
    private int curAction;
    private float decisionTimer = 0f;
    private bool hasAttackedThisTurn = false;
    private bool isTurnEnding = false;

    public GameObject player;
    private PlayersStat playerStats;

    // Grid offset untuk tengah tile
    private const float GRID_OFFSET = 0.5f;

    void Start()
    {
        visionDir = VisionDirEnum.Right;
        enemyCard = GetComponent<EnemyCard>();
        canMove = false;
        curAction = action;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerStats = player.GetComponent<PlayersStat>();
            playerTarget = player.transform;
        }

        if (movePoint == null)
        {
            Debug.LogError("MovePoint belum diassign!");
            enabled = false;
            return;
        }

        movePoint.parent = null;

        // Snap posisi awal ke tengah grid (dengan offset 0.5)
        SnapToGridCenter();

        // Inisialisasi BagianDepan/Belakang/Kanan/Kiri dari posisi awal
        UpdateVisionPositions(transform.position);
    }

    public void setup(Model model)
    {
        this.model = model;
        //title.text = model.nama;
        //cost.text = model.cost;
        Area = model.Area;
        attack = model.damage;
        foreach (PlainEffect effect in model.effects)
        {

            effects = model.effects;
        }
        //image.sprite = model.gambar;
        //damages.text = model.damage.ToString();
    }
    void SnapToGridCenter()
    {
        // Hitung posisi grid dengan offset 0.5
        Vector3 snappedPos = GetGridCenterPosition(transform.position);

        transform.position = snappedPos;
        movePoint.position = snappedPos;

        Debug.Log($"Enemy snapped to grid center: {snappedPos}");
    }

    Vector3 GetGridCenterPosition(Vector3 worldPosition)
    {
        // Ambil koordinat grid integer
        int gridX = Mathf.FloorToInt(worldPosition.x);
        int gridY = Mathf.FloorToInt(worldPosition.y);

        // Tambah offset 0.5 untuk tengah tile
        return new Vector3(gridX + GRID_OFFSET, gridY + GRID_OFFSET, worldPosition.z);
    }

    Vector2Int GetGridCell(Vector3 worldPosition)
    {
        // Mengembalikan koordinat sel grid (tanpa offset)
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x),
            Mathf.FloorToInt(worldPosition.y)
        );
    }

    Vector3 GetWorldPositionFromGrid(Vector2Int gridCell)
    {
        // Konversi dari sel grid ke posisi dunia di tengah tile
        return new Vector3(
            gridCell.x + GRID_OFFSET,
            gridCell.y + GRID_OFFSET,
            transform.position.z
        );
    }

    void Update()
    {
        // Gerakkan musuh ke movePoint
        BeforeTargetPosition = transform.position;
        this.transform.position = Vector3.MoveTowards(
            transform.position,
            movePoint.position,
            moveSpeed * Time.deltaTime
        );

        // Pastikan posisi tepat di tengah tile saat idle
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.001f)
        {
            SnapToExactPosition();
        }

        // Cek apakah sudah sampai di grid tujuan
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            decisionTimer += Time.deltaTime;

            if (decisionTimer >= decisionInterval && curAction > 0)
            {
                if (canMove && curAction > 0)
                {
                    decisionTimer = 0f;
                    CalculateAndMove();
                }
            }
        }

        // Cek apakah action habis dan belum memanggil end turn
        if (curAction <= 0 && canMove && !isTurnEnding)
        {
            EndEnemyTurn();
        }
    }

    void SnapToExactPosition()
    {
        // Snap ke posisi yang tepat
        transform.position = movePoint.position;
    }

    void CalculateAndMove()
    {
        if (playerTarget == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTarget = player.transform;
                playerStats = player.GetComponent<PlayersStat>();
            }
            else
            {
                Debug.LogWarning("Player target not found!");
                curAction = 0;
                return;
            }
        }

        // Dapatkan sel grid saat ini dan player
        Vector2Int aiGridCell = GetGridCell(movePoint.position);
        Vector2Int playerGridCell = GetGridCell(playerTarget.position);

        int distance = ManhattanDistance(aiGridCell, playerGridCell);

        Debug.Log($"AI Grid Cell: {aiGridCell}, Player Grid Cell: {playerGridCell}, Distance: {distance}");

        // 1. Cek apakah bisa menyerang
        if (distance <= 1 && !hasAttackedThisTurn)
        {
            AttackPlayer();
            return;
        }

        // 2. Jika masih jauh, bergerak mendekat
        if (distance > 1 && curAction > 0)
        {
            MoveTowardsPlayer(aiGridCell, playerGridCell);
        }
    }

    void MoveTowardsPlayer(Vector2Int aiGridCell, Vector2Int playerGridCell)
    {
        inAction = true;
        Vector2Int bestMove = aiGridCell;
        int bestDistance = ManhattanDistance(aiGridCell, playerGridCell);
        bool foundValidMove = false;

        // Hitung perbedaan untuk menentukan prioritas arah
        Vector2Int diff = playerGridCell - aiGridCell;

        // Tentukan urutan arah berdasarkan prioritas
        Vector2Int[] priorityDirections = GetPriorityDirections(diff);

        // Coba setiap arah berdasarkan prioritas
        foreach (Vector2Int dir in priorityDirections)
        {
            Vector2Int targetGridCell = aiGridCell + dir;

            // Skip jika target adalah posisi player
            if (targetGridCell == playerGridCell)
            {
                continue;
            }

            // Konversi ke posisi dunia di tengah tile
            Vector3 targetWorldPos = GetWorldPositionFromGrid(targetGridCell);
            Collider2D hit = Physics2D.OverlapCircle(targetWorldPos, 0.2f, blockingLayers);
            if (hit == null || hit.gameObject == gameObject)
            {
                ExecuteMove(targetWorldPos);
                return;
            }
            // Cek tabrakan
            if (Physics2D.OverlapCircle(targetWorldPos, 0.2f, obstacleLayer))
            {
                Debug.Log($"Collision detected at grid cell: {targetGridCell}");
                continue;
            }

            // Hitung jarak baru ke player
            int newDistance = ManhattanDistance(targetGridCell, playerGridCell);

            // Pilih gerakan yang mengurangi jarak
            if (newDistance < bestDistance)
            {
                bestDistance = newDistance;
                bestMove = targetGridCell;
                foundValidMove = true;
                break; // Gunakan gerakan pertama yang valid
            }
        }

        // Jika ditemukan gerakan yang valid
        if (foundValidMove && bestMove != aiGridCell)
        {
            Vector3 targetWorldPos = GetWorldPositionFromGrid(bestMove);
            ExecuteMove(targetWorldPos);

            // Cek apakah setelah bergerak bisa menyerang
            Vector2Int newAiGridCell = GetGridCell(movePoint.position);
            int newDistance = ManhattanDistance(newAiGridCell, playerGridCell);

            if (newDistance <= 1 && !hasAttackedThisTurn && curAction > 0)
            {
                AttackPlayer();
            }
        }
        else if (!foundValidMove)
        {
            // Tidak bisa bergerak ke arah yang mengurangi jarak
            Debug.Log("No valid move found that reduces distance");

            // Coba gerakan alternatif
            TryAlternativeMove(aiGridCell, playerGridCell);
        }
    }

    Vector2Int[] GetPriorityDirections(Vector2Int diff)
    {
        Vector2Int[] directions;

        // Prioritas berdasarkan perbedaan terbesar
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            // Horizontal dulu
            if (diff.x > 0)
            {
                directions = new Vector2Int[] {
                    new Vector2Int(1, 0),   // Kanan
                    new Vector2Int(0, diff.y > 0 ? 1 : -1), // Atas/Bawah sesuai diff
                    new Vector2Int(0, diff.y > 0 ? -1 : 1), // Kebalikan
                    new Vector2Int(-1, 0)   // Kiri
                };
            }
            else
            {
                directions = new Vector2Int[] {
                    new Vector2Int(-1, 0),  // Kiri
                    new Vector2Int(0, diff.y > 0 ? 1 : -1), // Atas/Bawah
                    new Vector2Int(0, diff.y > 0 ? -1 : 1), // Kebalikan
                    new Vector2Int(1, 0)    // Kanan
                };
            }
        }
        else
        {
            // Vertikal dulu
            if (diff.y > 0)
            {
                directions = new Vector2Int[] {
                    new Vector2Int(0, 1),   // Atas
                    new Vector2Int(diff.x > 0 ? 1 : -1, 0), // Kanan/Kiri
                    new Vector2Int(diff.x > 0 ? -1 : 1, 0), // Kebalikan
                    new Vector2Int(0, -1)   // Bawah
                };
            }
            else
            {
                directions = new Vector2Int[] {
                    new Vector2Int(0, -1),  // Bawah
                    new Vector2Int(diff.x > 0 ? 1 : -1, 0), // Kanan/Kiri
                    new Vector2Int(diff.x > 0 ? -1 : 1, 0), // Kebalikan
                    new Vector2Int(0, 1)    // Atas
                };
            }
        }

        return directions;
    }

    void TryAlternativeMove(Vector2Int aiGridCell, Vector2Int playerGridCell)
    {
        // Coba semua arah secara berurutan
        Vector2Int[] allDirections = {
            new Vector2Int(1, 0),   // Kanan
            new Vector2Int(-1, 0),  // Kiri
            new Vector2Int(0, 1),   // Atas
            new Vector2Int(0, -1)   // Bawah
        };

        foreach (Vector2Int dir in allDirections)
        {
            Vector2Int targetGridCell = aiGridCell + dir;

            // Skip posisi player
            if (targetGridCell == playerGridCell) continue;

            Vector3 targetWorldPos = GetWorldPositionFromGrid(targetGridCell);

            Collider2D hit = Physics2D.OverlapCircle(targetWorldPos, 0.2f, blockingLayers);
            if (hit == null || hit.gameObject == gameObject)
            {
                ExecuteMove(targetWorldPos);
                return;
            }

        }

        // Jika benar-benar tidak bisa bergerak
        Debug.Log("Enemy stuck, cannot move");

        // Coba serang jika berdekatan
        int currentDistance = ManhattanDistance(aiGridCell, playerGridCell);
        if (currentDistance <= 1 && !hasAttackedThisTurn)
        {
            AttackPlayer();
        }
        else
        {
            // Skip turn
            curAction = 0;
        }
    }

    void ExecuteMove(Vector3 targetPosition)
    {
        targetPosition = GetGridCenterPosition(targetPosition);

        // Tentukan arah hadap berdasarkan arah gerak (gunakan movePoint, bukan BeforeTargetPosition)
        if      (targetPosition.y > movePoint.position.y) visionDir = VisionDirEnum.Up;
        else if (targetPosition.y < movePoint.position.y) visionDir = VisionDirEnum.Down;
        else if (targetPosition.x > movePoint.position.x) visionDir = VisionDirEnum.Right;
        else if (targetPosition.x < movePoint.position.x) visionDir = VisionDirEnum.Left;

        movePoint.position = targetPosition;
        curAction--;

        // Update semua posisi vision berdasarkan tile tujuan
        UpdateVisionPositions(targetPosition);

        Debug.Log($"Enemy moving to: {targetPosition}, visionDir: {visionDir}, Actions left: {curAction}");
    }

    void UpdateVisionPositions(Vector3 pos)
    {
        switch (visionDir)
        {
            case VisionDirEnum.Right:
                BagianDepan    = new Vector3(pos.x + 1f, pos.y,        pos.z);
                BagianBelakang = new Vector3(pos.x - 1f, pos.y,        pos.z);
                BagianKanan    = new Vector3(pos.x,       pos.y - 1f,  pos.z);
                BagianKiri     = new Vector3(pos.x,       pos.y + 1f,  pos.z);
                break;
            case VisionDirEnum.Left:
                BagianDepan    = new Vector3(pos.x - 1f, pos.y,        pos.z);
                BagianBelakang = new Vector3(pos.x + 1f, pos.y,        pos.z);
                BagianKanan    = new Vector3(pos.x,       pos.y + 1f,  pos.z);
                BagianKiri     = new Vector3(pos.x,       pos.y - 1f,  pos.z);
                break;
            case VisionDirEnum.Up:
                BagianDepan    = new Vector3(pos.x,       pos.y + 1f,  pos.z);
                BagianBelakang = new Vector3(pos.x,       pos.y - 1f,  pos.z);
                BagianKanan    = new Vector3(pos.x + 1f,  pos.y,       pos.z);
                BagianKiri     = new Vector3(pos.x - 1f,  pos.y,       pos.z);
                break;
            case VisionDirEnum.Down:
                BagianDepan    = new Vector3(pos.x,       pos.y - 1f,  pos.z);
                BagianBelakang = new Vector3(pos.x,       pos.y + 1f,  pos.z);
                BagianKanan    = new Vector3(pos.x - 1f,  pos.y,       pos.z);
                BagianKiri     = new Vector3(pos.x + 1f,  pos.y,       pos.z);
                break;
        }
    }

    void AttackPlayer()
    {
        inAction = true;
        if (playerStats != null)
        {
            Debug.Log($"Enemy attacking player with damage: {enemyCard.damage}");
            if (effects != null)
            {
                Debug.Log($"Applying {effects.Count} effects to player");
                foreach (PlainEffect effect in effects)
                {

                    //Debug.Log("Applying effect: " + effect.GetType().Name + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    Debug.Log(player.name + " " + enemyCard.gameObject.name + " " + attack + " ");
                    effect.OnBattle(player, enemyCard.gameObject, attack, UserType.enemy);
                    curAction--;
                    Debug.Log($"Enemy attacked! Damage: {enemyCard.damage}");
                }
           }
                    //Destroy(this.gameObject);
                
            Debug.Log($"Enemy attacked! Damage: {enemyCard.damage}");
        }

        curAction = 0;
        hasAttackedThisTurn = true;

        if (curAction <= 0 && canMove)
        {
            EndEnemyTurn();
        }
    }

    void EndEnemyTurn()
    {
        if (isTurnEnding) return;
        inAction = false;
        isTurnEnding = true;
        canMove = false;

        
        SnapToExactPosition();

        var turnSystem = GameObject.FindGameObjectWithTag("TurnSystem")?.GetComponent<TurnSystem>();
        if (turnSystem != null)
        {
            curAction = action;
            hasAttackedThisTurn = false;
            turnSystem.enemyEndTurn();
        }

        isTurnEnding = false;
    }

    
    private int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    
    public void ResetAction()
    {
        
        curAction = action;
        hasAttackedThisTurn = false;
        decisionTimer = 0f;
        isTurnEnding = false;

        Debug.Log("Enemy actions reset");
    }

    public void ResetAndMove()
    {
        ResetAction();
        canMove = true;
    }

    public void SetTarget(Transform newTarget)
    {
        playerTarget = newTarget;
        if (playerTarget != null)
        {
            playerStats = playerTarget.GetComponent<PlayersStat>();
        }
    }

    
    void OnDrawGizmosSelected()
    {
        if (movePoint != null)
        {
            // Gambar tile di posisi movePoint
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Vector3 tileCenter = GetGridCenterPosition(movePoint.position);
            Gizmos.DrawCube(tileCenter, new Vector3(0.95f, 0.95f, 0));

            // Gambar border tile
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(tileCenter, Vector3.one);

            if (playerTarget != null)
            {
                // Gambar garis ke player
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, playerTarget.position);

                // Gambar tile player
                Gizmos.color = new Color(0, 1, 0, 0.3f);
                Vector3 playerTileCenter = GetGridCenterPosition(playerTarget.position);
                Gizmos.DrawCube(playerTileCenter, new Vector3(0.95f, 0.95f, 0));

                // Gambar border tile player
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(playerTileCenter, Vector3.one);
            }
        }

        // Gambar tile enemy saat ini
        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Vector3 currentTileCenter = GetGridCenterPosition(transform.position);
        Gizmos.DrawCube(currentTileCenter, new Vector3(0.9f, 0.9f, 0));
    }
}