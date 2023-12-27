using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float normalTime;

    private ShootingButton shootingBtn;

    private void Start()
    {
        normalTime = Time.timeScale;
        StopTime();

        //Menonaktifkan Shooting Button untuk sementara
        shootingBtn = GameObject.Find("Shooting_button").GetComponent<ShootingButton>();
        shootingBtn.enabled = false;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;

    }

    public void ResetTime()
    {
        Time.timeScale = normalTime;
        shootingBtn.enabled = true;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
