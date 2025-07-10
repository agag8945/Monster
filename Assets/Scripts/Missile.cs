using UnityEngine;

public class Missile : MonoBehaviour
{
    // 미사일 이동 속도
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    public int missileDamage = 1;

    [SerializeField]
    GameObject Expeffect;

    public Vector3 direction = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
        if (transform.position.y > 7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject effect = Instantiate(Expeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
            Destroy(gameObject);
        }
    
    }
}
