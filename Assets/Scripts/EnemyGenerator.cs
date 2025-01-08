using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using static EnemyGenerator;

public class EnemyGenerator : MonoBehaviour
{
    [Serializable]
    public class Enemy
    {
        public GameObject prefab;
        public MagicType type;
        public float hp;
        public float maxHP;  // readonlyを削除して、Inspectorで設定可能にする
        public MagicType lastHitType = MagicType.None;
        public int comboCount = 0;
        public string displayName;

        public float hpRate {
            get {
                if (maxHP <= 0) {
                    Debug.LogError($"MaxHP is invalid: {maxHP}");
                    return 0;
                }
                return hp / maxHP;
            }
        }

        // デフォルトコンストラクタ（UnityのSerializationに必要）
        public Enemy()
        {
        }

        // 既存のEnemyからコピーを作成するコンストラクタ
        public Enemy(Enemy source, GameObject newPrefab)
        {
            if (source == null)
            {
                Debug.LogError("Source enemy is null!");
                return;
            }

            if (source.maxHP <= 0)
            {
                Debug.LogError($"Invalid maxHP in source enemy: {source.maxHP}");
                return;
            }

            this.prefab = newPrefab;
            this.type = source.type;
            this.maxHP = source.maxHP;
            this.hp = this.maxHP;  // 初期HPは最大HPと同じ
            this.displayName = source.displayName;
            this.lastHitType = MagicType.None;
            this.comboCount = 0;

            Debug.Log($"Created new enemy: {displayName} with HP: {hp}/{maxHP}");
        }
    }

    [Header("必要コンポーネント")]
    [SerializeField] private Enemy _waterGolem;
    [SerializeField] private Enemy _leafGolem;
    [SerializeField] private Enemy _fireGolem;
    [SerializeField] private Enemy _iceDragon;
    [SerializeField] private Enemy _fireDragon;
    [SerializeField] private Transform _generateParent;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private LogLecorder _logLecorder;
    [SerializeField] private CharacterMove _characterMove;
    [SerializeField] private MagicController _magicController;

    [Header("UI")]
    [SerializeField] private GameObject clearText;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("パラメータ")]
    [SerializeField] private int _autoGenerateIntervalMS = 5;

    private int currentEnemyIndex = 0;
    private readonly Enemy[] enemySequence = new Enemy[5];

    public bool isExistEnemy => existEnemy != null;
    private Enemy existEnemy = null;

    private float startT = 0;
    private float knockDownT { get => Time.time - startT; }

    private bool generated = false;
    private bool isGameCleared = false;

    private void Start()
    {
        clearText.SetActive(false);
        InitializeEnemySequence();
        // 開始時は敵名を非表示に
        if (enemyNameText != null)
        {
            enemyNameText.gameObject.SetActive(false);
        }
    }

    private void InitializeEnemySequence()
    {
        enemySequence[0] = _waterGolem;
        enemySequence[1] = _leafGolem;
        enemySequence[2] = _fireGolem;
        enemySequence[3] = _iceDragon;
        enemySequence[4] = _fireDragon;
    }

    private void Update()
    {
        if (isGameCleared) return;

        if (_characterMove.MovementTimes == 2)
        {
            if (!generated) GenerateEnemy();
        }
    }

    private void UpdateEnemyName(Enemy enemy)
    {
        if (enemyNameText == null)
        {
            Debug.LogWarning("Enemy name text component is not assigned!");
            return;
        }

        if (enemy == null)
        {
            enemyNameText.gameObject.SetActive(false);
            return;
        }

        if (string.IsNullOrEmpty(enemy.displayName))
        {
            Debug.LogWarning($"Display name is not set for enemy type: {enemy.type}");
            enemyNameText.text = $"Unknown {enemy.type}";
        }
        else
        {
            enemyNameText.gameObject.SetActive(true);
            enemyNameText.text = enemy.displayName;
        }
    }

    public void GenerateEnemy()
    {
        if (existEnemy != null)
        {
            Debug.LogError("敵オブジェクトをまだ所有しています");
            return;
        }

        if (currentEnemyIndex >= enemySequence.Length)
        {
            ShowGameClear();
            return;
        }

        Enemy sourceEnemy = enemySequence[currentEnemyIndex];
        
        // maxHPの検証
        if (sourceEnemy.maxHP <= 0)
        {
            Debug.LogError($"Enemy {sourceEnemy.displayName} has invalid maxHP: {sourceEnemy.maxHP}. Please check Inspector settings.");
            return;
        }

        startT = Time.time;
        _logLecorder.SetEType(currentEnemyIndex);

        var enemyObj = Instantiate(sourceEnemy.prefab, _generateParent);
        enemyObj.transform.localPosition = Vector3.zero;

        // 新しい敵を生成
        existEnemy = new Enemy(sourceEnemy, enemyObj);
        
        // 生成後の値を確認
        Debug.Log($"Generated enemy: {existEnemy.displayName} with HP: {existEnemy.hp}/{existEnemy.maxHP}");

        _hpGauge.fillAmount = existEnemy.hpRate;
        generated = true;

        // 敵生成時に名前を表示
        UpdateEnemyName(existEnemy);
        
        // 敵生成時に入力遅延を開始
        if (_magicController != null)
        {
            _magicController.StartInputDelay();
        }
        else
        {
            Debug.LogWarning("MagicController is not assigned!");
        }
    }

    private void ShowGameClear()
    {
        isGameCleared = true;
        clearText.SetActive(true);
        // ゲームクリア時に敵名を非表示
        if (enemyNameText != null)
        {
            enemyNameText.gameObject.SetActive(false);
        }
        Debug.Log("Game Clear!");
    }

    public async void DestryoEnemy()
    {
        if (existEnemy == null)
        {
            Debug.LogError("敵オブジェクトが存在しません");
            return;
        }

        // 敵を倒す前に名前を非表示
        if (enemyNameText != null)
        {
            enemyNameText.gameObject.SetActive(false);
        }

        Destroy(existEnemy.prefab);
        existEnemy = null;

        _logLecorder.SetKnockOutTime(knockDownT);
        _logLecorder.Save();

        currentEnemyIndex++;

        if (currentEnemyIndex >= enemySequence.Length)
        {
            ShowGameClear();
            return;
        }

        await Task.Delay(3000);
        _characterMove.InitPosition();
        generated = false;
    }

    public void DamageEnemy(float dam, MagicType type)
    {
        if (existEnemy == null)
        {
            Debug.LogError("敵オブジェクトが存在しません");
            return;
        }

        float damageMultiplier = 1.0f;

        // 基本の属性相性
        switch (existEnemy.type)
        {
            case MagicType.Water:
                if (type == MagicType.Electric) damageMultiplier *= 2.0f;
                if (type == MagicType.Fire) damageMultiplier *= 0.75f;
                break;
                
            case MagicType.Leaf:
                if (type == MagicType.Fire) damageMultiplier *= 2.0f;
                if (type == MagicType.Electric) damageMultiplier *= 0.75f;
                break;
                
            case MagicType.Ice:
                if (type == MagicType.Fire) damageMultiplier *= 4.0f;
                if (type == MagicType.Electric) damageMultiplier *= 1.0f;
                break;
        }

        // Fire GolemとFire Dragonの特殊効果：コンボでダメージが増加
        if (existEnemy.prefab.name.Contains("Fire Golem") || existEnemy.prefab.name.Contains("Fire Dragon"))
        {
            if ((type == MagicType.Fire && existEnemy.lastHitType == MagicType.Electric) ||
                (type == MagicType.Electric && existEnemy.lastHitType == MagicType.Fire))
            {
                existEnemy.comboCount++;
                if (existEnemy.comboCount > 1)
                {
                    damageMultiplier *= (1 + existEnemy.comboCount * 1.0f);
                }
            }
            else
            {
                existEnemy.comboCount = 0;
            }
        }

        existEnemy.lastHitType = type;
        existEnemy.hp -= dam * damageMultiplier;
        
        // ダメージ計算後の状態をログ出力
        Debug.Log($"攻撃後 HP: {existEnemy.hp}/{existEnemy.maxHP}");

        _hpGauge.fillAmount = existEnemy.hpRate;

        if (existEnemy.hp <= 0) DestryoEnemy();
    }
}