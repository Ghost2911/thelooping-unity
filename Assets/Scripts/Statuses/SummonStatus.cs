using UnityEngine;

public class SummonStatus : Status
{
    public int maxUnitCount = 3;
    public string unitPrefabPath = "Mobs/Skelet";
    private Unit[] summonUnits;
    private Vector3[] offsets;
    private float rotateAngle = 0;
    static readonly float radius = 4f;
    static readonly float attackRadius = 10f;
    private GameObject summonPrefab;
    private string attackingTag;

    public override void OnActivate()
    {
        summonUnits = new Unit[maxUnitCount];
        offsets = new Vector3[maxUnitCount];
        rotateAngle = Mathf.PI * 2 / maxUnitCount;
        summonPrefab = Resources.Load(unitPrefabPath) as GameObject;
        summonPrefab.tag = target.tag;
        attackingTag = target.CompareTag("enemy") ? "Player":"enemy";

        for (int i = 0; i < maxUnitCount; i++)
            offsets[i] = new Vector3(Mathf.Cos(i * rotateAngle) * radius, 0f, Mathf.Sin(i * rotateAngle) * radius);
    }

    public override void Tick()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius);
        Transform enemy = null;
        foreach (Collider hit in hitEnemies)
            if (hit.CompareTag(attackingTag)) { enemy = hit.transform; break; }

        for (int i = 0; i < maxUnitCount; i++)
        {
            if (summonUnits[i] == null)
            {
                Unit unit = Instantiate(summonPrefab, transform.position + offsets[i], Quaternion.identity).GetComponent<Unit>();
                summonUnits[i] = unit;
            }
            else
            {
                if (summonUnits[i].target == null)
                {
                    if (enemy != null)
                        summonUnits[i].SetTarget(enemy);
                    else
                        summonUnits[i].SetTarget(transform.position + offsets[i]);
                }
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < maxUnitCount; i++)
            if (summonUnits[i] != null) summonUnits[i].stats.Damage(999, 0, Vector3.zero, Color.white);
    }

    private void OnDrawGizmos()
    {
        float angle = Mathf.PI * 2 / maxUnitCount;
        Gizmos.color = Color.red;
        for (int i = 0; i < maxUnitCount; i++)
            Gizmos.DrawSphere(transform.position + offsets[i], 0.5f);
    }
}
